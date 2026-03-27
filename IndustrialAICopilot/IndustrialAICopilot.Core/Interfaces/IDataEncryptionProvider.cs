namespace IndustrialAICopilot.Core.Interfaces
{
    /// <summary>
    /// 定義敏感資料的加密與解密提供者介面
    /// </summary>
    public interface IDataEncryptionProvider
    {
        /// <summary>
        /// 將明文字串轉換為加密後的密文字串。
        /// </summary>
        string Encrypt(string plainText);

        /// <summary>
        /// 將加密後的密文字串還原為原始明文字串。
        /// </summary>
        string Decrypt(string cipherText);
    }
}
