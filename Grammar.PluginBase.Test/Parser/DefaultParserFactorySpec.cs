using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Grammar.PluginBase.Attributes;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;
using Moq;
using Utils.Assembly;
using Utils.Enum;
using Utils.TypeHelper;
using Xunit;

//required since we use static helpers for multiple tests. If the assignment is changed in parallel then one of the test
//will get the version of another one of the tests. The alternatives is to create a mock that works for all tests in an assembly
//or run the test sequentially
[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Grammar.PluginBase.Test.Parser
{
    public class DefaultParserFactorySpec
    {
        public class Constructor : IDisposable
        {
            private void InitializeResourceMock()
            {
                var assemblyHelperMock = new Mock<IAssemblyAttribute>();
                var cultureName = "culture";
                var cultureMock = new NeutralResourcesLanguageAttribute(cultureName);
                var assemblyName = new AssemblyName("name");
                assemblyHelperMock.Setup(m => m.GetCustomAttributes<NeutralResourcesLanguageAttribute>())
                    .Returns(new List<NeutralResourcesLanguageAttribute> { cultureMock })
                    .Verifiable();
                assemblyHelperMock.Setup(m => m.GetName())
                    .Returns(new AssemblyName("assemblyName"))
                    .Verifiable();
                AssemblyAttributeExtensions.AssemblyAttributeFactory = a => assemblyHelperMock.Object;
            }

            [Fact]
            public void Null_UseDefaultAssembly()
            {
                var test = new DefaultParserFactory(null);
                test.Assembly.FullName.Should().Be(Assembly.GetAssembly(typeof(DefaultParserFactory)).FullName);
                test.TypeCache.Should().NotBeNull();
            }

            [Fact]
            public void Initialization_Fail_Without_NeutralLanguage_Defined()
            {
                InitializeResourceMock();
                var input = new Mock<Assembly>().Object; 
                var test = new DefaultParserFactory(input);

                test.Assembly.Should().BeSameAs(input);
                test.TypeCache.Should().NotBeNull();
            }

            public void Dispose()
            {
                typeof(AssemblyAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
            }
        }

        public class GetParser : IDisposable
        {
            public GetParser()
            {
                typeof(EnumAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
                typeof(TypeAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
                _typeInfo = new Mock<TypeInfo>();
                _assembly = new Mock<Assembly>();
                _defaultMock = new Mock<DefaultParserFactory>(_assembly.Object, null) { CallBase = true };
            }

            private void InitializeResourceMock()
            {
                var assemblyHelperMock = new Mock<IAssemblyAttribute>();
                var cultureName = "culture";
                var cultureMock = new NeutralResourcesLanguageAttribute(cultureName);
                var assemblyName = new AssemblyName("name");
                assemblyHelperMock.Setup(m => m.GetCustomAttributes<NeutralResourcesLanguageAttribute>())
                    .Returns(new List<NeutralResourcesLanguageAttribute> { cultureMock })
                    .Verifiable();
                assemblyHelperMock.Setup(m => m.GetName())
                    .Returns(new AssemblyName("assemblyName"))
                    .Verifiable();
                AssemblyAttributeExtensions.AssemblyAttributeFactory = a => assemblyHelperMock.Object;
            }

            private readonly Mock<TypeInfo> _typeInfo;
            private readonly Mock<Assembly> _assembly;
            private readonly Mock<DefaultParserFactory> _defaultMock;

            [Fact]
            public void GenericTypeFound_GenericTypeReturned()
            {
                InitializeResourceMock();
                _defaultMock.SetupGet(m => m.TypeCache)
                    .Returns(new List<TypeInfo> { _typeInfo.Object });
                _typeInfo.SetupGet(t => t.Name).Returns("Not a match");

                var genericParserMock = new Mock<GenericParserAttribute>();
                genericParserMock.Setup(m => m.GetGenericParserType())
                    .Returns(_typeInfo.Object);

                var enAttrHelperMock = new Mock<EnumAttributeHelper>(TokenNames.Undefined);
                enAttrHelperMock.Setup(m => m.GetAttribute<GenericParserAttribute>())
                    .Returns(genericParserMock.Object);

                EnumAttributeExtensions.EnumAttributeFactory = @enum => enAttrHelperMock.Object;

                var result = _defaultMock.Object.GetParser(TokenNames.Undefined);
                result.Should().BeSameAs(_typeInfo.Object);
                genericParserMock.Verify();
                enAttrHelperMock.Verify();
            }

            [Fact]
            public void CustomTypeFound_CustomTypeReturned()
            {
                InitializeResourceMock();
                _defaultMock.SetupGet(m => m.TypeCache)
                    .Returns(new List<TypeInfo> { _typeInfo.Object });
                _typeInfo.SetupGet(t => t.Name).Returns("Undefined" + DefaultParserFactory.ParserName);

                var result = _defaultMock.Object.GetParser(TokenNames.Undefined);
                result.Should().BeSameAs(_typeInfo.Object);
            }

            [Fact]
            public void NoTypeFound_LeafTypeReturned()
            {
                InitializeResourceMock();
                _defaultMock.SetupGet(m => m.TypeCache)
                    .Returns(new List<TypeInfo> { _typeInfo.Object });
                _typeInfo.SetupGet(t => t.Name).Returns("Not a match");

                var genericParserMock = new Mock<GenericParserAttribute>();
                genericParserMock.Setup(m => m.GetGenericParserType())
                    .Returns(default(TypeInfo));

                var enAttrHelperMock = new Mock<EnumAttributeHelper>(TokenNames.Undefined);
                enAttrHelperMock.Setup(m => m.GetAttribute<GenericParserAttribute>())
                    .Returns(genericParserMock.Object);

                EnumAttributeExtensions.EnumAttributeFactory = @enum => enAttrHelperMock.Object;

                var result = _defaultMock.Object.GetParser(TokenNames.Undefined);
                result.Should().Be(typeof(LeafParser).GetTypeInfo());
                genericParserMock.Verify();
                enAttrHelperMock.Verify();
            }

            [Fact]
            public void TypeCacheNotAssigned_InitializeIt()
            {
                InitializeResourceMock();
                var typeAttrHelperMock = new Mock<TypeAttributeHelper>(typeof(object));
                typeAttrHelperMock.Setup(m => m.GetTypeInfo())
                    .Returns(_typeInfo.Object);

                TypeAttributeExtensions.TypeAttributeFactory = type => typeAttrHelperMock.Object;

                _defaultMock.SetupGet(m => m.TypeCache)
                    .Returns(new List<TypeInfo>());

                _typeInfo.SetupGet(m => m.Name).Returns("Undefined" + DefaultParserFactory.ParserName);
                var typeMock = new Mock<Type>();
                typeMock.SetupGet(m => m.Name).Returns("Undefined" + DefaultParserFactory.ParserName);

                _assembly.Setup(m => m.GetTypes()).Returns(new[] { typeMock.Object });
                _defaultMock.SetupGet(m => m.Assembly).Returns(_assembly.Object);

                var result = _defaultMock.Object.GetParser(TokenNames.Undefined);
                result.Should().BeSameAs(_typeInfo.Object);
                typeAttrHelperMock.Verify();
                _assembly.Verify();
            }

            public void Dispose()
            {
                typeof(EnumAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
                typeof(TypeAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
                typeof(AssemblyAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
            }
        }

        public class CreateParser : IDisposable
        {
            public CreateParser()
            {
                typeof(EnumAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
                typeof(TypeAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
                _parserPilot = new Mock<IParserPilot>();
                _typeInfo = new Mock<TypeInfo>();
                _defaultMock = new Mock<DefaultParserFactory>(null, null) { CallBase = true };
            }

            private Mock<IParserPilot> _parserPilot;
            private Mock<TypeInfo> _typeInfo;
            //private Mock<Assembly> _assembly;
            private Mock<DefaultParserFactory> _defaultMock;

            [Fact]
            public void TypeNotParserBase_Throw()
            {
                _defaultMock.Setup(m => m.GetParser(It.IsAny<TokenNames>()))
                    .Returns(typeof(object).GetTypeInfo);

                Action test = () => _defaultMock.Object.CreateParser(TokenNames.Undefined, null);
                test.Should().Throw<NotSupportedException>().Which.Message.Should().Be(
                    $"The requested type {typeof(object).Name} from the {TokenNames.Undefined} is not compatible with {nameof(ParserBase)}");
                _defaultMock.Verify();
            }

            [Fact]
            public void GenericFound_Return()
            {
                var genericContainer = new Mock<ContainerParser>(TokenNames.Undefined, _parserPilot.Object);

                var genericParserMock = new Mock<GenericParserAttribute>();
                genericParserMock.Setup(m => m.CreateGenericParserInstance(
                    It.IsAny<TokenNames>(),
                    It.IsAny<IParserPilot>()))
                    .Returns(genericContainer.Object);

                var enAttrHelperMock = new Mock<EnumAttributeHelper>(TokenNames.Undefined);
                enAttrHelperMock.Setup(m => m.GetAttribute<GenericParserAttribute>())
                    .Returns(genericParserMock.Object);

                EnumAttributeExtensions.EnumAttributeFactory = @enum => enAttrHelperMock.Object;

                _defaultMock.Setup(m => m.GetParser(It.IsAny<TokenNames>()))
                    .Returns(typeof(ContainerParser).GetTypeInfo);

                _defaultMock.Object.CreateParser(TokenNames.Undefined, _parserPilot.Object)
                    .Should().Be(genericContainer.Object);
                genericParserMock.Verify(m =>
                m.CreateGenericParserInstance(
                    TokenNames.Undefined,
                    _parserPilot.Object),
                    Times.Once);
                enAttrHelperMock.Verify();
            }

            internal class TestReflection : ContainerParser
            {
                public string TestResult = "";

                public TestReflection(TokenNames type, IParserPilot pilot) : base(type, pilot)
                {
                    TestResult = "Default";
                }

                public TestReflection(TokenNames type, IParserPilot pilot, IResources resources) : base(type, pilot)
                {
                    TestResult = "With resource";
                }

                public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
                {
                    throw new NotImplementedException();
                }

                /// <inheritdoc/>
                public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
                {
                    throw new System.NotImplementedException();
                }
            }

            internal class NoValidConstructor : ParserBase
            {
                public NoValidConstructor(object firstParam, string secondType)
                    : base(TokenNames.Undefined, default(IParserPilot))
                {

                }

                public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
                {
                    throw new NotImplementedException();
                }

                /// <inheritdoc/>
                public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
                {
                    throw new System.NotImplementedException();
                }
            }

            [Fact]
            public void CustomFound_ReturnLongestConstructorInstance()
            {
                var enAttrHelperMock = new Mock<EnumAttributeHelper>(TokenNames.Undefined);
                enAttrHelperMock.Setup(m => m.GetAttribute<GenericParserAttribute>())
                    .Returns(default(GenericParserAttribute));

                EnumAttributeExtensions.EnumAttributeFactory = @enum => enAttrHelperMock.Object;

                _defaultMock.Setup(m => m.GetParser(It.IsAny<TokenNames>()))
                    .Returns(typeof(TestReflection).GetTypeInfo);

                var result = _defaultMock.Object.CreateParser(TokenNames.Undefined, _parserPilot.Object);

                enAttrHelperMock.Verify();
                result.Should().NotBeNull().And.Subject.As<TestReflection>()
                    .TestResult.Should().Be("With resource");
            }

            [Fact]
            public void NoConstructorFound_Throw()
            {
                var enAttrHelperMock = new Mock<EnumAttributeHelper>(TokenNames.Undefined);
                enAttrHelperMock.Setup(m => m.GetAttribute<GenericParserAttribute>())
                    .Returns(default(GenericParserAttribute));

                EnumAttributeExtensions.EnumAttributeFactory = @enum => enAttrHelperMock.Object;

                _defaultMock.Setup(m => m.GetParser(It.IsAny<TokenNames>()))
                    .Returns(typeof(NoValidConstructor).GetTypeInfo);

                Action test = () => _defaultMock.Object.CreateParser(TokenNames.Undefined, _parserPilot.Object);

                var exception = test.Should().Throw<AggregateException>().Which;
                exception.InnerExceptions.Should().HaveCount(1);
                exception.InnerExceptions.First().Message.Should().Be(
                    "Impossible to use the constructor with 2 the parameter firstParam of type System.Object is incompatible with the data available"
                    );

                enAttrHelperMock.Verify();
                _defaultMock.Verify();

            }

            public void Dispose()
            {
                typeof(EnumAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
                typeof(TypeAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
            }
        }
    }
}
