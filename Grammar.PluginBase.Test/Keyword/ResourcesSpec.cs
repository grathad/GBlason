using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using FluentAssertions;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Token;
using Moq;
using Newtonsoft.Json;
using Utils.Assembly;
using Xunit;

namespace Grammar.PluginBase.Test.Keyword
{
    public class ResourcesSpec : IDisposable
    {
        public ResourcesSpec()
        {
            InitStaticHelpers();
        }
        public void Dispose()
        {
            InitStaticHelpers();
        }

        private void InitStaticHelpers()
        {
            typeof(AssemblyAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
        }

        public class Constructor
        {
            private Mock<Assembly> _assMock = new Mock<Assembly>();

            [Fact]
            public void FromNameAndAssembly_Initialization()
            {

                var nr = new PluginBase.Keyword.Resources("name", _assMock.Object);
                nr.ResourceName.Should().Be("name");
                nr.Assembly.Should().BeSameAs(_assMock.Object);
                nr.Root.Should().BeNull();
            }

            [Fact]
            public void FromAssemblyOnlyNoCulture_Throw()
            {
                var assemblyHelperMock = new Mock<IAssemblyAttribute>();
                assemblyHelperMock.Setup(m => m.GetCustomAttributes<NeutralResourcesLanguageAttribute>())
                    .Returns(new List<NeutralResourcesLanguageAttribute> {
                        default(NeutralResourcesLanguageAttribute)
                    });
                AssemblyAttributeExtensions.AssemblyAttributeFactory = a => assemblyHelperMock.Object;

                Action test = () => new Resources(_assMock.Object);
                test.Should().Throw<NullReferenceException>().Which.Message.Should()
                    .Be($"The assembly needs to contain a {nameof(NeutralResourcesLanguageAttribute)}");
            }

            [Fact]
            public void FromAssemblyOnly_ValidInitialization()
            {
                var assemblyHelperMock = new Mock<IAssemblyAttribute>();
                var cultureName = "culture";
                var cultureMock = new NeutralResourcesLanguageAttribute(cultureName);
                var assemblyName = new AssemblyName("name");
                assemblyHelperMock.Setup(m => m.GetCustomAttributes<NeutralResourcesLanguageAttribute>())
                    .Returns(new List<NeutralResourcesLanguageAttribute> { cultureMock })
                    .Verifiable();
                assemblyHelperMock.Setup(m => m.GetName())
                    .Returns(assemblyName)
                    .Verifiable();
                AssemblyAttributeExtensions.AssemblyAttributeFactory = a => assemblyHelperMock.Object;

                var result = new Resources(_assMock.Object);
                result.GrammarResourceName.Should()
                    .Be($"{assemblyName.Name}.{Resources.DefaultGrammarFileName}.{cultureName}");
                assemblyHelperMock.VerifyAll();
            }

            [Fact]
            public void FromEmpty_NoCulture_Throw()
            {
                var assemblyHelperMock = new Mock<IAssemblyAttribute>();
                assemblyHelperMock.Setup(m => m.GetCustomAttributes<NeutralResourcesLanguageAttribute>())
                    .Returns(new List<NeutralResourcesLanguageAttribute> {
                        default(NeutralResourcesLanguageAttribute)
                    });
                AssemblyAttributeExtensions.AssemblyAttributeFactory = a => assemblyHelperMock.Object;

                Action test = () => new Resources();
                test.Should().Throw<NullReferenceException>().Which.Message.Should()
                    .Be($"The assembly needs to contain a {nameof(NeutralResourcesLanguageAttribute)}");
            }

            [Fact]
            public void FromEmpty_ValidInitialization()
            {
                var assemblyHelperMock = new Mock<IAssemblyAttribute>();
                var cultureName = "culture";
                var cultureMock = new NeutralResourcesLanguageAttribute(cultureName);
                var assemblyName = new AssemblyName("name");
                assemblyHelperMock.Setup(m => m.GetCustomAttributes<NeutralResourcesLanguageAttribute>())
                    .Returns(new List<NeutralResourcesLanguageAttribute> { cultureMock })
                    .Verifiable();
                assemblyHelperMock.Setup(m => m.GetName())
                    .Returns(assemblyName)
                    .Verifiable();
                AssemblyAttributeExtensions.AssemblyAttributeFactory = a => assemblyHelperMock.Object;

                var result = new Resources();
                result.GrammarResourceName.Should()
                    .Be($"{assemblyName.Name}.{Resources.DefaultGrammarFileName}.{cultureName}");
                assemblyHelperMock.VerifyAll();
            }
        }

    }
}
