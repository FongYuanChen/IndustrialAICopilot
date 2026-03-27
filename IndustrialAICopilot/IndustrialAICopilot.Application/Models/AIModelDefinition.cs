namespace IndustrialAICopilot.Application.Models
{
    /// <summary>
    /// 定義 AI 模型的基礎資訊
    /// </summary>
    public class AIModelDefinition
    {
        /// <summary>
        /// 模型代碼名稱，用於發送 API 請求時的模型參數。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 使用者介面上顯示的名稱。
        /// </summary>
        public string DisplayName { get; set; }
    }
}
