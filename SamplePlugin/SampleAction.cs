using GenericPluginInvoker.Core;

namespace SamplePlugin
{
    public class SampleAction : IPluginAction
    {
        public void Perform(IActionParameters parameters)
        {
            SampleActionParameters? actionParams = parameters as SampleActionParameters;


        }
    }

    public class SampleActionParameters : IActionParameters
    {
        public string MessageToShow { get; set; }
    }
}