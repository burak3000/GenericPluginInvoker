namespace GenericPluginInvoker.Core;

public interface IReflectionHelper
{
    T CreateInstanceOfTypeFromAssembly<T>(string typeName, string assemblyPath, object[] ctorParams);
    Type GetConcreteTypeFromAssembly(string typeName, string assemblyPath);
    Type[] GetAllTypesFromAssembly(string assemblyPath);
}