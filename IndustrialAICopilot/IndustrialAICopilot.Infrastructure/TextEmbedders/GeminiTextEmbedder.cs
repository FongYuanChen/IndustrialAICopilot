using Google.GenAI;
using IndustrialAICopilot.Core.Enums;
using IndustrialAICopilot.Core.Interfaces;
using IndustrialAICopilot.Core.Models;

namespace IndustrialAICopilot.Infrastructure.TextEmbedders
{
    public class GeminiTextEmbedder : ITextEmbedder
    {
        /// <summary>
        /// 是否支援處理。
        /// </summary>
        public bool CanHandle(AISettings settings)
            => settings.Provider == AIProvider.Google;

        /// <summary>
        /// 非同步向量化流程，將輸入文字轉換為浮點數陣列。
        /// </summary>
        public async Task<float[]> EmbedAsync(string text, AISettings settings)
        {
            var client = new Client(apiKey: settings.ApiKey);
            var embedding = (await client.Models.EmbedContentAsync(settings.EmbeddingModelName, text)).Embeddings[0].Values;
            return embedding.Select(value => (float)value).ToArray();
        }
    }
}