namespace IndustrialAICopilot.Application.Interfaces
{
    /// <summary>
    /// 定義問題回應服務的介面
    /// </summary>
    public interface IQuestionResponder
    {
        /// <summary>
        /// 非同步執行完整的問題回應流程（包含提問向量化、知識庫語意比對、上下文組裝、AI 模型生成回覆）。
        /// </summary>
        Task<string> RespondAsync(string query);
    }
}
