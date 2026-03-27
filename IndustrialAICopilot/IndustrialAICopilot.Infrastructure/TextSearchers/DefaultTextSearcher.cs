using IndustrialAICopilot.Core.Interfaces;
using IndustrialAICopilot.Core.Models;
using IndustrialAICopilot.Infrastructure.Utilities;

namespace IndustrialAICopilot.Infrastructure.Repositories
{
    public class DefaultTextSearcher : ITextSearcher
    {
        /// <summary>
        /// 是否支援處理。
        /// </summary>
        public bool CanHandle(AISettings settings) => true;

        /// <summary>
        /// 非同步搜尋流程，根據輸入的查詢向量，從索引中找出最相似的前 K 筆原始文字內容。
        /// </summary>
        public async Task<string[]> SearchAsync(DocumentChunk[] documentChunk, float[] queryVector, int topK, AISettings settings)
        {
            return documentChunk
                .Select(chunk => new
                {
                    Content = chunk.Content,
                    Score = VectorHelper.CosineSimilarity(queryVector, chunk.Vector)
                })
                .OrderByDescending(x => x.Score)
                .Take(topK)
                .Select(x => x.Content)
                .ToArray();
        }
    }
}
