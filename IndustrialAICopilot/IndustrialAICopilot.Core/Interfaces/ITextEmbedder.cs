using IndustrialAICopilot.Core.Models;

namespace IndustrialAICopilot.Core.Interfaces
{
    /// <summary>
    /// 定義文字向量化提取器的介面
    /// </summary>
    public interface ITextEmbedder
    {
        /// <summary>
        /// 是否支援處理。
        /// </summary>
        bool CanHandle(AISettings settings);

        /// <summary>
        /// 非同步向量化流程，將輸入文字轉換為浮點數陣列。
        /// </summary>
        Task<float[]> EmbedAsync(string text, AISettings settings);
    }
}
