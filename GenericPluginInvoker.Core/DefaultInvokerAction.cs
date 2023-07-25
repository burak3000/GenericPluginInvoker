using Microsoft.Extensions.Options;

namespace GenericPluginInvoker.Core
{
    public class DefaultInvokerAction : IPluginAction
    {
        private readonly IOptions<AppConfig> _configuration;
        private readonly IReflectionHelper _reflectionHelper;

        public DefaultInvokerAction(IOptions<AppConfig> configuration, IReflectionHelper reflectionHelper)
        {
            _configuration = configuration;
            _reflectionHelper = reflectionHelper;
        }
        public void Perform(IActionParameters parameters)
        {
            //Read json file to create actions to perform
            ActionsConfiguration config =
                ActionsConfiguration.ReadFromJsonFile(_configuration.Value.ActionsConfigurationJsonFilePath);
            foreach (var configItem in config.Actions)
            {
                IReflectionHelper rh = new ReflectionHelper();
                IPluginAction pluginAction = rh.CreateInstanceOfTypeFromAssembly<IPluginAction>(configItem.ActionType, configItem.ActionAssembly, null);

                string paramsAsJsonStr = configItem.ActionParametersJson.ToString();
                Type paramsType =
                    rh.GetConcreteTypeFromAssembly(configItem.ActionParametersType, configItem.ActionParametersAssembly);
                IActionParameters actionParameters = JsonHelper.DeserializeFromString<IActionParameters>(paramsAsJsonStr, paramsType);

                pluginAction.Perform(actionParameters);
            }
        }
    }
}
