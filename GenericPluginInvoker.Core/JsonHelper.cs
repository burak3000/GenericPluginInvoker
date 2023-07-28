using Newtonsoft.Json;

namespace GenericPluginInvoker.Core
{
    public static class JsonHelper
    {
        public static T DeserializeFromString<T>(string message)
        {
            var simpleDto = JsonConvert.DeserializeObject<T>(message,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
            return simpleDto;
        }

        public static T DeserializeFromString<T>(string message, Type type) where T : class
        {
            var simpleDto = JsonConvert.DeserializeObject(message, type);
            return simpleDto as T;
        }
    }
}
