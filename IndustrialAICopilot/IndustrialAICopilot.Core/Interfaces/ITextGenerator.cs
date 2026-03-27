using IndustrialAICopilot.Core.Models;

namespace IndustrialAICopilot.Core.Interfaces
{
    /// <summary>
    /// 定義文字生成器的介面
    /// </summary>
    public interface ITextGenerator
    {
        /// <summary>
        /// 是否支援處理。
        /// </summary>
        bool CanHandle(AISettings settings);

        /// <summary>
        /// 非同步生成流程，根據輸入的完整提示詞指令，由 AI 模型產出對應的文字結果。
        /// </summary>
        Task<string> GenerateAsync(string prompt, AISettings settings);
    }
}
