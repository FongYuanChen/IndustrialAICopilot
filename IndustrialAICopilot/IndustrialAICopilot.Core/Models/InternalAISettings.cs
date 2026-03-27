namespace IndustrialAICopilot.Core.Models
{
    /// <summary>
    /// 定義 AI 服務的配置資訊 (內部用)
    /// </summary>
    public static class InternalAISettings
    {
        /// <summary>
        /// 控制回應的隨機性。
        /// 數值越低，回應越趨向穩定與確定；數值越高，回應則越具創意與多樣性。
        /// </summary>
        public static float Temperature => 0.1f;

        /// <summary>
        /// 控制回應的「選字範圍（核取樣）」。
        /// 僅從累計機率加總達到 P 比例的候選詞中選字，過濾掉機率極低的冷門詞。
        /// </summary>
        public static float TopP => 0.95f;

        /// <summary>
        /// 限制候選詞的「固定數量」。
        /// 每次選字只考慮機率最高的前 K 個詞。
        /// </summary>
        public static int TopK => 40;

        /// <summary>
        /// 單次回應的「長度上限」。
        /// 限制輸出的文字量，避免消耗過多 Token 成本或產生過長的廢話。
        /// </summary>
        public static int MaxOutputTokens => 2048;
    }
}
