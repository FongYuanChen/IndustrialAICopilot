using IndustrialAICopilot.Core.Interfaces;
using IndustrialAICopilot.Core.Models;
using IndustrialAICopilot.Infrastructure.Utilities;
using Microsoft.Data.Sqlite;

namespace IndustrialAICopilot.Infrastructure.DocumentRepositories
{
    public class SqliteKnowledgeBaseRepository : IKnowledgeBaseRepository
    {
        private readonly SqliteConnectionStringBuilder _connectionStringBuilder =
            new SqliteConnectionStringBuilder
            {
                DataSource = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "KnowledgeBaseRepository.db"),
                ForeignKeys = true
            };

        /// <summary>
        /// 非同步執行資料庫初始化。
        /// </summary>
        public async Task InitializeAsync()
        {
            await using var connection = await CreateConnectionAsync();

            await using var command = connection.CreateCommand();
            command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS Documents (
                        Id TEXT PRIMARY KEY,
                        Name TEXT NOT NULL,
                        Size INTEGER NOT NULL,
                        CreatedAt TEXT NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS DocumentChunks (
                        Id TEXT PRIMARY KEY,
                        DocumentId TEXT NOT NULL,
                        Content TEXT NOT NULL,
                        Vector BLOB NOT NULL,
                        FOREIGN KEY (DocumentId) REFERENCES Documents(Id) ON DELETE CASCADE
                    );

                    CREATE INDEX IF NOT EXISTS idx_documents_name ON Documents(Name);
                    CREATE INDEX IF NOT EXISTS idx_chunks_document ON DocumentChunks(DocumentId);
                ";

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// 非同步獲取所有文件資訊。
        /// </summary>
        public async Task<List<Document>> GetDocumentsAsync()
        {
            var datum = new List<Document>();

            await using var connection = await CreateConnectionAsync();
            await using var command = connection.CreateCommand();

            command.CommandText =
                @"
                    SELECT Id, Name, Size, CreatedAt 
                    FROM Documents 
                    ORDER BY CreatedAt DESC;
                ";

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                datum.Add(new Document
                {
                    Id = Guid.Parse(reader.GetString(0)),
                    Name = reader.GetString(1),
                    Size = reader.GetInt64(2),
                    CreatedAt = DateTime.Parse(reader.GetString(3))
                });
            }

            return datum;
        }

        /// <summary>
        /// 非同步獲取所有文件內容區塊資訊。
        /// </summary>
        public async Task<List<DocumentChunk>> GetDocumentChunksAsync()
        {
            var datum = new List<DocumentChunk>();

            await using var connection = await CreateConnectionAsync();
            await using var command = connection.CreateCommand();

            command.CommandText = "SELECT Id, DocumentId, Content, Vector FROM DocumentChunks";

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                datum.Add(new DocumentChunk
                {
                    Id = reader.GetGuid(0),
                    DocumentId = reader.GetGuid(1),
                    Content = reader.GetString(2),
                    Vector = VectorConverter.ToFloatArray((byte[])reader.GetValue(3))
                });
            }

            return datum;
        }

        /// <summary>
        /// 非同步儲存原始文件資訊及切分後的內容區塊資訊。
        /// </summary>
        public async Task InsertDocumentAsync(Document document, IEnumerable<DocumentChunk> chunks)
        {
            await using var connection = await CreateConnectionAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // Insert Document
                await using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        @"
                            INSERT INTO Documents (Id, Name, Size, CreatedAt)
                            VALUES ($id, $name, $size, $createdAt);
                        ";

                    command.Parameters.Add("$id", SqliteType.Text).Value = document.Id.ToString();
                    command.Parameters.Add("$name", SqliteType.Text).Value = document.Name;
                    command.Parameters.Add("$size", SqliteType.Integer).Value = document.Size;
                    command.Parameters.Add("$createdAt", SqliteType.Text).Value = document.CreatedAt.ToString("o");

                    await command.ExecuteNonQueryAsync();
                }

                // Insert Chunks
                await using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        @"
                            INSERT INTO DocumentChunks (Id, DocumentId, Content, Vector)
                            VALUES ($id, $docId, $content, $vector);
                        ";  

                    var idParameter = command.Parameters.Add("$id", SqliteType.Text);
                    var docIdParameter = command.Parameters.Add("$docId", SqliteType.Text);
                    var contentParameter = command.Parameters.Add("$content", SqliteType.Text);
                    var vectorParameter = command.Parameters.Add("$vector", SqliteType.Blob);

                    foreach (var chunk in chunks)
                    {
                        idParameter.Value = chunk.Id.ToString();
                        docIdParameter.Value = chunk.DocumentId.ToString();
                        contentParameter.Value = chunk.Content ?? string.Empty;
                        vectorParameter.Value = VectorConverter.ToBytes(chunk.Vector);

                        await command.ExecuteNonQueryAsync();
                    }
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// 非同步刪除指定文件。
        /// </summary>
        public async Task DeleteDocumentAsync(Guid documentId)
        {
            await using var connection = await CreateConnectionAsync();
            await using var command = connection.CreateCommand();

            command.CommandText = "DELETE FROM Documents WHERE Id = $id;";
            command.Parameters.Add("$id", SqliteType.Text).Value = documentId.ToString();

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// 非同步更新文件內容區塊的文字向量陣列。
        /// </summary>
        public async Task UpdateDocumentChunkEmbeddingAsync(IEnumerable<(Guid ChunkId, float[] Vector)> updates)
        {
            await using var connection = await CreateConnectionAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                await using var command = connection.CreateCommand();
                command.CommandText = "UPDATE DocumentChunks SET Vector = $vector WHERE Id = $id;";

                var vectorParameter = command.Parameters.Add("$vector", SqliteType.Blob);
                var idParameter = command.Parameters.Add("$id", SqliteType.Text);

                foreach (var update in updates)
                {
                    vectorParameter.Value = VectorConverter.ToBytes(update.Vector);
                    idParameter.Value = update.ChunkId.ToString();

                    await command.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        #region 私有方法

        private async Task<SqliteConnection> CreateConnectionAsync()
        {
            var connection = new SqliteConnection(_connectionStringBuilder.ConnectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = 
                @"
                    PRAGMA journal_mode=WAL;
                    PRAGMA synchronous=NORMAL;
                    PRAGMA busy_timeout=5000;
                ";
            await command.ExecuteNonQueryAsync();

            return connection;
        }

        #endregion
    }
}
