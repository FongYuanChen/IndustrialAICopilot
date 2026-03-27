using IndustrialAICopilot.Application.Models;
using IndustrialAICopilot.Core.Models;

namespace IndustrialAICopilot.Application.Utilities
{
    public static class AISettingsExtensions
    {
        public static AISettingsContext ConvertToContext(this AISettings settings)
        {
            return new AISettingsContext
            {
                AISettings = settings.Clone()
            };
        }

        public static AISettings Clone(this AISettings settings)
        {
            return new AISettings
            {
                Provider = settings.Provider,
                CompletionModelName = settings.CompletionModelName,
                EmbeddingModelName = settings.EmbeddingModelName,
                ApiKey = settings.ApiKey
            };
        }
    }
}
