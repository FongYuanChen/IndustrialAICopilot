using IndustrialAICopilot.Core.Models;

namespace IndustrialAICopilot.Application.Interfaces
{
    /// <summary>
    /// 定義知識庫管理服務的介面
    /// </summary>
    public interface IKnowledgeBaseManager
    {
        /// <summary>
        /// 獲取目前系統支援處理的文件副檔名清單。
        /// </summary>
        Task<List<string>> GetSupportedExtensionsAsync();

        /// <summary>
        /// 非同步獲取目前知識庫中所有的文件資訊。
        /// </summary>
        Task<List<Document>> GetDocumentsAsync();

        /// <summary>
        /// 非同步獲取目前知識庫中所有的文件區塊資訊。
        /// </summary>
        Task<List<DocumentChunk>> GetDocumentChunksAsync();

        /// <summary>
        /// 非同步匯入新文件，包含內容提取、文字切分、向量轉化與存儲。
        /// </summary>
        Task<(Document Document, List<DocumentChunk> Chunks)> IngestDocumentAsync(string name, long size, Stream stream);

        /// <summary>
        /// 非同步重建知識庫，重新計算所有現存文件的向量資訊。
        /// </summary>
        Task RebuildDocumentChunkEmbeddingsAsync();

        /// <summary>
        /// 非同步刪除指定文件及其關聯的所有資訊。
        /// </summary>
        Task DeleteDocumentAsync(Guid documentId);
    }
}
