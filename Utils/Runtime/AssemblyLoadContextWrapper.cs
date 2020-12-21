using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using RefAssembly = System.Reflection.Assembly;

namespace Utils.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public class AssemblyLoadContextWrapper : IAssemblyLoadContext
    {
        internal readonly AssemblyLoadContext Wrapped;

        /// <summary>
        /// 
        /// </summary>
        public AssemblyLoadContextWrapper(AssemblyLoadContext init = null)
        {
            Wrapped = init ?? AssemblyLoadContext.Default;
        }

        /// <inheritdoc />
        public virtual RefAssembly LoadFromAssemblyName(AssemblyName assemblyName)
        {
            return assemblyName == null ? null : Wrapped?.LoadFromAssemblyName(assemblyName);
        }

        /// <inheritdoc />
        public virtual RefAssembly LoadFromAssemblyPath(string assemblyPath)
        {
            return assemblyPath == null ? null : Wrapped?.LoadFromAssemblyPath(assemblyPath);
        }

        /// <inheritdoc />
        public virtual RefAssembly LoadFromNativeImagePath(string nativeImagePath, string assemblyPath)
        {
            return nativeImagePath == null || string.IsNullOrEmpty(assemblyPath)
                ? null 
                : Wrapped?.LoadFromNativeImagePath(nativeImagePath, assemblyPath);
        }

        /// <inheritdoc />
        public virtual RefAssembly LoadFromStream(Stream assembly)
        {
            return assembly == null ? null : Wrapped?.LoadFromStream(assembly);
        }

        /// <inheritdoc />
        public virtual RefAssembly LoadFromStream(Stream assembly, Stream assemblySymbols)
        {
            return assembly == null ? null : Wrapped?.LoadFromStream(assembly, assemblySymbols);
        }

        /// <inheritdoc />
        public virtual void SetProfileOptimizationRoot(string directoryPath)
        {
            Wrapped?.SetProfileOptimizationRoot(directoryPath);
        }

        /// <inheritdoc />
        public virtual void StartProfileOptimization(string profile)
        {
            Wrapped?.StartProfileOptimization(profile);
        }
    }

    
}
