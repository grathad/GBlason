using System.IO;
using System.Reflection;
using RefAssembly = System.Reflection.Assembly;

namespace Utils.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAssemblyLoadContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        RefAssembly LoadFromAssemblyName(AssemblyName assemblyName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        RefAssembly LoadFromAssemblyPath(string assemblyPath);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nativeImagePath"></param>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        RefAssembly LoadFromNativeImagePath(string nativeImagePath, string assemblyPath);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        System.Reflection.Assembly LoadFromStream(Stream assembly);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="assemblySymbols"></param>
        /// <returns></returns>
        RefAssembly LoadFromStream(Stream assembly, Stream assemblySymbols);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="directoryPath"></param>
        void SetProfileOptimizationRoot(string directoryPath);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile"></param>
        void StartProfileOptimization(string profile);
    }
}
