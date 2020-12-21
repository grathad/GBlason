using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Grammar;
using Moq;
using Utils.Composition.Contracts;
using Utils.IO;
using Utils.Runtime;
using Utils.TypeHelper;
using Xunit;
using Eng = Engine.Engine;

namespace Engine.Test
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EngineSpec
    {
        public class Constructor
        {
            private Mock<global::Engine.Engine> _engineMock;

            [Fact]
            public void InitializeTrue_Initialize()
            {
                _engineMock = new Mock<Eng>(true, default(IContainerConfiguration), default(IAssemblyLoadContext)) { CallBase = true };
                _engineMock.Setup(e => e.LoadPlugin(It.IsAny<IEnumerable<Assembly>>())).Verifiable();
                _engineMock.Setup(e => e.CompatiblePlugins()).Returns(default(IEnumerable<Assembly>)).Verifiable();
                var useMock = _engineMock.Object.Parsers;
                _engineMock.VerifyAll();
            }

            [Fact]
            public void InitializeFalse_DoesNotLoadPlugins()
            {
                _engineMock = new Mock<Eng>(false, default(IContainerConfiguration), default(IAssemblyLoadContext)) { CallBase = true };
                _engineMock.Setup(e => e.LoadPlugin(It.IsAny<IEnumerable<Assembly>>())).Verifiable();
                _engineMock.Setup(e => e.CompatiblePlugins()).Returns(default(IEnumerable<Assembly>)).Verifiable();
                var useMock = _engineMock.Object.Parsers;
                _engineMock.Verify(m => m.LoadPlugin(It.IsAny<IEnumerable<Assembly>>()), Times.Never);
                _engineMock.Verify(m => m.CompatiblePlugins(), Times.Never);

            }
        }

        public class LoadPlugin
        {
            [Fact]
            public void NullInput_ThrowException()
            {
                var engine = new Eng(false);

                Action test = () => engine.LoadPlugin(null);
                test.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("assemblies");
            }

            [Fact]
            public void AssembliesInput_PresentInParsers()
            {
                var compositionHostMock = new Mock<ICompositionHost>();
                var containerConfig = new Mock<IContainerConfiguration>();
                containerConfig.Setup(c => c.WithAssemblies(It.IsAny<IEnumerable<Assembly>>()))
                    .Returns(containerConfig.Object)
                    .Verifiable();
                containerConfig.Setup(c => c.CreateContainer()).Returns(compositionHostMock.Object)
                    .Verifiable();
                compositionHostMock.Setup(c => c.GetExports<IGrammarParser>())
                    .Verifiable();
                var test = new Eng(false, containerConfig.Object);
                test.LoadPlugin(new List<Assembly>());
                compositionHostMock.VerifyAll();
                containerConfig.VerifyAll();
            }
        }

        public class CompatiblePlugins : IDisposable
        {
            [Fact]
            public void InvalidPath_ReturnNull()
            {
                Path.GetDirectoryName = s => "dirName";
                Path.Combine = (s, s1) => "combinedName";
                Directory.Exists = s => false;
                var engine = new Eng(false);
                engine.CompatiblePlugins().Should().BeNull();
            }

            [Fact]
            public void ValidPathNoDll_ReturnEmptyList()
            {
                Path.GetDirectoryName = s => "dirName";
                Path.Combine = (s, s1) => "combinedName";
                Directory.Exists = s => true;
                //return empty list
                Directory.GetFiles = (s, s1, arg3) => new string[0];
                var engine = new Eng(false);
                engine.CompatiblePlugins().Should().BeEmpty();
            }

            /// <summary>
            /// The folder contains a list of DLL
            /// None of the DLL in that list contains a type of <see cref="IGrammarParser"/>
            /// So the list of plugin ought to be empty
            /// </summary>
            [Fact]
            public void ValidPathDllPresentNoGrammarParser_ReturnEmptyList()
            {
                Path.GetDirectoryName = s => "dirName";
                Path.Combine = (s, s1) => "combinedName";
                Directory.Exists = s => true;
                //return one file
                Directory.GetFiles = (s, s1, arg3) => new[] { "test" };

                var tAttrExtMock = new Mock<TypeAttributeHelper>(typeof(object));
                tAttrExtMock.Setup(m => m.ContainsInterface<object>()).Returns(false);

                TypeAttributeExtensions.TypeAttributeFactory = type => tAttrExtMock.Object;

                var assemblyMock = new Mock<Assembly>();
                var contextLoaderMock = new Mock<IAssemblyLoadContext>();
                contextLoaderMock.Setup(l => l.LoadFromAssemblyPath(It.IsAny<string>())).Returns(assemblyMock.Object);
                //returning a type in the list that is not of the expected signature
                assemblyMock.SetupGet(m => m.ExportedTypes).Returns(new List<Type> { typeof(object) });

                var engine = new Eng(false, loadContext: contextLoaderMock.Object);

                engine.CompatiblePlugins().Should().BeEmpty();

                assemblyMock.VerifyGet(m => m.ExportedTypes);
                contextLoaderMock.Verify(m => m.LoadFromAssemblyPath("test"), Times.Once);
                tAttrExtMock.Verify(t => t.ContainsInterface<object>(), Times.Once);
            }

            [Fact]
            public void ValidPathDllPresentGrammarParserPresent_ReturnListWithAllAssemblies()
            {
                Path.GetDirectoryName = s => "dirName";
                Path.Combine = (s, s1) => "combinedName";
                Directory.Exists = s => true;
                //return one file
                Directory.GetFiles = (s, s1, arg3) => new[] { "test" };

                var tAttrExtMock = new Mock<TypeAttributeHelper>(typeof(object));
                tAttrExtMock.Setup(m => m.ContainsInterface<object>()).Returns(true);

                TypeAttributeExtensions.TypeAttributeFactory = type => tAttrExtMock.Object;

                var assemblyMock = new Mock<Assembly>();
                var contextLoaderMock = new Mock<IAssemblyLoadContext>();
                contextLoaderMock.Setup(l => l.LoadFromAssemblyPath(It.IsAny<string>())).Returns(assemblyMock.Object);
                //returning a type in the list that is not of the expected signature
                assemblyMock.SetupGet(m => m.ExportedTypes).Returns(new List<Type> { typeof(object) });

                var engine = new Eng(false, loadContext: contextLoaderMock.Object);

                var result = engine.CompatiblePlugins()?.ToList();
                result.Should().NotBeNull();
                result.Should().HaveCount(1);
                result.First().Should().BeSameAs(assemblyMock.Object);

                assemblyMock.VerifyGet(m => m.ExportedTypes);
                contextLoaderMock.Verify(m => m.LoadFromAssemblyPath("test"), Times.Once);
                tAttrExtMock.Verify(t => t.ContainsInterface<object>(), Times.Once);
            }

            public void Dispose()
            {
                typeof(Path).GetTypeInfo().TypeInitializer.Invoke(null, null);
                typeof(Directory).GetTypeInfo().TypeInitializer.Invoke(null, null);
            }
        }

        public class Parse
        {
            [Fact]
            public void NoBlazon_ThrowException()
            {
                var engine = new Eng(false);
                Action t = () => engine.Parse(null);
                t.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("blazon");
            }

            [Fact]
            public void NoInputPluginNoLoadedPlugins_ThrowException()
            {
                var engine = new Eng(false) { Parsers = null };
                Action test = () => engine.Parse("test");

                test.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("pluginToUse");
            }

            [Fact]
            public void InputPluginLoadedPlugins_UseInputPlugin()
            {
                var engine = new Eng(false) { Parsers = null };
                var pluginMock = new Mock<IGrammarParser>();
                pluginMock.Setup(m => m.Parse(It.IsAny<string>()));

                engine.Parse("test", pluginMock.Object);
                pluginMock.Verify(m => m.Parse("test"), Times.Once);

            }

            [Fact]
            public void NoInputPluginButLoadedPluginsAvailable_UseFirstLoadedPlugin()
            {
                var pluginMock = new Mock<IGrammarParser>();
                pluginMock.Setup(m => m.Parse(It.IsAny<string>()));

                var engine = new Eng(false) { Parsers = new List<IGrammarParser> { pluginMock.Object } };

                engine.Parse("test");
                pluginMock.Verify(m => m.Parse("test"), Times.Once);
            }
        }

    }
}
