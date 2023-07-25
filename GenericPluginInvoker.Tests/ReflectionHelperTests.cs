using GenericPluginInvoker.Core;
using NUnit.Framework;
using System.Reflection;

namespace GenericPluginInvoker.Tests
{
    public class DummyActionForTesting : IPluginAction
    {
        public void Perform(IActionParameters parameters)
        {

        }
    }
    public class DummyParams : IActionParameters { public string MessageToShow { get; set; } }

    public class Tests
    {
        private ActionsConfiguration _actionsFromCurrentAssembly;

        [SetUp]
        public void SetUp()
        {

            string configFullPath =
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", "testConfig.json");
            if (!File.Exists(configFullPath))
            {
                throw new NotSupportedException(
                    "Operations cannot be continued without actionsConfiguration.json file in the executing directory.");
            }

            _actionsFromCurrentAssembly = ActionsConfiguration.ReadFromJsonFile(configFullPath);

            string targetActionType = typeof(DummyActionForTesting).FullName;
            string targetParametersType = typeof(DummyParams).FullName;
        }

        [Test]
        public void CreateInstance()
        {
            var configItem = _actionsFromCurrentAssembly.Actions[0];
            IReflectionHelper rh = new ReflectionHelper();
            IPluginAction pluginAction = rh.CreateInstanceOfTypeFromAssembly<IPluginAction>(configItem.ActionType, configItem.ActionAssembly, null);

            string paramsAsJsonStr = configItem.ActionParametersJson.ToString();
            Type paramsType =
                rh.GetConcreteTypeFromAssembly(configItem.ActionParametersType, configItem.ActionParametersAssembly);
            IActionParameters parameters = JsonHelper.DeserializeFromString<IActionParameters>(paramsAsJsonStr, paramsType);
            Assert.NotNull(pluginAction);
            Assert.NotNull(parameters);

            pluginAction.Perform(parameters);

        }



        #region GetAllTypesFromAssemblyTests
        [Test]
        public void GetAllTypesFromAssembly_EmptyAssembly_ReturnsTypesFromAppDomainAssemblies()
        {
            IReflectionHelper rh = new ReflectionHelper();
            var typesInCaller = rh.GetAllTypesFromAssembly("");
            typesInCaller = rh.GetAllTypesFromAssembly("");
            var aTypeFromThisAssembly = typesInCaller.Where(x => x.FullName.Equals(typeof(DummyActionForTesting).FullName)).FirstOrDefault();
            var anotherTypeFromThisAssembly = typesInCaller.Where(x => x.FullName.Equals(typeof(DummyParams).FullName)).FirstOrDefault();

            Assert.IsNotNull(aTypeFromThisAssembly);
            Assert.IsNotNull(anotherTypeFromThisAssembly);
        }

        [Test]
        public void GetAllTypesFromAssembly_NullAssembly_ThrowsArgumentNullException()
        {
            IReflectionHelper rh = new ReflectionHelper();
            Assert.Throws<ArgumentNullException>(() => rh.GetAllTypesFromAssembly(null));
        }

        [Test]
        public void GetAllTypesFromAssembly_CurrentAssembly_FindsAClass()
        {
            IReflectionHelper rh = new ReflectionHelper();
            var allTypesInThisAssembly = rh.GetAllTypesFromAssembly(Assembly.GetExecutingAssembly().Location);
            var aTypeFromThisAssembly = allTypesInThisAssembly.Where(x => x.FullName.Equals(typeof(DummyActionForTesting).FullName)).FirstOrDefault();

            Assert.IsNotNull(aTypeFromThisAssembly);
        }
        #endregion
    }
}