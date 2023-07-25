using Newtonsoft.Json.Linq;

namespace GenericPluginInvoker.Core
{
    public class ActionsConfiguration
    {
        public const string ConfigSectionName = "ActionsConfiguration";
        public List<Action> Actions { get; set; }
        public static ActionsConfiguration ReadFromJsonFile(string jsonConfigFileFullPath)
        {
            var config = JsonHelper.DeserializeFromString<ActionsConfiguration>(File.ReadAllText(jsonConfigFileFullPath));
            return config;
        }
    }

    public class Action
    {
        public string ActionType { get; set; }
        public string ActionAssembly { get; set; }
        public string ActionParametersType { get; set; }
        public string ActionParametersAssembly { get; set; }
        public JObject ActionParametersJson { get; set; }
    }
}