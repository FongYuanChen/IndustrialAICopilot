using IndustrialAICopilot.Core.Models;

namespace IndustrialAICopilot.Core.Interfaces
{
    /// <summary>
    /// 定義知識庫的資料存取介面
    /// </summary>
    public interface IKnowledgeBaseRepository
    {
        /// <summary>
        /// 非同步執行資料庫初始化。
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// 非同步獲取所有文件資訊。
        /// </summary>
        Task<List<Document>> GetDocumentsAsync();

        /// <summary>
        /// 非同步獲取所有文件內容區塊資訊。
        /// </summary>
        Task<List<DocumentChunk>> GetDocumentChunksAsync();

        /// <summary>
        /// 非同步儲存原始文件資訊及切分後的內容區塊資訊。
        /// </summary>
        Task InsertDocumentAsync(Document document, IEnumerable<DocumentChunk> documentChunks);

        /// <summary>
        /// 非同步刪除指定文件。
        /// </summary>
        Task DeleteDocumentAsync(Guid documentId);

        /// <summary>
        /// 非同步更新文件內容區塊的文字向量陣列。
        /// </summary>
        Task UpdateDocumentChunkEmbeddingAsync(IEnumerable<(Guid ChunkId, float[] Vector)> updates);
    }
}
