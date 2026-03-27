using IndustrialAICopilot.Core.Enums;
using IndustrialAICopilot.Core.Interfaces;
using IndustrialAICopilot.Core.Models;
using OpenAI;
using OpenAI.Chat;

namespace IndustrialAICopilot.Infrastructure.PromptChatters
{
    public class OpenAITextGenerator : ITextGenerator
    {
        /// <summary>
        /// 是否支援處理。
        /// </summary>
        public bool CanHandle(AISettings settings)
            => settings.Provider == AIProvider.OpenAI;

        /// <summary>
        /// 非同步生成流程，根據輸入的完整提示詞指令，由 AI 模型產出對應的文字結果。
        /// </summary>
        public async Task<string> GenerateAsync(string prompt, AISettings settings)
        {
            var openAIClient = new OpenAIClient(settings.ApiKey);
            var chatClient = openAIClient.GetChatClient(settings.CompletionModelName);
            var chatMessages = new List<ChatMessage>()
            {
                new UserChatMessage(prompt)
            };
            var chatCompletionOptions = new ChatCompletionOptions
            {
                Temperature = InternalAISettings.Temperature,
                TopP = InternalAISettings.TopP,
                MaxOutputTokenCount = InternalAISettings.MaxOutputTokens                 
            };
            var chatCompletion = (await chatClient.CompleteChatAsync(chatMessages, chatCompletionOptions)).Value;
            return chatCompletion.Content[0].Text;
        }
    }
}
