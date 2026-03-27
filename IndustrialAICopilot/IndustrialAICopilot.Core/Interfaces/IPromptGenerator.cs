using IndustrialAICopilot.Core.Models;

namespace IndustrialAICopilot.Core.Interfaces
{
    /// <summary>
    /// 定義提示詞生成器的介面
    /// </summary>
    public interface IPromptGenerator
    {
        /// <summary>
        /// 是否支援處理。
        /// </summary>
        bool CanHandle(AISettings settings);

        /// <summary>
        /// 非同步生成流程，根據使用者提問與知識庫檢索結果，依照樣板產出提示指令。
        /// </summary>
        Task<string> GenerateAsync(string query, string context, AISettings settings);
    }
}
