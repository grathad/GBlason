using System;
using System.Collections.Generic;
using System.Composition.Convention;
using System.Composition.Hosting.Core;
using RefAssembly = System.Reflection.Assembly;

namespace Utils.Composition.Contracts
{
    /// <summary>
    /// Contract representing the container configuration as used as an injected logic for unit testing
    /// </summary>
    public interface IContainerConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ICompositionHost CreateContainer();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        IContainerConfiguration WithAssemblies(IEnumerable<RefAssembly> assemblies);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="conventions"></param>
        /// <returns></returns>
        IContainerConfiguration WithAssemblies(IEnumerable<RefAssembly> assemblies, AttributedModelProvider conventions);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IContainerConfiguration WithAssembly(RefAssembly assembly);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="conventions"></param>
        /// <returns></returns>
        IContainerConfiguration WithAssembly(RefAssembly assembly, AttributedModelProvider conventions);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conventions"></param>
        /// <returns></returns>
        IContainerConfiguration WithDefaultConventions(AttributedModelProvider conventions);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="partType"></param>
        /// <returns></returns>
        IContainerConfiguration WithPart(Type partType);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="partType"></param>
        /// <param name="conventions"></param>
        /// <returns></returns>
        IContainerConfiguration WithPart(Type partType, AttributedModelProvider conventions);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TPart"></typeparam>
        /// <returns></returns>
        IContainerConfiguration WithPart<TPart>();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TPart"></typeparam>
        /// <param name="conventions"></param>
        /// <returns></returns>
        IContainerConfiguration WithPart<TPart>(AttributedModelProvider conventions);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="partTypes"></param>
        /// <returns></returns>
        IContainerConfiguration WithParts(params Type[] partTypes);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="partTypes"></param>
        /// <returns></returns>
        IContainerConfiguration WithParts(IEnumerable<Type> partTypes);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="partTypes"></param>
        /// <param name="conventions"></param>
        /// <returns></returns>
        IContainerConfiguration WithParts(IEnumerable<Type> partTypes, AttributedModelProvider conventions);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exportDescriptorProvider"></param>
        /// <returns></returns>
        IContainerConfiguration WithProvider(ExportDescriptorProvider exportDescriptorProvider);
    }
}