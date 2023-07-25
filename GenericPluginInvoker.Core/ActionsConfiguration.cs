namespace GenericPluginInvoker.Core
{
    public class ActionsConfiguration
    {
        public const string ConfigSectionName = "ActionsConfiguration";
        public Action[] Actions { get; set; }
    }

    public class Action
    {
        public string ActionType { get; set; }
        public string ActionAssembly { get; set; }
        public string ActionParametersType { get; set; }
        public string ActionParametersAssembly { get; set; }
        public ActionParameters ActionParameters { get; set; }
    }

    public class ActionParameters
    {
        public string MessageToShow { get; set; }
    }
}