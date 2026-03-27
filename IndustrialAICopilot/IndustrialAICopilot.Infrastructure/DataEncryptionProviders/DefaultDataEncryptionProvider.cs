using IndustrialAICopilot.Core.Interfaces;
using Microsoft.AspNetCore.DataProtection;

namespace IndustrialAICopilot.Infrastructure.DataProtectors
{
    public class DefaultDataEncryptionProvider : IDataEncryptionProvider
    {
        private readonly IDataProtector _dataProtector;

        public DefaultDataEncryptionProvider(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtector = dataProtectionProvider.CreateProtector(nameof(DefaultDataEncryptionProvider));
        }

        /// <summary>
        /// 將明文字串轉換為加密後的密文字串。
        /// </summary>
        public string Encrypt(string plainText)
            => string.IsNullOrEmpty(plainText) ? string.Empty : _dataProtector.Protect(plainText);

        /// <summary>
        /// 將加密後的密文字串還原為原始明文字串。
        /// </summary>
        public string Decrypt(string cipherText)
            => string.IsNullOrEmpty(cipherText) ? string.Empty : _dataProtector.Unprotect(cipherText);
    }
}
