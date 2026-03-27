using IndustrialAICopilot.Core.Enums;

namespace IndustrialAICopilot.Core.Models
{
    /// <summary>
    /// 定義 AI 服務的配置資訊
    /// </summary>
    public class AISettings
    {
        /// <summary>
        /// 目前選用的 AI 服務供應商。
        /// </summary>
        public AIProvider Provider { get; set; }

        /// <summary>
        /// 用於內容生成與對話的模型代碼名稱。
        /// </summary>
        public string CompletionModelName { get; set; }

        /// <summary>
        /// 用於文字向量化的模型代碼名稱。
        /// </summary>
        public string EmbeddingModelName { get; set; }

        /// <summary>
        /// 用於存取 AI 服務的 API 授權金鑰。
        /// </summary>
        public string ApiKey { get; set; }
    }
}
