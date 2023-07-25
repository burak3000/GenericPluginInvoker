using Autofac.Extensions.DependencyInjection;
using GenericPluginInvoker.CompositionRoot;
using GenericPluginInvoker.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericPluginInvoker.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddBaseServices();
                    services.AddHostedService<PluginHostedService>();
                }).Build().Run();
        }
    }

    public class PluginHostedService : BackgroundService
    {
        private readonly IPluginAction _action;
        private readonly ILogger<PluginHostedService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="factory">I'm not passing ILogger<PluginHostedService> because it will be hard to test. For logger dependencies, I always pass ILoggerFactory/param>
        public PluginHostedService(IPluginAction action, ILoggerFactory factory)
        {
            _action = action;
            _logger = factory.CreateLogger<PluginHostedService>();

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Operations are started.");
            _action.Perform(null);
            _logger.LogWarning("Operations are finished.You can close this app via CTRL+C");
            return Task.CompletedTask;

        }
    }
}