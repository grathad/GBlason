using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Common;
using Grammar.PluginBase.Attributes;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;
using Moq;
using Moq.Protected;
using Utils.Enum;
using Xunit;

namespace Grammar.PluginBase.Test.Parser
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ParserPilotSpecs
    {
        public class Constructor
        {
            [Fact]
            public void NullParameters()
            {
                //need to check the first all null input
                var parser = new ParserPilot(null, null);

                parser.Should().NotBeNull().And.Subject
                    .As<ParserPilot>().Errors.Should().NotBeNull().And.Subject.Should().BeEmpty();

                parser.LastPosition.Should().Be(-1);

                Action test = () => parser.GetKeyword(0);
                test.Should().Throw<IndexOutOfRangeException>();
            }

            [Fact]
            public void ValidInput()
            {
                //need to check with valid inputs
                var parser = new ParserPilot(
                    new DefaultParserFactory(typeof(ParserPilot).GetTypeInfo().Assembly),
                    new[] { new PluginBase.Keyword.ParsedKeyword() }, new[] { new ParserError(0, "explanation") });

                parser.Should().NotBeNull().And.Subject
                    .As<ParserPilot>().Errors.Should().NotBeNull().And.Subject
                    .As<IList<ParserError>>().Should().HaveCount(1);

                parser.LastPosition.Should().Be(0);

                parser.GetKeyword(0).Should().NotBeNull().And.Subject.Should().BeOfType<ParsedKeyword>();
            }
        }

        public class Parse
        {
            private readonly IList<ParsedKeyword> _keywords = new List<ParsedKeyword>();
            private readonly IList<ParserError> _errors = new List<ParserError>();
            private readonly Mock<ParserPilot> _pilotMock;
            private readonly Mock<ITokenResult> _tokenResult;
            private readonly Mock<ParserNode> _node;
            private readonly Mock<ParserBase> _parent;
            private readonly Mock<IParserTree> _tree;

            public Parse()
            {
                _tokenResult = new Mock<ITokenResult>();
                var pilotFactory = new Mock<IParserFactory>();
                _pilotMock = new Mock<ParserPilot>(pilotFactory.Object, _keywords, _errors) { CallBase = true };
                _parent = new Mock<ParserBase>(
                    TokenNames.Undefined,
                    _pilotMock.Object);

                _node = new Mock<ParserNode>(
                    _parent.Object,
                    new Mock<TokenParsingPosition>().Object);
                _tree = new Mock<IParserTree>();
                _pilotMock.SetupGet(m => m.CallTree).Returns(_tree.Object);
            }

            [Fact]
            public void InputInvalid_ReturnNull()
            {
                var position = new TokenParsingPosition { Start = 5 };
                TokenParsingPosition receivedParameter = null;
                _pilotMock.Setup(m => m.IsInputValid(
                    ref It.Ref<ITokenParsingPosition>.IsAny,
                    It.IsAny<TokenNames>()))
                    .Returns<TokenParsingPosition, TokenNames>((p, t) =>
                    {
                        receivedParameter = p;
                        return false;
                    });

                _pilotMock.Object.Parse(position, token: TokenNames.Undefined).Should().BeNull();
                _pilotMock.Verify(
                    m => m.IsInputValid(
                    ref It.Ref<ITokenParsingPosition>.IsAny,
                    TokenNames.Undefined),
                    Times.Once());
                receivedParameter.Should().Be(position);
            }

            [Fact]
            public void ParserAlreadyExecuted_ReturnFromMemory()
            {
                _pilotMock.Setup(m => m.IsInputValid(
                    ref It.Ref<ITokenParsingPosition>.IsAny,
                    It.IsAny<TokenNames>()))
                    .Returns(true)
                    .Verifiable();

                _pilotMock.Protected().Setup<ParserNode>(
                    "GetFromMemory",
                    ItExpr.IsAny<TokenNames>(),
                    ItExpr.IsAny<int>())
                    .Returns(_node.Object)
                    .Verifiable();

                var result = _tokenResult.Object;
                _pilotMock.Setup(m => m.ParserAlreadyExistAndExecutedNoReRun(
                    It.IsAny<ParserBase>(),
                    It.IsAny<ParserNode>(),
                    out result))
                    .Returns(true)
                    .Verifiable();

                _pilotMock.Object.Parse(
                    TokenParsingPosition.DefaultStartingPosition,
                    _parent.Object,
                    TokenNames.Undefined)
                    .Should()
                    .Be(_tokenResult.Object);

                _pilotMock.Verify(m => m.IsInputValid(
                        ref It.Ref<ITokenParsingPosition>.IsAny,
                        It.IsAny<TokenNames>()),
                    Times.Once);
                _pilotMock.Verify(m => m.ParserAlreadyExistAndExecutedNoReRun(
                        It.IsAny<ParserBase>(),
                        It.IsAny<ParserNode>(),
                        out result),
                    Times.Once);
                _pilotMock.Verify(m => m.NewParserInstance(It.IsAny<TokenNames>()),
                    Times.Never);
                _pilotMock.Protected().Verify(
                    "GetFromMemory",
                    Times.Once(),
                    ItExpr.IsAny<TokenNames>(),
                    ItExpr.IsAny<int>());
            }

            [Fact]
            public void NewParserPreExecutionFalse_ReturnNull()
            {
                _pilotMock.Setup(m => m.IsInputValid(
                        ref It.Ref<ITokenParsingPosition>.IsAny,
                        It.IsAny<TokenNames>()))
                    .Returns(true)
                    .Verifiable();

                var tResult = _tokenResult.Object;
                _pilotMock.Setup(m => m.ParserAlreadyExistAndExecutedNoReRun(
                        It.IsAny<ParserBase>(),
                        It.IsAny<ParserNode>(),
                        out tResult))
                    .Returns(false)
                    .Verifiable();

                _pilotMock.Setup(m => m.NewParserInstance(It.IsAny<TokenNames>()))
                    .Returns(_parent.Object)
                    .Verifiable();

                _pilotMock.Protected().Setup<bool>(
                        "RunPreExecution",
                        ItExpr.IsAny<ParserBase>(),
                        ItExpr.IsAny<ParserBase>(),
                        ItExpr.IsAny<int>())
                    .Returns(false);

                _pilotMock.Object.Parse(
                        TokenParsingPosition.DefaultStartingPosition,
                        _parent.Object,
                        TokenNames.Undefined)
                    .Should()
                    .BeNull();

                _pilotMock.Verify(m => m.IsInputValid(
                    ref It.Ref<ITokenParsingPosition>.IsAny,
                    It.IsAny<TokenNames>()),
                    Times.Once);
                _pilotMock.Verify(m => m.ParserAlreadyExistAndExecutedNoReRun(
                    It.IsAny<ParserBase>(),
                    It.IsAny<ParserNode>(),
                    out tResult),
                    Times.Once);
                _pilotMock.Verify(m => m.NewParserInstance(It.IsAny<TokenNames>()),
                    Times.Once);
                _pilotMock.Protected().Verify(
                    "RunPreExecution",
                    Times.Once(),
                    ItExpr.IsAny<ParserBase>(),
                    ItExpr.IsAny<ParserBase>(),
                    ItExpr.IsAny<int>());
            }

            [Fact]
            public void CreateNewParserAndExecuteIt_ReturnResult()
            {
                _pilotMock.Setup(m => m.IsInputValid(
                        ref It.Ref<ITokenParsingPosition>.IsAny,
                        It.IsAny<TokenNames>()))
                    .Returns(true)
                    .Verifiable();

                var tResult = _tokenResult.Object;
                _pilotMock.Setup(m => m.ParserAlreadyExistAndExecutedNoReRun(
                        It.IsAny<ParserBase>(),
                        It.IsAny<ParserNode>(),
                        out tResult))
                    .Returns(false)
                    .Verifiable();

                _pilotMock.Setup(m => m.NewParserInstance(It.IsAny<TokenNames>()))
                    .Returns(_parent.Object)
                    .Verifiable();

                _pilotMock.Protected().Setup<bool>(
                        "RunPreExecution",
                        ItExpr.IsAny<ParserBase>(),
                        ItExpr.IsAny<ParserBase>(),
                        ItExpr.IsAny<int>())
                    .Returns(true);

                ParserNode newNode = null;

                _tree.Setup(m => m.AddChild(It.IsAny<ParserBase>(), It.IsAny<ParserNode>()))
                    .Callback<ParserBase, ParserNode>((b, n) =>
                    {
                        newNode = n;
                    })
                    .Verifiable();

                _parent.Setup(m => m.TryConsume(ref It.Ref<ITokenParsingPosition>.IsAny))
                    .Returns(_tokenResult.Object);

                _pilotMock.Object.Parse(
                        TokenParsingPosition.DefaultStartingPosition,
                        _parent.Object,
                        TokenNames.Undefined)
                    .Should()
                    .Be(_tokenResult.Object);

                _pilotMock.VerifyAll();
                _tree.Verify();
                _pilotMock.Protected().Verify(
                    "RunPreExecution",
                    Times.Once(),
                    ItExpr.IsAny<ParserBase>(),
                    ItExpr.IsAny<ParserBase>(),
                    ItExpr.IsAny<int>());
                newNode.Status.Should().Be(NodeParserStatus.Executed);
                newNode.Result.Should().Be(_tokenResult.Object);
            }
        }

        public class NewParserInstance
        {
            private readonly Mock<ParserPilot> _pilotMock;
            private readonly IList<ParsedKeyword> _keywords = new List<ParsedKeyword>();
            private readonly IList<ParserError> _errors = new List<ParserError>();
            private readonly Mock<IParserFactory> _factory;
            private readonly Mock<ParserBase> _parserBase;
            public NewParserInstance()
            {
                _factory = new Mock<IParserFactory>();
                _pilotMock = new Mock<ParserPilot>(_factory.Object, _keywords, _errors) { CallBase = true };
                _parserBase = new Mock<ParserBase>(TokenNames.Undefined, _pilotMock.Object);
            }

            [Fact]
            public void FactoryThrow_Throw()
            {
                _factory.Setup(m => m.CreateParser(
                    It.IsAny<TokenNames>(),
                    It.IsAny<IParserPilot>(),
                    It.IsAny<IResources>()))
                    .Throws<Exception>()
                    .Verifiable();
                Action test = () => _pilotMock.Object.NewParserInstance(TokenNames.Undefined);
                test.Should().Throw<NotSupportedException>().Which.Message.Should().Be(
                    "We could not create a parser for the Undefined because: Exception of type 'System.Exception' was thrown."
                );
                _factory.Verify(m => m.CreateParser(
                    TokenNames.Undefined,
                    _pilotMock.Object,
                    default(IResources)),
                    Times.Once());
            }

            [Fact]
            public void InstanceNull_Throw()
            {
                _factory.Setup(m => m.CreateParser(
                        It.IsAny<TokenNames>(),
                        It.IsAny<IParserPilot>(),
                        It.IsAny<IResources>()))
                    .Returns(default(ParserBase))
                    .Verifiable();
                Action test = () => _pilotMock.Object.NewParserInstance(TokenNames.Undefined);
                test.Should().Throw<NotSupportedException>().Which.Message.Should().Be(
                    "The parser created for Undefined is null"
                );
                _factory.Verify(m => m.CreateParser(
                        TokenNames.Undefined,
                        _pilotMock.Object,
                        default(IResources)),
                    Times.Once());
            }

            [Fact]
            public void InstanceCreated_ReturnInstance()
            {
                _factory.Setup(m => m.CreateParser(
                        It.IsAny<TokenNames>(),
                        It.IsAny<IParserPilot>(),
                        It.IsAny<IResources>()))
                    .Returns(_parserBase.Object)
                    .Verifiable();
                _pilotMock.Object.NewParserInstance(TokenNames.Undefined)
                    .Should().Be(_parserBase.Object);
                _factory.Verify(m => m.CreateParser(
                        TokenNames.Undefined,
                        _pilotMock.Object,
                        default(IResources)),
                    Times.Once());
            }
        }

        public class ParserAlreadyExistAndExecutedNoReRun
        {
            private readonly IList<ParsedKeyword> _keywords = new List<ParsedKeyword>();
            private readonly IList<ParserError> _errors = new List<ParserError>();
            private readonly Mock<ParserPilot> _pilotMock;
            private readonly Mock<ITokenResult> _tokenResult;
            private readonly Mock<ParserNode> _node;
            private readonly Mock<ParserBase> _parent;
            private readonly Mock<IParserTree> _tree;

            public ParserAlreadyExistAndExecutedNoReRun()
            {
                _tokenResult = new Mock<ITokenResult>();
                var pilotFactory = new Mock<IParserFactory>();
                _pilotMock = new Mock<ParserPilot>(pilotFactory.Object, _keywords, _errors) { CallBase = true };
                _parent = new Mock<ParserBase>(
                    TokenNames.Undefined,
                    _pilotMock.Object);

                _node = new Mock<ParserNode>(
                    _parent.Object,
                    new Mock<TokenParsingPosition>().Object);
                _tree = new Mock<IParserTree>();
                _pilotMock.SetupGet(m => m.CallTree).Returns(_tree.Object);
            }

            [Fact]
            public void NodeNull_ReturnFalse()
            {
                var tR = _tokenResult.Object;
                _pilotMock.Object.ParserAlreadyExistAndExecutedNoReRun(_parent.Object, null, out tR).Should().BeFalse();
                tR.Should().BeNull();
            }

            [Fact]
            public void NodeNotExecuted_ReturnFalse()
            {
                _node.SetupGet(m => m.Status).Returns(NodeParserStatus.Created);
                var tR = _tokenResult.Object;
                _pilotMock.Object.ParserAlreadyExistAndExecutedNoReRun(_parent.Object, _node.Object, out tR).Should().BeFalse();
                tR.Should().BeNull();
            }

            [Fact]
            public void RunTreeMemoryIsFalse_ReturnFalse()
            {
                _pilotMock.Protected().Setup<bool>(
                        "RunTreeMemory",
                        ItExpr.IsAny<ParserNode>())
                    .Returns(false)
                    .Verifiable();
                _node.SetupGet(m => m.Status).Returns(NodeParserStatus.Executed);
                var tR = _tokenResult.Object;
                _pilotMock.Object.ParserAlreadyExistAndExecutedNoReRun(_parent.Object, _node.Object, out tR).Should().BeFalse();
                tR.Should().BeNull();
                _pilotMock.Protected().Verify(
                    "RunTreeMemory",
                    Times.Once(),
                    ItExpr.Is<ParserNode>(p => p == _node.Object));
            }

            [Fact]
            public void NodeValid_ReturnTrue_TreeUpdated_ResultAssigned()
            {
                _pilotMock.Protected().Setup<bool>(
                        "RunTreeMemory",
                        ItExpr.IsAny<ParserNode>())
                    .Returns(true)
                    .Verifiable();
                _node.SetupGet(m => m.Status).Returns(NodeParserStatus.Executed);
                _tree.Setup(m => m.Get(It.IsAny<ParserBase>()))
                    .Returns(_node.Object)
                    .Verifiable();
                _tree.Setup(m => m.AddChild(It.IsAny<ParserBase>(), It.IsAny<ParserNode>()))
                    .Verifiable();

                _node.SetupGet(m => m.Result).Returns(_tokenResult.Object);
                _node.SetupGet(m => m.Parent).Returns(default(ParserNode))
                    .Verifiable();
                _pilotMock.Object.ParserAlreadyExistAndExecutedNoReRun(_parent.Object, _node.Object, out var tR)
                    .Should().BeTrue();
                tR.Should().Be(_tokenResult.Object);
                _pilotMock.Protected().Verify(
                    "RunTreeMemory",
                    Times.Once(),
                    ItExpr.Is<ParserNode>(p => p == _node.Object));
                _node.VerifyGet(m => m.Status, Times.Once);
                _node.VerifyGet(m => m.Result, Times.Once);
                _node.VerifyGet(m => m.Parent, Times.Once);
                _tree.Verify(m => m.Get(_parent.Object), Times.Once);
                _tree.Verify(m => m.AddChild(_parent.Object, _node.Object), Times.Once);
            }
        }

        public class IsInputValid
        {
            private readonly IList<ParsedKeyword> _keywords = new List<ParsedKeyword>();
            private readonly IList<ParserError> _errors = new List<ParserError>();
            private Mock<ParserPilot> _pilotMock;
            private readonly Mock<TokenResult> _tokenResult;
            private readonly Mock<ParserNode> _node;
            private ITokenParsingPosition _position;
            private readonly Mock<ParserBase> _parent;
            private readonly Mock<IParserTree> _tree;
            private readonly Mock<IParserFactory> _factory = new Mock<IParserFactory>();

            public IsInputValid()
            {
                _position = TokenParsingPosition.DefaultStartingPosition;
                _tokenResult = new Mock<TokenResult>(
                    new Mock<PluginBase.Token.Token>().Object,
                    new Mock<TokenParsingPosition>().Object);
                _factory = new Mock<IParserFactory>();
                _pilotMock = new Mock<ParserPilot>(_factory.Object, _keywords, _errors) { CallBase = true };
                _parent = new Mock<ParserBase>(
                    TokenNames.Undefined,
                    _pilotMock.Object);

                _node = new Mock<ParserNode>(
                    _parent.Object,
                    new Mock<TokenParsingPosition>().Object);
                _tree = new Mock<IParserTree>();
                _pilotMock.SetupGet(m => m.CallTree).Returns(_tree.Object);
            }

            [Fact]
            public void FactoryNull_Throw()
            {
                _pilotMock.SetupGet(m => m.Factory).Returns(default(IParserFactory));
                Action test = () => _pilotMock.Object.IsInputValid(ref _position, TokenNames.Undefined);
                test.Should().Throw<NotSupportedException>().Which.Message.Should().Be(nameof(ParserPilot.Factory));
                _position.Should().Be(TokenParsingPosition.DefaultStartingPosition);
            }

            [Fact]
            public void KeywordsNullOrEmpty_ReturnFalse()
            {
                _pilotMock.SetupGet(m => m.KeyWords).Returns(default(IList<PluginBase.Keyword.ParsedKeyword>));
                _pilotMock.Object.IsInputValid(ref _position, TokenNames.Undefined).Should().BeFalse();
                _position.Should().Be(TokenParsingPosition.DefaultStartingPosition);

                _pilotMock.SetupGet(m => m.KeyWords).Returns(new List<PluginBase.Keyword.ParsedKeyword>());
                _pilotMock.Object.IsInputValid(ref _position, TokenNames.Undefined).Should().BeFalse();
                _position.Should().Be(TokenParsingPosition.DefaultStartingPosition);
            }

            [Fact]
            public void PositionStartOverLastPosition_ReturnFalse()
            {
                _pilotMock.SetupGet(m => m.KeyWords).Returns(new List<PluginBase.Keyword.ParsedKeyword> { new PluginBase.Keyword.ParsedKeyword() });
                _pilotMock.SetupGet(m => m.LastPosition).Returns(-1);
                _position = null;
                _pilotMock.Object.IsInputValid(ref _position, TokenNames.Undefined).Should().BeFalse();
                _position.Should().Be(TokenParsingPosition.DefaultStartingPosition);
            }

            [Fact]
            public void FactoryParserNull_ReturnFalse()
            {
                _pilotMock.SetupGet(m => m.KeyWords).Returns(new List<PluginBase.Keyword.ParsedKeyword> { new PluginBase.Keyword.ParsedKeyword() });
                _factory.Setup(m => m.GetParser(It.IsAny<TokenNames>()))
                    .Returns(default(TypeInfo));
                _pilotMock.Object.IsInputValid(ref _position, TokenNames.Undefined).Should().BeFalse();
                _factory.Verify();
            }

            [Fact]
            public void AllInputValid_ReturnTrue()
            {
                _pilotMock.SetupGet(m => m.KeyWords).Returns(new List<PluginBase.Keyword.ParsedKeyword> { new PluginBase.Keyword.ParsedKeyword() });
                _factory.Setup(m => m.GetParser(It.IsAny<TokenNames>()))
                    .Returns(typeof(object).GetTypeInfo);
                _pilotMock.Object.IsInputValid(ref _position, TokenNames.Undefined).Should().BeTrue();
                _factory.Verify();
            }
        }

        // ReSharper disable once ClassNeverInstantiated.Global
        internal class ParserPilotOverride : ParserPilot
        {
            public bool RunPreExecutionOverride(ParserBase currentParser, ParserBase parent, int position)
            {
                return RunPreExecution(currentParser, parent, position);
            }

            public bool RunTreeMemoryOverride(ParserNode currentNode)
            {
                return RunTreeMemory(currentNode);
            }

            public ParserNode GetFromMemoryOverride(TokenNames name, int positionStart)
            {
                return GetFromMemory(name, positionStart);
            }

            public ParserPilotOverride(IParserFactory factory, IList<PluginBase.Keyword.ParsedKeyword> keyWords, IList<ParserError> errors = null) : base(factory, keyWords, errors)
            {
            }
        }

        public class RunPreExecution : IDisposable
        {
            private readonly Mock<ParserPilotOverride> _mock;
            private readonly Mock<IParserFactory> _factory;
            private readonly Mock<ParserBase> _current;
            private readonly Mock<ParserBase> _parent;
            private readonly Mock<PreParsingAttribute> _firstAttr;
            private readonly Mock<PreParsingAttribute> _secondAttr;
            public RunPreExecution()
            {
                _factory = new Mock<IParserFactory>();
                _mock = new Mock<ParserPilotOverride>(
                    _factory.Object,
                    new List<ParsedKeyword>(),
                    new List<ParserError>())
                { CallBase = true };
                _current = new Mock<ParserBase>(TokenNames.Undefined, _mock.Object);
                _parent = new Mock<ParserBase>(TokenNames.Undefined, _mock.Object);
                _firstAttr = new Mock<PreParsingAttribute>();
                _secondAttr = new Mock<PreParsingAttribute>();
            }

            [Fact]
            public void CurrentParserNull_ReturnTrue()
            {
                _mock.Object.RunPreExecutionOverride(null, _parent.Object, 0).Should().BeTrue();
            }

            [Fact]
            public void CurrentParserNoAttributes_ReturnTrue()
            {
                _current.SetupGet(m => m.Type).Returns(TokenNames.Undefined);

                var genericParserMock = new List<PreParsingAttribute>();

                var enAttrHelperMock = new Mock<EnumAttributeHelper>(TokenNames.Undefined);
                enAttrHelperMock.Setup(m => m.FindAttributes<PreParsingAttribute>())
                    .Returns(genericParserMock);

                EnumAttributeExtensions.EnumAttributeFactory = @enum => enAttrHelperMock.Object;

                _mock.Object.RunPreExecutionOverride(_current.Object, _parent.Object, 0).Should().BeTrue();

                enAttrHelperMock.Verify();
            }

            [Fact]
            public void CurrentParserOneAttributeFalse_ReturnFalse()
            {
                _current.SetupGet(m => m.Type).Returns(TokenNames.Undefined);

                _firstAttr.Setup(m => m.Execute(
                        It.IsAny<IParserTree>(),
                        It.IsAny<ParserBase>(),
                        It.IsAny<ParserBase>(),
                        It.IsAny<int>()))
                    .Returns(true);


                _secondAttr.Setup(m => m.Execute(
                        It.IsAny<IParserTree>(),
                        It.IsAny<ParserBase>(),
                        It.IsAny<ParserBase>(),
                        It.IsAny<int>()))
                    .Returns(false);

                var listAttributes = new List<PreParsingAttribute> { _firstAttr.Object, _secondAttr.Object };

                var enAttrHelperMock = new Mock<EnumAttributeHelper>(TokenNames.Undefined);
                enAttrHelperMock.Setup(m => m.FindAttributes<PreParsingAttribute>())
                    .Returns(listAttributes);

                EnumAttributeExtensions.EnumAttributeFactory = @enum => enAttrHelperMock.Object;

                _mock.Object.RunPreExecutionOverride(_current.Object, _parent.Object, 0).Should().BeFalse();

                enAttrHelperMock.Verify();
            }

            [Fact]
            public void CurrentParserAllAttributeTrue_ReturnTrue()
            {
                _current.SetupGet(m => m.Type).Returns(TokenNames.Undefined);

                _firstAttr.Setup(m => m.Execute(
                        It.IsAny<IParserTree>(),
                        It.IsAny<ParserBase>(),
                        It.IsAny<ParserBase>(),
                        It.IsAny<int>()))
                    .Returns(true);


                _secondAttr.Setup(m => m.Execute(
                        It.IsAny<IParserTree>(),
                        It.IsAny<ParserBase>(),
                        It.IsAny<ParserBase>(),
                        It.IsAny<int>()))
                    .Returns(true);

                var listAttributes = new List<PreParsingAttribute> { _firstAttr.Object, _secondAttr.Object };

                var enAttrHelperMock = new Mock<EnumAttributeHelper>(TokenNames.Undefined);
                enAttrHelperMock.Setup(m => m.FindAttributes<PreParsingAttribute>())
                    .Returns(listAttributes);

                EnumAttributeExtensions.EnumAttributeFactory = @enum => enAttrHelperMock.Object;

                _mock.Object.RunPreExecutionOverride(_current.Object, _parent.Object, 0).Should().BeTrue();

                enAttrHelperMock.Verify();
            }

            public void Dispose()
            {
                typeof(EnumAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
            }
        }

        public class RunTreeMemory : IDisposable
        {
            private readonly Mock<ParserPilotOverride> _mock;
            private readonly Mock<IParserFactory> _factory;
            private readonly Mock<ParserBase> _parser;
            private readonly Mock<ParserNode> _node;
            private readonly Mock<TreeMemoryAttribute> _firstAttr;
            private readonly Mock<TreeMemoryAttribute> _secondAttr;
            public RunTreeMemory()
            {
                typeof(EnumAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
                _factory = new Mock<IParserFactory>();
                _mock = new Mock<ParserPilotOverride>(
                    _factory.Object,
                    new List<ParsedKeyword>(),
                    new List<ParserError>())
                { CallBase = true };
                _parser = new Mock<ParserBase>(TokenNames.Undefined, _mock.Object);
                _node = new Mock<ParserNode>(_parser.Object, TokenParsingPosition.DefaultStartingPosition);
                _firstAttr = new Mock<TreeMemoryAttribute>();
                _secondAttr = new Mock<TreeMemoryAttribute>();
            }

            [Fact]
            public void CurrentNodeAndParserNull_ReturnTrue()
            {
                _mock.Object.RunTreeMemoryOverride(null).Should().BeTrue();

                _node.SetupGet(m => m.Parser).Returns(default(ParserBase));
                _mock.Object.RunTreeMemoryOverride(_node.Object).Should().BeTrue();
            }

            [Fact]
            public void CurrentParserNoAttributes_ReturnTrue()
            {
                _node.SetupGet(m => m.Parser).Returns(_parser.Object);
                _parser.SetupGet(m => m.Type).Returns(TokenNames.Undefined);

                var genericParserMock = new List<TreeMemoryAttribute>();

                var enAttrHelperMock = new Mock<EnumAttributeHelper>(TokenNames.Undefined);
                enAttrHelperMock.Setup(m => m.FindAttributes<TreeMemoryAttribute>())
                    .Returns(genericParserMock);

                EnumAttributeExtensions.EnumAttributeFactory = @enum => enAttrHelperMock.Object;

                _mock.Object.RunTreeMemoryOverride(_node.Object).Should().BeTrue();

                enAttrHelperMock.Verify();
            }

            [Fact]
            public void CurrentParserOneAttributeFalse_ReturnFalse()
            {
                _node.SetupGet(m => m.Parser).Returns(_parser.Object);
                _parser.SetupGet(m => m.Type).Returns(TokenNames.Undefined);

                _firstAttr.Setup(m => m.Execute(
                        It.IsAny<IParserTree>(),
                        It.IsAny<ParserNode>()))
                    .Returns(true);


                _secondAttr.Setup(m => m.Execute(
                        It.IsAny<IParserTree>(),
                        It.IsAny<ParserNode>()))
                    .Returns(false);

                var listAttributes = new List<TreeMemoryAttribute> { _firstAttr.Object, _secondAttr.Object };

                var enAttrHelperMock = new Mock<EnumAttributeHelper>(TokenNames.Undefined);
                enAttrHelperMock.Setup(m => m.FindAttributes<TreeMemoryAttribute>())
                    .Returns(listAttributes);

                EnumAttributeExtensions.EnumAttributeFactory = @enum => enAttrHelperMock.Object;

                _mock.Object.RunTreeMemoryOverride(_node.Object).Should().BeFalse();

                enAttrHelperMock.Verify();
            }

            [Fact]
            public void CurrentParserAllAttributeTrue_ReturnTrue()
            {
                _node.SetupGet(m => m.Parser).Returns(_parser.Object);
                _parser.SetupGet(m => m.Type).Returns(TokenNames.Undefined);

                _firstAttr.Setup(m => m.Execute(
                        It.IsAny<IParserTree>(),
                        It.IsAny<ParserNode>()))
                    .Returns(true);


                _secondAttr.Setup(m => m.Execute(
                        It.IsAny<IParserTree>(),
                        It.IsAny<ParserNode>()))
                    .Returns(true);

                var listAttributes = new List<TreeMemoryAttribute> { _firstAttr.Object, _secondAttr.Object };

                var enAttrHelperMock = new Mock<EnumAttributeHelper>(TokenNames.Undefined);
                enAttrHelperMock.Setup(m => m.FindAttributes<TreeMemoryAttribute>())
                    .Returns(listAttributes);

                EnumAttributeExtensions.EnumAttributeFactory = @enum => enAttrHelperMock.Object;

                _mock.Object.RunTreeMemoryOverride(_node.Object).Should().BeTrue();

                enAttrHelperMock.Verify();
            }

            public void Dispose()
            {
                typeof(EnumAttributeExtensions).GetTypeInfo().TypeInitializer.Invoke(null, null);
            }
        }

        public class GetKeyword
        {
            private readonly Mock<ParserPilot> _mock;
            private readonly Mock<IList<PluginBase.Keyword.ParsedKeyword>> _keyWords;

            public GetKeyword()
            {
                _keyWords = new Mock<IList<PluginBase.Keyword.ParsedKeyword>>();
                _mock = new Mock<ParserPilot>(
                    new Mock<IParserFactory>().Object,
                    _keyWords.Object,
                    new List<ParserError>())
                { CallBase = true };
                _mock.SetupGet(m => m.KeyWords).Returns(_keyWords.Object);
            }

            [Fact]
            public void PositionNegative_Throw()
            {
                Action test = () => _mock.Object.GetKeyword(-1);
                test.Should().Throw<IndexOutOfRangeException>();
            }


            [Fact]
            public void PositionOverLength_Throw()
            {
                _keyWords.SetupGet(m => m.Count).Returns(1);
                Action test = () => _mock.Object.GetKeyword(1);
                test.Should().Throw<IndexOutOfRangeException>();

                _keyWords.SetupGet(m => m.Count).Returns(0);
                test.Should().Throw<IndexOutOfRangeException>();
            }

            [Fact]
            public void ReturnKeywordAtPosition()
            {
                var result = new Mock<PluginBase.Keyword.ParsedKeyword>();
                _mock.SetupGet(m => m.KeyWords).Returns(new List<PluginBase.Keyword.ParsedKeyword>
                    {
                        new Mock<PluginBase.Keyword.ParsedKeyword>().Object,
                        result.Object
                    });
                _mock.Object.GetKeyword(1).Should().Be(result.Object);
            }
        }

        public class GetRemainingKeywords
        {
            private readonly Mock<ParserPilot> _mock;
            private readonly Mock<IList<PluginBase.Keyword.ParsedKeyword>> _keyWords;

            public GetRemainingKeywords()
            {
                _keyWords = new Mock<IList<PluginBase.Keyword.ParsedKeyword>>();
                _mock = new Mock<ParserPilot>(
                        new Mock<IParserFactory>().Object,
                        _keyWords.Object,
                        new List<ParserError>())
                { CallBase = true };
                _mock.SetupGet(m => m.KeyWords).Returns(_keyWords.Object);
            }

            [Fact]
            public void PositionNegative_ReturnEmptyList()
            {
                _mock.Object.GetRemainingKeywords(-1).Should().BeEmpty();
            }

            [Fact]
            public void PositionOverLength_ReturnEmptyList()
            {
                _keyWords.SetupGet(m => m.Count).Returns(1);
                _mock.Object.GetRemainingKeywords(1).Should().BeEmpty();

                _keyWords.SetupGet(m => m.Count).Returns(0);
                _mock.Object.GetRemainingKeywords(1).Should().BeEmpty();
            }

            [Fact]
            public void ReturnRemainingKeyWordsFromPosition()
            {
                var result = new Mock<PluginBase.Keyword.ParsedKeyword>();
                _mock.SetupGet(m => m.KeyWords).Returns(new List<PluginBase.Keyword.ParsedKeyword>
                {
                    new Mock<PluginBase.Keyword.ParsedKeyword>().Object,
                    result.Object
                });

                _mock.Object.GetRemainingKeywords(1)
                    .Should().HaveCount(1).And.Subject.As<IList<ParsedKeyword>>()
                    .Should().ContainInOrder(result.Object);
            }
        }

        public class GetFromMemory
        {

            private readonly Mock<ParserNode> _node;
            private readonly Mock<ParserBase> _parser;
            private readonly Mock<ParserPilotOverride> _mock;
            private readonly Mock<IParserFactory> _factory;
            private readonly Mock<IParserTree> _tree;
            public GetFromMemory()
            {
                _factory = new Mock<IParserFactory>();
                _tree = new Mock<IParserTree>();
                _mock = new Mock<ParserPilotOverride>(
                        _factory.Object,
                        new List<ParsedKeyword>(),
                        new List<ParserError>())
                { CallBase = true };
                _parser = new Mock<ParserBase>(TokenNames.Undefined, _mock.Object);
                _node = new Mock<ParserNode>(
                    _parser.Object,
                    TokenParsingPosition.DefaultStartingPosition);
            }

            [Fact]
            [Category("Input")]
            public void PositionNegative_ReturnNull()
            {
                _mock.Object.GetFromMemoryOverride(TokenNames.Undefined, -1).Should().BeNull();
            }

            [Fact]
            [Category("Input")]
            public void PositionOverLimit_ReturnNull()
            {
                _mock.SetupGet(m => m.LastPosition).Returns(1);
                _mock.Object.GetFromMemoryOverride(TokenNames.Undefined, 2).Should().BeNull();
            }

            [Fact]
            [Category("Logic")]
            public void CallGetFirstOfDefaultOnCallTree()
            {
                _mock.SetupGet(m => m.LastPosition).Returns(1);
                _mock.SetupGet(m => m.CallTree).Returns(_tree.Object);
                _node.SetupGet(m => m.Parser).Returns(_parser.Object);
                _parser.SetupGet(m => m.Type).Returns(TokenNames.Undefined);
                _node.SetupGet(m => m.Position).Returns(TokenParsingPosition.DefaultStartingPosition);
                var logicResult = false;

                _tree.Setup(m => m.GetFirstOrDefault(It.IsAny<Func<ParserNode, bool>>()))
                    .Callback<Func<ParserNode, bool>>(fun =>
                    {
                        logicResult = fun.Invoke(_node.Object);
                    });
                var origin = TokenParsingPosition.DefaultStartingPosition;
                _mock.Object.GetFromMemoryOverride(TokenNames.Undefined, origin.Start);
                _tree.Verify(m => m.GetFirstOrDefault(It.IsAny<Func<ParserNode, bool>>()), Times.Once);
                logicResult.Should().BeTrue();
            }
        }

    }
}
