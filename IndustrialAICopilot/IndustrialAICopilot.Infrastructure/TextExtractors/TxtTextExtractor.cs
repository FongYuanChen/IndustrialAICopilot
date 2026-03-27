using IndustrialAICopilot.Core.Interfaces;
using IndustrialAICopilot.Core.Models;
using System.Text;

namespace IndustrialAICopilot.Infrastructure.TextExtractors
{
    public class TxtTextExtractor : ITextExtractor
    {
        /// <summary>
        /// 支援的副檔名清單。
        /// </summary>
        public string[] SupportedExtensions => [".txt"];

        /// <summary>
        /// 是否支援處理。
        /// </summary>
        public bool CanHandle(AISettings settings) => true;

        /// <summary>
        /// 是否支援處理。
        /// </summary>
        public bool CanHandle(string extension)
            => SupportedExtensions.Any(supported => extension.Equals(supported, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// 非同步提取流程，從提供的檔案串流中讀取並解析出完整的純文字內容。
        /// </summary>
        public async Task<string> ExtractAsync(Stream stream, AISettings settings)
        {
            using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
            return await reader.ReadToEndAsync();
        }
    }
}
