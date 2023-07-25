namespace GenericPluginInvoker.Core
{
    public interface IPluginAction
    {
        void Perform(IActionParameters parameters);
    }
}
