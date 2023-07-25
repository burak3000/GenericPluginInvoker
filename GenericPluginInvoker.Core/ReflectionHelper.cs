using System.Collections.Concurrent;
using System.Reflection;

namespace GenericPluginInvoker.Core
{
    public class ReflectionHelper : IReflectionHelper
    {
        private ConcurrentDictionary<string, Type> _typeNameToTypeDictionary = new ConcurrentDictionary<string, Type>();
        private ConcurrentDictionary<string, HashSet<Type>> _assemblyNameToTypeDictionary = new ConcurrentDictionary<string, HashSet<Type>>();

        public ReflectionHelper()
        {
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in allAssemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    _typeNameToTypeDictionary.TryAdd(type.FullName, type);
                }
            }

            AppDomain.CurrentDomain.AssemblyLoad += (sender, args) =>
            {
                Task.Run(() =>
                {
                    HashSet<Type> types = new HashSet<Type>();
                    foreach (var type in args.LoadedAssembly.GetTypes())
                    {
                        types.Add(type);
                        _typeNameToTypeDictionary.TryAdd(type.FullName, type);
                        _typeNameToTypeDictionary.AddOrUpdate(type.FullName, type, (existingKey, existingValue) =>
                        {
                            existingValue = type;
                            return existingValue;
                        });
                    }
                    _assemblyNameToTypeDictionary.AddOrUpdate(args.LoadedAssembly.Location, types,
                        (existingKey, existingValue) =>
                        {
                            existingValue.UnionWith(types);
                            return existingValue;
                        });
                });

            };
        }
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
            //check the cache first
            Type typeToReturn = null;
            if (_assemblyNameToTypeDictionary.ContainsKey(assemblyPath) && _typeNameToTypeDictionary.ContainsKey(typeName))
            {
                if (_assemblyNameToTypeDictionary[assemblyPath]
                    .TryGetValue(_typeNameToTypeDictionary[typeName], out typeToReturn))
                {
                    return typeToReturn;
                }
            }
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
            HashSet<Type> types = new HashSet<Type>();
            if (assemblyPath is null)
            {
                throw new ArgumentNullException(nameof(assemblyPath));
            }

            if (string.IsNullOrWhiteSpace(assemblyPath))
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        types.Add(type);
                        _typeNameToTypeDictionary.AddOrUpdate(type.FullName, type, (existingKey, existingValue) =>
                        {
                            existingValue = type;
                            return existingValue;
                        });
                    }
                }
            }
            else
            {
                Assembly assemblyOfTheType = Assembly.LoadFrom(assemblyPath);
                foreach (var type in assemblyOfTheType.GetTypes())
                {
                    types.Add(type);
                    _typeNameToTypeDictionary.AddOrUpdate(type.FullName, type, (existingKey, existingValue) =>
                    {
                        existingValue = type;
                        return existingValue;
                    });
                }

            }

            if (!_assemblyNameToTypeDictionary.ContainsKey(assemblyPath))
            {
                _assemblyNameToTypeDictionary.TryAdd(assemblyPath, types);
            }

            return types.ToArray();

        }
    }
}
