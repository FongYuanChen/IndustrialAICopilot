using IndustrialAICopilot.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IndustrialAICopilot.Application.Utilities
{
    /// <summary>
    /// 應用層服務初始化背景服務
    /// </summary>
    public class ApplicationServiceInitializerHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public ApplicationServiceInitializerHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // 預載入
            using var scope = _serviceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<IQuestionResponder>();
            scope.ServiceProvider.GetRequiredService<IKnowledgeBaseManager>();
            scope.ServiceProvider.GetRequiredService<IAISettingsManager>();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
