using IndustrialAICopilot.Core.Enums;

namespace IndustrialAICopilot.Application.Models
{
    /// <summary>
    /// 定義 AI 服務供應商的基礎資訊
    /// </summary>
    public class AIProviderInfo
    {
        /// <summary>
        /// 供應商。
        /// </summary>
        public AIProvider Provider { get; set; }

        /// <summary>
        /// 使用者介面上顯示的供應商名稱。
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        /// 用於內容生成與對話的模型列表。
        /// </summary>
        public List<AIModelDefinition> CompletionModels { get; set; }

        /// <summary>
        /// 用於文字向量化的模型列表。
        /// </summary>
        public List<AIModelDefinition> EmbeddingModels { get; set; }
    }
}
