using System.Reflection;

namespace GenericPluginInvoker.Core
{
    public class ReflectionHelper : IReflectionHelper
    {

        public T CreateInstanceOfTypeFromAssembly<T>(string typeName, string assemblyPath, object[] ctorParams)
        {
            Type targetType = GetConcreteTypeFromAssembly(typeName, assemblyPath);
            if (targetType == null)
            {
                throw new ArgumentException($"Given {typeName} could not be located inside the assembly {assemblyPath}.");
            }
            return (T)Activator.CreateInstance(targetType, ctorParams);
        }


        public Type GetConcreteTypeFromAssembly(string typeName, string assemblyPath)
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
        /// <summary>
        ///  
        /// </summary>
        /// <param name="assemblyPath">Returns all the app domain types if this is empty</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Type[] GetAllTypesFromAssembly(string assemblyPath)
        {
            assemblyPath = Path.GetFullPath(assemblyPath);
            HashSet<Type> types = new HashSet<Type>();
            Assembly assemblyOfTheType = Assembly.LoadFrom(assemblyPath);
            foreach (var type in assemblyOfTheType.GetTypes())
            {
                types.Add(type);
            }

            return types.ToArray();

        }
    }
}
