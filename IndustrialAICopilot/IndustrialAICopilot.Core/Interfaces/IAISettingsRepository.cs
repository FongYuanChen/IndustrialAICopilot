using IndustrialAICopilot.Core.Models;

namespace IndustrialAICopilot.Core.Interfaces
{
    /// <summary>
    /// 定義 AI 服務設定的資料存取介面
    /// </summary>
    public interface IAISettingsRepository
    {
        /// <summary>
        /// 非同步執行資料庫初始化。
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// 非同步讀取目前的 AI 服務配置資訊。
        /// </summary>
        Task<AISettings> GetAsync();

        /// <summary>
        /// 非同步更新 AI 服務配置資訊。
        /// </summary>
        Task UpdateAsync(AISettings settings);
    }
}
