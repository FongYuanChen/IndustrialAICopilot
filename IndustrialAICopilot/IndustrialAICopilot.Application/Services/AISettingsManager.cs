using IndustrialAICopilot.Application.Interfaces;
using IndustrialAICopilot.Application.Models;
using IndustrialAICopilot.Application.Utilities;
using IndustrialAICopilot.Core.Interfaces;
using IndustrialAICopilot.Core.Models;

namespace IndustrialAICopilot.Application.Services
{
    public class AISettingsManager : IAISettingsManager
    {
        private readonly IAISettingsRepository _aiSettingsRepository;
        private readonly IDataEncryptionProvider _dataEncryptionProvider;

        private AISettings _aiSettingsCache;
        private readonly SemaphoreSlim _lock = new(1, 1);

        public event Action<AISettingsContext> OnContextChanged;

        public AISettingsManager(IAISettingsRepository aiSettingsRepository,
                                 IDataEncryptionProvider dataEncryptionProvider)
        {
            _aiSettingsRepository = aiSettingsRepository;
            _dataEncryptionProvider = dataEncryptionProvider;
        }

        /// <summary>
        /// 非同步獲取目前的 AI 服務的配置上下文資訊。
        /// 優先從快取讀取，若無快取則從儲存層載入並自動執行敏感資訊解密。
        /// </summary>
        public async Task<AISettingsContext> GetContextAsync()
        {
            if (_aiSettingsCache != null)
            {
                return _aiSettingsCache.ConvertToContext();
            }

            await _lock.WaitAsync();
            try
            {
                if (_aiSettingsCache != null)
                    return _aiSettingsCache.ConvertToContext();

                var decryptSettings = await _aiSettingsRepository.GetAsync();
                if (decryptSettings != null)
                {
                    if (!string.IsNullOrEmpty(decryptSettings.ApiKey))
                    {
                        decryptSettings.ApiKey = _dataEncryptionProvider.Decrypt(decryptSettings.ApiKey);
                    }
                    _aiSettingsCache = decryptSettings;
                    return _aiSettingsCache.ConvertToContext();
                }

                return new AISettingsContext();
            }
            finally
            {
                _lock.Release();
            }
        }

        /// <summary>
        /// 非同步更新 AI 服務的配置上下文資訊。
        /// 自動執行敏感資訊加密、持久化儲存，並同步更新記憶體快取。
        /// </summary>
        public async Task UpdateContextAsync(AISettingsContext settingsContext)
        {
            await _lock.WaitAsync();
            try
            {
                var encryptedSettings = settingsContext.AISettings.Clone();
                if (!string.IsNullOrEmpty(encryptedSettings.ApiKey))
                {
                    encryptedSettings.ApiKey = _dataEncryptionProvider.Encrypt(encryptedSettings.ApiKey);
                }
                await _aiSettingsRepository.UpdateAsync(encryptedSettings);

                _aiSettingsCache = settingsContext.AISettings.Clone();

                OnContextChanged?.Invoke(_aiSettingsCache.ConvertToContext());
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
