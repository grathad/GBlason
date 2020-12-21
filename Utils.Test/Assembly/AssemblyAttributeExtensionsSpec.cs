using System;
using System.Reflection;
using FluentAssertions;
using Moq;
using Utils.Assembly;
using Xunit;

namespace Utils.Test.Assembly
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AssemblyAttributeExtensionsSpec
    {
        public AssemblyAttributeExtensionsSpec()
        {
            typeof(AssemblyAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
        }

        public class Assembly : IDisposable
        {
            [Fact]
            public void Default()
            {
                typeof(AssemblyAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
                AssemblyAttributeExtensions.AssemblyAttributeFactory.Should().NotBeNull();
            }

            [Fact]
            public void CallFactory()
            {
                var result = System.Reflection.Assembly.GetAssembly(typeof(object));

                AssemblyAttributeExtensions.AssemblyAttributeFactory = a =>
                {
                    result = a;
                    return new Mock<IAssemblyAttribute>().Object;
                };
                AssemblyAttributeExtensions.Assembly(null);
                result.Should().BeNull();
                System.Reflection.Assembly.GetAssembly(typeof(AssemblyAttributeExtensionsSpec)).Assembly();
                result.Should().BeSameAs(System.Reflection.Assembly.GetAssembly(typeof(AssemblyAttributeExtensionsSpec)));
            }

            public void Dispose()
            {
                typeof(AssemblyAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
            }
        }
    }
}
