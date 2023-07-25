using GenericPluginInvoker.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using System.Reflection;

namespace GenericPluginInvoker.CompositionRoot
{
    public static class ServicesExtensions
    {
        private static string configName = "actionsConfiguration.json";

        public static IServiceCollection AddBaseServices(this IServiceCollection services)
        {
            string configFullPath =
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), configName);
            if (!File.Exists(configFullPath))
            {
                throw new NotSupportedException(
                    "Operations cannot be continued without actionsConfiguration.json file in the executing directory.");
            }
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(configFullPath)
                .Build();

            var actionsConfiguration = Options.Create(config.GetSection(ActionsConfiguration.ConfigSectionName).Get<ActionsConfiguration>());
            services.AddSingleton(actionsConfiguration);

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(logger: logger);
            });
            services.AddSingleton<IPluginAction, DefaultInvokerAction>();

            return services;
        }
    }
}