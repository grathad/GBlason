using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentAssertions;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using Moq.Protected;
using Xunit;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Grammar.PluginBase.Test.Parser
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ContainerParserSpecs
    {
        internal class ContainerParserSpy : ContainerParser
        {
            public ContainerToken CurrentTokenOverride => CurrentToken;

            public ContainerParserSpy(TokenNames type, IParserPilot pilot)
                : base(type, pilot)
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

            public ITokenResult TryConsumeOrOverride(
                ref ITokenParsingPosition origin,
                params TokenNames[] names)
            {
                return TryConsumeOr(ref origin, names);
            }

            public ITokenResult TryConsumeOrOverride(
                int originPosition,
                params TokenNames[] names)
            {
                return TryConsumeOr(originPosition, names);
            }

            public void AttachChildAfterOverride(
                PluginBase.Token.Token child,
                PluginBase.Token.Token childBefore = null)
            {
                AttachChildAfter(child, childBefore);
            }

            public void AttachChildOverride(IToken child)
            {
                AttachChild(child);
            }

            public void AttachChildrenOverride(IEnumerable<PluginBase.Token.Token> children)
            {
                AttachChildren(children);
            }

            public bool TryConsumeAndAttachOneOverride(ref ITokenParsingPosition position, TokenNames typeName)
            {
                return TryConsumeAndAttachOne(ref position, typeName);
            }
        }


        public class Constructor
        {
            [Fact]
            public void Initialization()
            {
                var pilotMock = new Mock<IParserPilot>();
                var test = new ContainerParserSpy(TokenNames.Undefined, pilotMock.Object);
                test.CurrentTokenOverride.Type.Should().Be(TokenNames.Undefined);
                test.ParserPilot.Should().Be(pilotMock.Object);
            }
        }

        public class AttachChildAfter
        {
            public AttachChildAfter()
            {
                PilotMock = new Mock<IParserPilot>();
                Token = new Mock<PluginBase.Token.Token>();
                SecondToken = new Mock<PluginBase.Token.Token>();
                defaultParserSpy = new ContainerParserSpy(TokenNames.Undefined, PilotMock.Object);
            }

            internal Mock<IParserPilot> PilotMock;

            internal Mock<PluginBase.Token.Token> Token;
            internal Mock<PluginBase.Token.Token> SecondToken;

            internal ContainerParserSpy defaultParserSpy;

            [Fact]
            public void NullChild_Return()
            {
                defaultParserSpy.AttachChildAfterOverride(null, Token.Object);
                defaultParserSpy.CurrentTokenOverride.Children.Should().BeEmpty();
            }

            [Fact]
            public void NullChildBefore_InsertAtStart()
            {
                defaultParserSpy.AttachChildAfterOverride(Token.Object);
                defaultParserSpy.CurrentTokenOverride.Children.Should().HaveCount(1);
                defaultParserSpy.CurrentTokenOverride.Children.Should().ContainInOrder(Token.Object);
                defaultParserSpy.AttachChildAfterOverride(SecondToken.Object);
                defaultParserSpy.CurrentTokenOverride.Children.Should().HaveCount(2);
                defaultParserSpy.CurrentTokenOverride.Children.Should().ContainInOrder(SecondToken.Object, Token.Object);
            }

            [Fact]
            public void NotFoundChildBefore_InsertAtStart()
            {
                defaultParserSpy.AttachChildAfterOverride(Token.Object);
                defaultParserSpy.CurrentTokenOverride.Children.Should().HaveCount(1);
                defaultParserSpy.CurrentTokenOverride.Children.Should().ContainInOrder(Token.Object);
                //now we try to insert after a non existing child ao the index should be -1
                defaultParserSpy.AttachChildAfterOverride(SecondToken.Object, SecondToken.Object);
                defaultParserSpy.CurrentTokenOverride.Children.Should().HaveCount(2);
                defaultParserSpy.CurrentTokenOverride.Children.Should().ContainInOrder(SecondToken.Object, Token.Object);
            }

            [Fact]
            public void ValidChildBefore_InsertAfter()
            {
                defaultParserSpy.AttachChildAfterOverride(Token.Object);
                defaultParserSpy.CurrentTokenOverride.Children.Should().HaveCount(1);
                defaultParserSpy.CurrentTokenOverride.Children.Should().ContainInOrder(Token.Object);
                //now we try to insert after an existing child so the index should be 1
                defaultParserSpy.AttachChildAfterOverride(SecondToken.Object, Token.Object);
                defaultParserSpy.CurrentTokenOverride.Children.Should().HaveCount(2);
                defaultParserSpy.CurrentTokenOverride.Children.Should().ContainInOrder(Token.Object, SecondToken.Object);
            }
        }

        public class AttachChild
        {
            [Fact]
            public void InputUsedToCallAttachChildAfter()
            {
                var csMock = new Mock<ContainerParserSpy>(TokenNames.Undefined, default(IParserPilot))
                { CallBase = true };

                IToken output = null;
                csMock
                    .Protected()
                    .Setup("AttachChildAfter", ItExpr.IsAny<IToken>(), ItExpr.IsAny<IToken>())
                    .Callback<IToken, IToken>((token, after) =>
                    {
                        output = token;
                    });
                var input = new Mock<IToken>();
                csMock.Object.AttachChildOverride(input.Object);
                output.Should().Be(input.Object);
            }
        }

        public class AttachChildren
        {
            public AttachChildren()
            {
                var pilotMock = new Mock<IParserPilot>();
                Token = new Mock<PluginBase.Token.Token>();
                SecondToken = new Mock<PluginBase.Token.Token>();
                _defaultParserSpy = new Mock<ContainerParserSpy>(TokenNames.Undefined, pilotMock.Object) { CallBase = true };
            }

            internal Mock<PluginBase.Token.Token> Token;
            internal Mock<PluginBase.Token.Token> SecondToken;

            private readonly Mock<ContainerParserSpy> _defaultParserSpy;

            [Fact]
            public void ChildrenNull_Throw()
            {
                Action test = () => _defaultParserSpy.Object.AttachChildrenOverride(null);
                test.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("children");
            }

            [Fact]
            public void ChildrenEmpty_NoCall()
            {
                _defaultParserSpy
                    .Protected()
                    .Setup("AttachChildAfter", ItExpr.IsAny<PluginBase.Token.Token>(), ItExpr.IsAny<PluginBase.Token.Token>())
                    .Verifiable();
                _defaultParserSpy.Object.AttachChildrenOverride(new List<PluginBase.Token.Token>());
                _defaultParserSpy.Protected().Verify("AttachChildAfter", Times.Never(), ItExpr.IsAny<PluginBase.Token.Token>(), ItExpr.IsAny<PluginBase.Token.Token>());
            }

            [Fact]
            public void ChildrenFull_CallOncePerChild()
            {
                _defaultParserSpy
                    .Protected()
                    .Setup("AttachChildAfter", ItExpr.IsAny<PluginBase.Token.Token>(), ItExpr.IsAny<PluginBase.Token.Token>())
                    .Verifiable();
                var tokenMockObject = new Mock<PluginBase.Token.Token>().Object;
                _defaultParserSpy.Object.AttachChildrenOverride(new List<PluginBase.Token.Token> { tokenMockObject, tokenMockObject, tokenMockObject });
                _defaultParserSpy.Protected().Verify(
                    "AttachChildAfter",
                    Times.Exactly(3),
                    ItExpr.IsAny<PluginBase.Token.Token>(),
                    ItExpr.IsAny<PluginBase.Token.Token>());
            }
        }

        public class TryConsumeOr
        {
            public TryConsumeOr()
            {
                var pilotMock = new Mock<IParserPilot>();
                _tokenParsingPosition = TokenParsingPosition.DefaultStartingPosition;
                _defaultParserSpyMock = new Mock<ContainerParserSpy>(TokenNames.Undefined, pilotMock.Object) { CallBase = true };
            }

            private ITokenParsingPosition _tokenParsingPosition;

            private readonly Mock<ContainerParserSpy> _defaultParserSpyMock;

            [Fact]
            public void NullOrigin_Throw()
            {
                _tokenParsingPosition = null;
                Action test = () => _defaultParserSpyMock.Object.TryConsumeOrOverride(
                    ref _tokenParsingPosition, TokenNames.Undefined);

                test.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("origin");
            }

            [Fact]
            public void NullNames_Throw()
            {
                Action test = () => _defaultParserSpyMock.Object.TryConsumeOrOverride(
                    ref _tokenParsingPosition, default(TokenNames[]));

                test.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("names");
            }

            [Fact]
            public void EmptyNames_ReturnNull()
            {
                _defaultParserSpyMock.Object.TryConsumeOrOverride(
                    ref _tokenParsingPosition).Should().BeNull();
            }

            [Fact]
            public void OneChoiceNotfound_ReturnNull()
            {
                //null
                _defaultParserSpyMock.Protected().Setup<ITokenResult>(
                        "Parse",
                        ItExpr.IsAny<ITokenParsingPosition>(),
                        ItExpr.IsAny<TokenNames>())
                    .Returns(default(ITokenResult))
                    .Verifiable();
                //this one should return the longest result being null
                _defaultParserSpyMock.Object
                    .TryConsumeOrOverride(ref _tokenParsingPosition, TokenNames.Undefined)
                    .Should().BeNull();
                _defaultParserSpyMock.Protected().Verify(
                    "Parse",
                    Times.Once(),
                    _tokenParsingPosition,
                    TokenNames.Undefined);
                _tokenParsingPosition.Start.Should().Be(0);
            }


            [Fact]
            public void OneChoiceFound_Returned()
            {
                var foundChoice = new Mock<ITokenResult>();
                var foundPosition = new Mock<ITokenParsingPosition>();
                foundPosition.SetupGet(m => m.Start)
                    .Returns(1)
                    .Verifiable();
                foundChoice.SetupGet(m => m.Position)
                    .Returns(foundPosition.Object)
                    .Verifiable();
                //null
                _defaultParserSpyMock.Protected().Setup<ITokenResult>(
                        "Parse",
                        ItExpr.IsAny<ITokenParsingPosition>(),
                        ItExpr.IsAny<TokenNames>())
                    .Returns(foundChoice.Object)
                    .Verifiable();
                //this one should return the longest result being null
                _defaultParserSpyMock.Object
                    .TryConsumeOrOverride(ref _tokenParsingPosition, TokenNames.Undefined)
                    .Should().Be(foundChoice.Object);
                _defaultParserSpyMock.Protected().Verify(
                    "Parse",
                    Times.Once(),
                    ItExpr.Is<ITokenParsingPosition>(o => o.Start == 0),
                    TokenNames.Undefined);
                _tokenParsingPosition.Should().BeSameAs(foundPosition.Object);
            }

            [Fact]
            public void MultipleChoices_ReturnLongest()
            {
                var foundChoiceShort = new Mock<ITokenResult>();
                var foundChoiceLong = new Mock<ITokenResult>();
                var foundPositionShort = new Mock<ITokenParsingPosition>();
                foundPositionShort.SetupGet(m => m.Start)
                    .Returns(1);
                var foundPositionLong = new Mock<ITokenParsingPosition>();
                foundPositionLong.SetupGet(m => m.Start)
                    .Returns(10);
                foundChoiceShort.SetupGet(m => m.Position)
                    .Returns(foundPositionShort.Object);
                foundChoiceLong.SetupGet(m => m.Position)
                    .Returns(foundPositionLong.Object);
                //null
                _defaultParserSpyMock.Protected().SetupSequence<ITokenResult>(
                        "Parse",
                        ItExpr.IsAny<ITokenParsingPosition>(),
                        ItExpr.IsAny<TokenNames>())
                    .Returns(foundChoiceLong.Object)
                    .Returns(foundChoiceShort.Object);
                //this one should return the longest result being null
                _defaultParserSpyMock.Object
                    .TryConsumeOrOverride(ref _tokenParsingPosition, TokenNames.Undefined, TokenNames.Undefined)
                    .Should().Be(foundChoiceLong.Object);
                _defaultParserSpyMock.Protected().Verify(
                    "Parse",
                    Times.Exactly(2),
                    ItExpr.Is<ITokenParsingPosition>(o => o.Start == 0),
                    TokenNames.Undefined);
                _tokenParsingPosition.Should().BeSameAs(foundPositionLong.Object);
            }

            [Fact]
            public void IntVersion_CallReferenceOne()
            {
                _defaultParserSpyMock.Protected().Setup(
                    "TryConsumeOr",
                    ItExpr.Ref<TokenParsingPosition>.IsAny,
                    ItExpr.IsAny<TokenNames[]>()).Verifiable();
                _defaultParserSpyMock.Object.TryConsumeOrOverride(0, TokenNames.Undefined);
                _defaultParserSpyMock.Protected().Verify(
                    "TryConsumeOr",
                    Times.Once(),
                    ItExpr.Ref<TokenParsingPosition>.IsAny,
                    ItExpr.Is<TokenNames[]>(o => o.Length == 1 && o[0] == TokenNames.Undefined));
            }
        }

        public class TryConsumeAndAttachOne
        {
            public TryConsumeAndAttachOne()
            {
                _parserPilot = new Mock<IParserPilot>();
                _tokenParsingPosition = TokenParsingPosition.DefaultStartingPosition;
                _defaultParserSpyMock = new Mock<ContainerParserSpy>(TokenNames.Undefined, _parserPilot.Object) { CallBase = true };
            }

            private Mock<IParserPilot> _parserPilot;

            private ITokenParsingPosition _tokenParsingPosition;

            private readonly Mock<ContainerParserSpy> _defaultParserSpyMock;

            [Fact]
            public void NullPosition_Throws()
            {
                _tokenParsingPosition = null;
                Action test = () => _defaultParserSpyMock.Object.TryConsumeAndAttachOneOverride(
                    ref _tokenParsingPosition, TokenNames.Undefined);
                test.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("position");
            }

            [Fact]
            public void NotFound_ReturnFalse_PositionUnchanged()
            {
                _defaultParserSpyMock.Protected().Setup<ITokenResult>(
                        "Parse",
                        ItExpr.IsAny<ITokenParsingPosition>(),
                        ItExpr.IsAny<TokenNames>())
                    .Returns(default(ITokenResult))
                    .Verifiable();

                _defaultParserSpyMock.Object.TryConsumeAndAttachOneOverride(
                    ref _tokenParsingPosition, TokenNames.Undefined).Should().BeFalse();
                _defaultParserSpyMock.Protected().Verify(
                    "Parse",
                    Times.Once(),
                    ItExpr.IsAny<ITokenParsingPosition>(),
                    ItExpr.IsAny<TokenNames>());
                _tokenParsingPosition.Start.Should().Be(0);
            }

            [Fact]
            public void Found_ReturnTrue_AttachChild_PositionChanged()
            {
                var foundChoice = new Mock<ITokenResult>();
                var foundPosition = new Mock<ITokenParsingPosition>();
                foundPosition.SetupGet(m => m.Start)
                    .Returns(1);
                foundChoice.SetupGet(m => m.Position)
                    .Returns(foundPosition.Object);
                var resultToken = new Mock<IToken>();
                foundChoice.SetupGet(m => m.ResultToken)
                    .Returns(resultToken.Object);

                _defaultParserSpyMock.Protected().Setup<ITokenResult>(
                        "Parse",
                        ItExpr.IsAny<ITokenParsingPosition>(),
                        ItExpr.IsAny<TokenNames>())
                    .Returns(foundChoice.Object)
                    .Verifiable();

                _defaultParserSpyMock.Protected().Setup(
                        "AttachChild",
                        ItExpr.IsAny<IToken>())
                    .Verifiable();

                _defaultParserSpyMock.Object.TryConsumeAndAttachOneOverride(
                    ref _tokenParsingPosition, TokenNames.Undefined).Should().BeTrue();
                _defaultParserSpyMock.Protected().Verify(
                    "Parse",
                    Times.Once(),
                    ItExpr.IsAny<ITokenParsingPosition>(),
                    ItExpr.IsAny<TokenNames>());
                _defaultParserSpyMock.Protected().Verify(
                    "AttachChild",
                    Times.Once(),
                    ItExpr.Is<IToken>(o => o == foundChoice.Object.ResultToken));
                _tokenParsingPosition.Start.Should().Be(1);

            }
        }
    }
}
