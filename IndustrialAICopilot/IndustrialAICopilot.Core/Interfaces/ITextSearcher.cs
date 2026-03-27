using IndustrialAICopilot.Core.Models;

namespace IndustrialAICopilot.Core.Interfaces
{
    /// <summary>
    /// 定義向量存儲器的介面
    /// </summary>
    public interface ITextSearcher
    {
        /// <summary>
        /// 是否支援處理。
        /// </summary>
        bool CanHandle(AISettings settings);

        /// <summary>
        /// 非同步搜尋流程，根據輸入的查詢向量，從索引中找出最相似的前 K 筆原始文字內容。
        /// </summary>
        Task<string[]> SearchAsync(DocumentChunk[] documentChunk, float[] queryVector, int topK, AISettings settings);
    }
}
