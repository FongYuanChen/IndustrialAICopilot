using IndustrialAICopilot.Application.Interfaces;
using IndustrialAICopilot.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IndustrialAICopilot.Application.Utilities
{
    /// <summary>
    /// 應用層服務的擴充方法
    /// </summary>
    public static class ApplicationServiceExtensions
    {
        /// <summary>
        /// 將應用層服務註冊至 DI 容器。
        /// </summary>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IAISettingsManager, AISettingsManager>();
            services.AddSingleton<IKnowledgeBaseManager, KnowledgeBaseManager>();
            services.AddSingleton<IQuestionResponder, QuestionResponder>();
            services.AddHostedService<ApplicationServiceInitializerHostedService>();
            return services;
        }
    }
}
