using IndustrialAICopilot.Core.Enums;
using IndustrialAICopilot.Core.Interfaces;
using IndustrialAICopilot.Core.Models;
using OpenAI;

namespace IndustrialAICopilot.Infrastructure.TextEmbedders
{
    public class OpenAITextEmbedder : ITextEmbedder
    {
        /// <summary>
        /// 是否支援處理。
        /// </summary>
        public bool CanHandle(AISettings settings)
            => settings.Provider == AIProvider.OpenAI;

        /// <summary>
        /// 非同步向量化流程，將輸入文字轉換為浮點數陣列。
        /// </summary>
        public async Task<float[]> EmbedAsync(string text, AISettings settings)
        {
            var openAIClient = new OpenAIClient(settings.ApiKey);
            var embeddingClient = openAIClient.GetEmbeddingClient(settings.EmbeddingModelName);
            var embedding = (await embeddingClient.GenerateEmbeddingAsync(text)).Value;
            return embedding.ToFloats().ToArray();
        }
    }
}
