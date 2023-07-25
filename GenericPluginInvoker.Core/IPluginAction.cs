namespace GenericPluginInvoker.Core
{
    public interface IPluginAction
    {
        public void Perform(IActionParameters parameters);
    }
}
