using System.Reflection;

namespace GenericPluginInvoker.Core
{
    public static class ReflectionHelper
    {
        public static T CreateInstanceOfTypeFromAssembly<T>(string typeName, string assemblyPath, object[] ctorParams)
        {
            Type targetType = GetConcreteTypeFromAssembly(typeName, assemblyPath);
            if (targetType == null)
            {
                throw new ArgumentException($"Given {typeName} could not be located inside the assembly {assemblyPath}.");
            }
            return (T)Activator.CreateInstance(targetType, ctorParams);
        }


        public static Type GetConcreteTypeFromAssembly(string typeName, string assemblyPath)
        {
            var types = GetAllTypesFromAssembly(assemblyPath);
            foreach (var type in types)
            {
                if (type.FullName.EndsWith(typeName))
                {
                    return type;
                }
            }
            return null;
        }

        public static Type[] GetAllTypesFromAssembly(string assemblyPath)
        {
            Assembly assemblyOfTheType = Assembly.LoadFile(assemblyPath);
            return assemblyOfTheType.GetTypes();
        }
    }
}
