using Google.GenAI;
using Google.GenAI.Types;
using IndustrialAICopilot.Core.Enums;
using IndustrialAICopilot.Core.Interfaces;
using IndustrialAICopilot.Core.Models;

namespace IndustrialAICopilot.Infrastructure.PromptChatters
{
    public class GeminiTextGenerator : ITextGenerator
    {
        /// <summary>
        /// 是否支援處理。
        /// </summary>
        public bool CanHandle(AISettings settings)
            => settings.Provider == AIProvider.Google;

        /// <summary>
        /// 非同步生成流程，根據輸入的完整提示詞指令，由 AI 模型產出對應的文字結果。
        /// </summary>
        public async Task<string> GenerateAsync(string prompt, AISettings settings)
        {
            var client = new Client(apiKey: settings.ApiKey);
            var generateConfig = new GenerateContentConfig
            {
                Temperature = InternalAISettings.Temperature,
                TopP = InternalAISettings.TopP,
                TopK = InternalAISettings.TopK,
                MaxOutputTokens = InternalAISettings.MaxOutputTokens
            };
            var text = (await client.Models.GenerateContentAsync(settings.CompletionModelName, prompt, generateConfig)).Text;
            return text;
        }
    }
}
