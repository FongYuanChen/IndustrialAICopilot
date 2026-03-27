using IndustrialAICopilot.Core.Models;

namespace IndustrialAICopilot.Core.Interfaces
{
    /// <summary>
    /// 定義文字切分器的介面
    /// </summary>
    public interface ITextSplitter
    {
        /// <summary>
        /// 是否支援處理。
        /// </summary>
        bool CanHandle(AISettings settings);

        /// <summary>
        /// 非同步切分流程，將文字分割為字串陣列。
        /// </summary>
        Task<string[]> Split(string text, AISettings settings);
    }
}
