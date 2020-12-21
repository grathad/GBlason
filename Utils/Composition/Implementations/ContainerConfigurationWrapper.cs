using System;
using System.Collections.Generic;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.Composition.Hosting.Core;
using System.Reflection;
using Utils.Composition.Contracts;
using RefAssembly = System.Reflection.Assembly;

namespace Utils.Composition.Implementations
{
    /// <inheritdoc />
    public class ContainerConfigurationWrapper : IContainerConfiguration
    {
        internal ContainerConfiguration Wrapped;

        /// <summary>
        /// 
        /// </summary>
        public ContainerConfigurationWrapper()
        {
            Wrapped = new ContainerConfiguration();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toWrap"></param>
        public ContainerConfigurationWrapper(ContainerConfiguration toWrap)
        {
            Wrapped = toWrap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wrap"></param>
        public static explicit operator ContainerConfiguration(ContainerConfigurationWrapper wrap)
        {
            return wrap?.Wrapped;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toWrap"></param>
        public static explicit operator ContainerConfigurationWrapper(ContainerConfiguration toWrap)
        {
            return new ContainerConfigurationWrapper(toWrap);
        }

        /// <inheritdoc />
        public ICompositionHost CreateContainer()
        {
            return new CompositionHostWrapper(Wrapped?.CreateContainer());
        }

        /// <inheritdoc />
        public virtual IContainerConfiguration WithAssemblies(IEnumerable<RefAssembly> assemblies)
        {
            Wrapped = Wrapped?.WithAssemblies(assemblies);
            return this;
        }

        /// <inheritdoc />
        public virtual IContainerConfiguration WithAssemblies(IEnumerable<RefAssembly> assemblies, AttributedModelProvider conventions)
        {
            Wrapped = Wrapped?.WithAssemblies(assemblies, conventions);
            return this;
        }

        /// <inheritdoc />
        public virtual IContainerConfiguration WithAssembly(RefAssembly assembly)
        {
            Wrapped = Wrapped?.WithAssembly(assembly);
            return this;
        }

        /// <inheritdoc />
        public virtual IContainerConfiguration WithAssembly(RefAssembly assembly, AttributedModelProvider conventions)
        {
            Wrapped = Wrapped?.WithAssembly(assembly, conventions);
            return this;
        }

        /// <inheritdoc />
        public virtual IContainerConfiguration WithDefaultConventions(AttributedModelProvider conventions)
        {
            Wrapped = Wrapped?.WithDefaultConventions(conventions);
            return this;
        }

        /// <inheritdoc />
        public virtual IContainerConfiguration WithPart(Type partType)
        {
            Wrapped = Wrapped?.WithPart(partType);
            return this;
        }

        /// <inheritdoc />
        public virtual IContainerConfiguration WithPart(Type partType, AttributedModelProvider conventions)
        {
            Wrapped = Wrapped?.WithPart(partType, conventions);
            return this;
        }

        /// <inheritdoc />
        public virtual IContainerConfiguration WithPart<TPart>()
        {
            Wrapped = Wrapped?.WithPart<TPart>();
            return this;
        }

        /// <inheritdoc />
        public virtual IContainerConfiguration WithPart<TPart>(AttributedModelProvider conventions)
        {
            Wrapped = Wrapped?.WithPart<TPart>(conventions);
            return this;
        }

        /// <inheritdoc />
        public virtual IContainerConfiguration WithParts(params Type[] partTypes)
        {
            Wrapped = Wrapped?.WithParts(partTypes);
            return this;
        }

        /// <inheritdoc />
        public virtual IContainerConfiguration WithParts(IEnumerable<Type> partTypes)
        {
            Wrapped = Wrapped?.WithParts(partTypes);
            return this;
        }

        /// <inheritdoc />
        public virtual IContainerConfiguration WithParts(IEnumerable<Type> partTypes, AttributedModelProvider conventions)
        {
            Wrapped = Wrapped?.WithParts(partTypes, conventions);
            return this;
        }

        /// <inheritdoc />
        public virtual IContainerConfiguration WithProvider(ExportDescriptorProvider exportDescriptorProvider)
        {
            Wrapped = Wrapped?.WithProvider(exportDescriptorProvider);
            return this;
        }
    }
}