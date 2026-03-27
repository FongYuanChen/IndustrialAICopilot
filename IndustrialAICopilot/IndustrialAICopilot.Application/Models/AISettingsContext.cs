using IndustrialAICopilot.Core.Enums;
using IndustrialAICopilot.Core.Models;

namespace IndustrialAICopilot.Application.Models
{
    /// <summary>
    /// AI 服務的配置上下文資訊
    /// </summary>
    public class AISettingsContext
    {
        /// <summary>
        /// 目前支援的所有 AI 供應商基礎資訊清單。
        /// </summary>
        public static List<AIProviderInfo> AvailableProviderInfos =>
            new List<AIProviderInfo>
            {
                new()
                {
                    Provider = AIProvider.Google,
                    ProviderDisplayName = "Google Gemini",
                    CompletionModels = new List<AIModelDefinition>
                    {
                        new() { Name = "gemini-3-flash-preview", DisplayName = "Gemini 3 Flash 預先發布版" },
                        new() { Name = "gemini-3.1-flash-lite-preview", DisplayName = "Gemini 3.1 Flash Lite 預先發布版" },
                        new() { Name = "gemini-2.5-flash", DisplayName = "Gemini 2.5 Flash" },
                        new() { Name = "gemini-2.5-flash-lite", DisplayName = "Gemini 2.5 Flash Lite" },
                    },
                    EmbeddingModels = new List<AIModelDefinition>
                    {
                        new() { Name = "models/gemini-embedding-2-preview", DisplayName = "Gemini Embedding 2 預先發布版" },
                        new() { Name = "models/gemini-embedding-001", DisplayName = "Gemini Embedding 1" }
                    }
                },
                new()
                {
                    Provider = AIProvider.OpenAI,
                    ProviderDisplayName = "OpenAI",
                    CompletionModels = new List<AIModelDefinition>
                    {
                        new() { Name = "gpt-4.1", DisplayName = "GPT-4.1" },
                        new() { Name = "gpt-4.1-nano", DisplayName = "GPT-4.1 Nano" },
                        new() { Name = "gpt-4o", DisplayName = "GPT-4o" },
                        new() { Name = "gpt-4o-mini", DisplayName = "GPT-4o Mini" },
                        new() { Name = "o4-mini", DisplayName = "o4-mini" }
                    },
                    EmbeddingModels = new List<AIModelDefinition>
                    {
                        new() { Name = "text-embedding-3-small", DisplayName = "Text Embedding 3 (Small)" },
                        new() { Name = "text-embedding-3-large", DisplayName = "Text Embedding 3 (Large)" }
                    }
                }
            };

        /// <summary>
        /// AI 服務的配置資訊是否有效
        /// </summary>
        public bool AISettingsIsValid =>
            AvailableProviderInfos.Any(info =>
                info.Provider == AISettings?.Provider &&
                info.CompletionModels.Any(model => model.Name == AISettings?.CompletionModelName) &&
                info.EmbeddingModels.Any(model => model.Name == AISettings?.EmbeddingModelName)
            );

        /// <summary>
        /// AI 服務的配置資訊
        /// </summary>
        public AISettings AISettings { get; set; }
    }
}
