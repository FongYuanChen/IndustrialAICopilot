using IndustrialAICopilot.Core.Interfaces;
using IndustrialAICopilot.Infrastructure.AISettingsRepositories;
using IndustrialAICopilot.Infrastructure.DataProtectors;
using IndustrialAICopilot.Infrastructure.DocumentRepositories;
using IndustrialAICopilot.Infrastructure.PromptChatters;
using IndustrialAICopilot.Infrastructure.PromptGenerators;
using IndustrialAICopilot.Infrastructure.Repositories;
using IndustrialAICopilot.Infrastructure.TextEmbedders;
using IndustrialAICopilot.Infrastructure.TextExtractors;
using IndustrialAICopilot.Infrastructure.TextSplitters;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace IndustrialAICopilot.Infrastructure.Utilities
{
    /// <summary>
    /// 基礎建設層服務的擴充方法
    /// </summary>
    public static class InfrastructureServiceExtensions
    {
        /// <summary>
        /// 將基礎建設層服務註冊至 DI 容器。
        /// </summary>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Keys")))
                    .SetApplicationName("IndustrialAICopilot");
            services.AddSingleton<IDataEncryptionProvider, DefaultDataEncryptionProvider>();
            services.AddSingleton<ITextExtractor, LangChainPdfTextExtractor>();
            services.AddSingleton<ITextExtractor, LangChainWordTextExtractor>();
            services.AddSingleton<ITextExtractor, TxtTextExtractor>();
            services.AddSingleton<ITextSplitter, LangChainRecursiveTextSplitter>();
            services.AddSingleton<ITextEmbedder, GeminiTextEmbedder>();
            services.AddSingleton<ITextEmbedder, OpenAITextEmbedder>();
            services.AddSingleton<ITextSearcher, DefaultTextSearcher>();
            services.AddSingleton<IPromptGenerator, DefaultPromptGenerator>();
            services.AddSingleton<ITextGenerator, GeminiTextGenerator>();
            services.AddSingleton<ITextGenerator, OpenAITextGenerator>();
            services.AddSingleton<IKnowledgeBaseRepository, SqliteKnowledgeBaseRepository>();
            services.AddSingleton<IAISettingsRepository, SqliteAISettingsRepository>();
            services.AddHostedService<InfrastructureServiceInitializerHostedService>();
            return services;
        }
    }
}
