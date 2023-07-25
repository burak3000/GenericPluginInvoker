using Microsoft.Extensions.Options;

namespace GenericPluginInvoker.Core
{
    public class DefaultInvokerAction : IPluginAction
    {
        private readonly IOptions<ActionsConfiguration> _configuration;

        public DefaultInvokerAction(IOptions<ActionsConfiguration> configuration)
        {
            _configuration = configuration;
        }
        public void Perform(IActionParameters parameters)
        {
            //throw new NotImplementedException();
        }
    }
}
