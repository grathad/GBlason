using System;
using System.Collections.Generic;
using System.Composition.Hosting.Core;

namespace Utils.Composition.Contracts
{
    /// <summary>
    /// Contract to inject the composition host to handle unit testing
    /// </summary>
    public interface ICompositionHost : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="export"></param>
        /// <returns></returns>
        bool TryGetExport(CompositionContract contract, out object export);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exportType"></param>
        /// <returns></returns>
        object GetExport(System.Type exportType);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exportType"></param>
        /// <param name="contractName"></param>
        /// <returns></returns>
        object GetExport(System.Type exportType, string contractName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        object GetExport(CompositionContract contract);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TExport"></typeparam>
        /// <returns></returns>
        TExport GetExport<TExport>();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TExport"></typeparam>
        /// <param name="contractName"></param>
        /// <returns></returns>
        TExport GetExport<TExport>(string contractName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exportType"></param>
        /// <returns></returns>
        IEnumerable<object> GetExports(System.Type exportType);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exportType"></param>
        /// <param name="contractName"></param>
        /// <returns></returns>
        IEnumerable<object> GetExports(System.Type exportType, string contractName);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TExport"></typeparam>
        /// <returns></returns>
        IEnumerable<TExport> GetExports<TExport>();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TExport"></typeparam>
        /// <param name="contractName"></param>
        /// <returns></returns>
        IEnumerable<TExport> GetExports<TExport>(string contractName);
    }
}