using IndustrialAICopilot.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IndustrialAICopilot.Infrastructure.Utilities
{
    /// <summary>
    /// 基礎建設層服務初始化背景服務
    /// </summary>
    public class InfrastructureServiceInitializerHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public InfrastructureServiceInitializerHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var aiSettingsRepository = scope.ServiceProvider.GetRequiredService<IAISettingsRepository>();
            await aiSettingsRepository.InitializeAsync();

            var knowledgeBaseRepository = scope.ServiceProvider.GetRequiredService<IKnowledgeBaseRepository>();
            await knowledgeBaseRepository.InitializeAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
