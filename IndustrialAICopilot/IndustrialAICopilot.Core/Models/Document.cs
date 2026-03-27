namespace IndustrialAICopilot.Core.Models
{
    /// <summary>
    /// 定義知識庫中的原始文件資訊
    /// </summary>
    public class Document
    {
        /// <summary>
        /// 文件的唯一識別碼。
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 文件的完整名稱（包含副檔名，如 "report.pdf"）。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文件大小（以位元組 Byte 為單位）。
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 文件匯入系統的時間。
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
