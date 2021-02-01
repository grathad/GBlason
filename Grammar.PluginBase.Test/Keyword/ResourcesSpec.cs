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
                result.ResourceName.Should()
                    .Be($"{assemblyName.Name}.{Resources.DefaultResourceFileName}.{cultureName}");
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
                result.ResourceName.Should()
                    .Be($"{assemblyName.Name}.{Resources.DefaultResourceFileName}.{cultureName}");
                assemblyHelperMock.VerifyAll();
            }
        }

        public class LoadResourceInMemory
        {
            [Fact]
            public void NoResourceFound_Throw()
            {
                var ass = new Mock<Assembly>();
                ass.Setup(m => m.GetManifestResourceStream(It.IsAny<string>()))
                    .Returns(default(Stream));
                var nr = new Resources("name", ass.Object);
                Action test = () => nr.LoadResourceInMemory();
                test.Should().Throw<MissingManifestResourceException>()
                    .Which.Message.Should().Be("name");
                ass.Verify();
            }

            [Fact]
            public void ResourceFound_RootAssigned()
            {
                var resource = new PluginBase.Keyword.Format
                {
                    Keywords = new Dictionary<string, IEnumerable<string>>(),
                    MergeableWords = new List<string> { "one", "two" },
                    Tokens = new Dictionary<string, IEnumerable<IEnumerable<string>>>()
                };

                var resourceString = JsonConvert.SerializeObject(resource);
                var resourceStream = new MemoryStream(Encoding.UTF8.GetBytes(resourceString));

                var ass = new Mock<Assembly>();
                ass.Setup(m => m.GetManifestResourceStream(It.IsAny<string>()))
                    .Returns(resourceStream);
                var nr = new Resources("name", ass.Object);
                nr.LoadResourceInMemory();
                nr.Root.Should().BeOfType<PluginBase.Keyword.Format>()
                    .Which.MergeableWords.Should().HaveCount(2)
                    .And.Subject.Should().ContainInOrder("one", "two");
                ass.Verify();
            }
        }

        public class GetKeyWords
        {
            [Fact]
            public void RootNull_InitializeThenReturn()
            {
                var mR = new Mock<Resources>("any", new Mock<Assembly>().Object) { CallBase = true };
                mR.Setup(m => m.LoadResourceInMemory());
                mR.SetupSequence(m => m.Root)
                    .Returns(default(PluginBase.Keyword.Format))
                    .Returns(new PluginBase.Keyword.Format
                    {
                        Keywords = new Dictionary<string, IEnumerable<string>>()
                    });
                mR.Object.GetKeywords().Should().NotBeNull();
                mR.Verify();
            }

            [Fact]
            public void RootInitialized_Return()
            {
                var mR = new Mock<Resources>("any", new Mock<Assembly>().Object) { CallBase = true };
                mR.Setup(m => m.LoadResourceInMemory());
                mR.SetupGet(m => m.Root)
                    .Returns(new PluginBase.Keyword.Format
                    {
                        Keywords = new Dictionary<string, IEnumerable<string>>()
                    });
                mR.Object.GetKeywords().Should().NotBeNull();
                mR.Verify(m => m.LoadResourceInMemory(), Times.Never);
            }
        }

        public class GetTokens
        {
            [Fact]
            public void RootNull_InitializeThenReturn()
            {
                var mR = new Mock<Resources>("any", new Mock<Assembly>().Object) { CallBase = true };
                mR.Setup(m => m.LoadResourceInMemory());
                mR.SetupSequence(m => m.Root)
                    .Returns(default(PluginBase.Keyword.Format))
                    .Returns(new PluginBase.Keyword.Format
                    {
                        Tokens = new Dictionary<string, IEnumerable<IEnumerable<string>>>()
                    });
                mR.Object.GetTokens().Should().NotBeNull();
                mR.Verify();
            }

            [Fact]
            public void RootInitialized_Return()
            {
                var mR = new Mock<Resources>("any", new Mock<Assembly>().Object) { CallBase = true };
                mR.Setup(m => m.LoadResourceInMemory());
                mR.SetupGet(m => m.Root)
                    .Returns(new PluginBase.Keyword.Format
                    {
                        Tokens = new Dictionary<string, IEnumerable<IEnumerable<string>>>()
                    });
                mR.Object.GetTokens().Should().NotBeNull();
                mR.Verify(m => m.LoadResourceInMemory(), Times.Never);
            }

            [Fact]
            public void NameNotPresent_ReturnNull()
            {
                var mR = new Mock<Resources>("any", new Mock<Assembly>().Object) { CallBase = true };
                mR.Setup(m => m.GetTokens()).Returns(new Dictionary<string, IEnumerable<IEnumerable<string>>>());
                mR.Object.GetTokens(TokenNames.Undefined).Should().BeNull();
                mR.Verify();
            }

            [Fact]
            public void NamePresent_ReturnName()
            {
                var mR = new Mock<Resources>("any", new Mock<Assembly>().Object) { CallBase = true };
                var tokens = new Dictionary<string, IEnumerable<IEnumerable<string>>>
                {
                    {"Undefined", new[] {new[] {"one"}}}
                };
                mR.Setup(m => m.GetTokens())
                    .Returns(tokens);
                mR.Object.GetTokens(TokenNames.Undefined).Should().NotBeNull()
                    .And.Subject.Should().HaveCount(1);
                mR.Verify();
            }
        }

        public class GetMergeableWords
        {
            [Fact]
            public void RootNull_InitializeThenReturn()
            {
                var mR = new Mock<Resources>("any", new Mock<Assembly>().Object) { CallBase = true };
                mR.Setup(m => m.LoadResourceInMemory());
                mR.SetupSequence(m => m.Root)
                    .Returns(default(PluginBase.Keyword.Format))
                    .Returns(new PluginBase.Keyword.Format
                    {
                        MergeableWords = new List<string>()
                    });
                mR.Object.GetMergeableWords().Should().NotBeNull();
                mR.Verify();
            }

            [Fact]
            public void RootInitialized_Return()
            {
                var mR = new Mock<Resources>("any", new Mock<Assembly>().Object) { CallBase = true };
                mR.Setup(m => m.LoadResourceInMemory());
                mR.SetupGet(m => m.Root)
                    .Returns(new PluginBase.Keyword.Format
                    {
                        MergeableWords = new List<string>()
                    });
                mR.Object.GetMergeableWords().Should().NotBeNull();
                mR.Verify(m => m.LoadResourceInMemory(), Times.Never);
            }
        }

    }
}
