namespace IndustrialAICopilot.Core.Models
{
    /// <summary>
    /// 定義文件經過切分後的區塊資訊
    /// </summary>
    public class DocumentChunk
    {
        /// <summary>
        /// 區塊的唯一識別碼。
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 所屬文件的識別碼。
        /// </summary>
        public Guid DocumentId { get; set; }

        /// <summary>
        /// 區塊的純文字內容。
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 區塊的文字向量陣列（用於語意搜尋）。
        /// </summary>
        public float[] Vector { get; set; }
    }
}
