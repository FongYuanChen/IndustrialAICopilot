using IndustrialAICopilot.Application.Models;

namespace IndustrialAICopilot.Application.Interfaces
{
    /// <summary>
    /// 定義 AI 服務設定管理的介面
    /// </summary>
    public interface IAISettingsManager
    {
        /// <summary>
        /// 非同步獲取目前的 AI 服務的配置上下文資訊。
        /// </summary>
        Task<AISettingsContext> GetContextAsync();

        /// <summary>
        /// 非同步更新 AI 服務的配置上下文資訊。
        /// </summary>
        Task UpdateContextAsync(AISettingsContext settingsContext);

        /// <summary>
        /// 當上下文資訊更新後觸發的事件。
        /// </summary>
        event Action<AISettingsContext> OnContextChanged;
    }
}
