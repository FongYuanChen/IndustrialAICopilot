using IndustrialAICopilot.Core.Models;

namespace IndustrialAICopilot.Core.Interfaces
{
    /// <summary>
    /// 定義文字提取器的介面
    /// </summary>
    public interface ITextExtractor
    {
        /// <summary>
        /// 支援的副檔名清單。
        /// </summary>
        string[] SupportedExtensions { get; }

        /// <summary>
        /// 是否支援處理。
        /// </summary>
        bool CanHandle(AISettings settings);

        /// <summary>
        /// 是否支援處理。
        /// </summary>
        bool CanHandle(string extension);

        /// <summary>
        /// 非同步提取流程，從提供的檔案串流中讀取並解析出完整的純文字內容。
        /// </summary>
        Task<string> ExtractAsync(Stream stream, AISettings settings);
    }
}
