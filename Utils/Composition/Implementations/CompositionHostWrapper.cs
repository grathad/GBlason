using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.Composition.Hosting.Core;
using Utils.Composition.Contracts;

namespace Utils.Composition.Implementations
{
   /// <inheritdoc />
    public class CompositionHostWrapper : ICompositionHost
    {
        internal readonly CompositionHost Wrapped;

        /// <summary>
        /// wrapper construction to handle the default (and only implementation) wrapping around a <see cref="CompositionHost"/>
        /// </summary>
        /// <param name="toWrap">The original composition host to wrap</param>
        public CompositionHostWrapper(CompositionHost toWrap)
        {
            Wrapped = toWrap;
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
            Wrapped?.Dispose();
        }

        /// <inheritdoc />
        public bool TryGetExport(CompositionContract contract, out object export)
        {
            export = null;
            return Wrapped != null && Wrapped.TryGetExport(contract, out export);
        }

        /// <inheritdoc />
        public object GetExport(Type exportType)
        {
            return Wrapped?.GetExport(exportType);
        }

        /// <inheritdoc />
        public object GetExport(Type exportType, string contractName)
        {
            return Wrapped?.GetExport(exportType, contractName);
        }

        /// <inheritdoc />
        public object GetExport(CompositionContract contract)
        {
            return Wrapped?.GetExport(contract);
        }

        /// <inheritdoc />
        public TExport GetExport<TExport>()
        {
            return Wrapped == null ? default(TExport) : Wrapped.GetExport<TExport>();
        }

        /// <inheritdoc />
        public TExport GetExport<TExport>(string contractName)
        {
            return Wrapped == null ? default(TExport) : Wrapped.GetExport<TExport>(contractName);
        }

        /// <inheritdoc />
        public IEnumerable<object> GetExports(Type exportType)
        {
            return Wrapped?.GetExports(exportType);
        }

        /// <inheritdoc />
        public IEnumerable<object> GetExports(Type exportType, string contractName)
        {
            return Wrapped?.GetExports(exportType, contractName);
        }

        /// <inheritdoc />
        public IEnumerable<TExport> GetExports<TExport>()
        {
            return Wrapped?.GetExports<TExport>();
        }

        /// <inheritdoc />
        public IEnumerable<TExport> GetExports<TExport>(string contractName)
        {
            return Wrapped?.GetExports<TExport>(contractName);
        }
    }
}