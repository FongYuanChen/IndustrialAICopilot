using IndustrialAICopilot.Core.Enums;
using IndustrialAICopilot.Core.Interfaces;
using IndustrialAICopilot.Core.Models;
using Microsoft.Data.Sqlite;

namespace IndustrialAICopilot.Infrastructure.AISettingsRepositories
{
    public class SqliteAISettingsRepository : IAISettingsRepository
    {
        private readonly SqliteConnectionStringBuilder _connectionStringBuilder =
            new SqliteConnectionStringBuilder
            {
                DataSource = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AISettingsRepository.db")
            };

        /// <summary>
        /// 非同步執行資料庫初始化。
        /// </summary>
        public async Task InitializeAsync()
        {
            await using var connection = new SqliteConnection(_connectionStringBuilder.ConnectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS AISettings (
                        Id INTEGER PRIMARY KEY CHECK (Id = 1),
                        Provider INTEGER NOT NULL,
                        CompletionModelName TEXT NOT NULL,
                        EmbeddingModelName TEXT NOT NULL,
                        ApiKey TEXT NOT NULL
                    );
                ";

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// 非同步讀取目前的 AI 服務配置資訊。
        /// </summary>
        public async Task<AISettings> GetAsync()
        {
            await using var connection = new SqliteConnection(_connectionStringBuilder.ConnectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = 
                @"
                    SELECT Provider, CompletionModelName, EmbeddingModelName, ApiKey 
                    FROM AISettings 
                    WHERE Id = 1;
                ";

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new AISettings
                {
                    Provider = (AIProvider)reader.GetInt32(0),
                    CompletionModelName = reader.GetString(1),
                    EmbeddingModelName = reader.GetString(2),
                    ApiKey = reader.GetString(3)
                };
            }

            return null;
        }

        /// <summary>
        /// 非同步更新 AI 服務配置資訊。
        /// </summary>
        public async Task UpdateAsync(AISettings settings)
        {
            await using var connection = new SqliteConnection(_connectionStringBuilder.ConnectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = 
                @"
                    INSERT INTO AISettings (Id, Provider, CompletionModelName, EmbeddingModelName, ApiKey)
                    VALUES (1, @provider, @completionModelName, @embeddingModelName, @apiKey)
                    ON CONFLICT(Id) DO UPDATE SET
                        Provider = excluded.Provider,
                        CompletionModelName = excluded.CompletionModelName,
                        EmbeddingModelName = excluded.EmbeddingModelName,
                        ApiKey = excluded.ApiKey;
                ";

            command.Parameters.Add("@provider", SqliteType.Integer).Value = (int)settings.Provider;
            command.Parameters.Add("@completionModelName", SqliteType.Text).Value = settings.CompletionModelName ?? string.Empty;
            command.Parameters.Add("@embeddingModelName", SqliteType.Text).Value = settings.EmbeddingModelName ?? string.Empty;
            command.Parameters.Add("@apiKey", SqliteType.Text).Value = settings.ApiKey ?? string.Empty;

            await command.ExecuteNonQueryAsync();
        }
    }
}
