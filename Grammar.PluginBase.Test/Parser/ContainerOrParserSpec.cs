using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;
using Moq;
using Moq.Protected;
using Xunit;

namespace Grammar.PluginBase.Test.Parser
{
    public class ContainerOrParserSpec
    {
        public class Constructor
        {
            [Fact]
            public void Initialization()
            {
                var testInput = new[] { TokenNames.Undefined };
                var test = new ContainerOrParser(
                    TokenNames.Undefined,
                    new Mock<IParserPilot>().Object,
                    testInput);
                test.Options.Should().HaveCount(1);
                test.Options.Should().ContainInOrder(TokenNames.Undefined);
            }
        }

        public class TryConsume
        {
            [Fact]
            public void NullResult_ReturnNull()
            {
                var mock = new Mock<ContainerOrParser>(
                    TokenNames.Undefined,
                    new Mock<IParserPilot>().Object,
                    new[] { TokenNames.Undefined, TokenNames.Undefined })
                { CallBase = true };

                mock.Protected().Setup<ITokenResult>("TryConsumeOr",
                        ItExpr.Ref<ITokenParsingPosition>.IsAny,
                        ItExpr.IsAny<TokenNames[]>())
                    .Returns(default(ITokenResult));

                var origin = TokenParsingPosition.DefaultStartingPosition;
                mock.Object.TryConsume(ref origin).Should().BeNull();
                mock.Protected().Verify(
                    "TryConsumeOr",
                    Times.Once(),
                    ItExpr.Ref<ITokenParsingPosition>.IsAny,
                    ItExpr.IsAny<TokenNames[]>());
                origin.Should().Be(TokenParsingPosition.DefaultStartingPosition);
            }

            [Fact]
            public void ValidResult_ReturnAndAttachChild()
            {
                var token = new Mock<IToken>();
                var result = new Mock<ITokenResult>();
                result.SetupGet(m => m.ResultToken).Returns(token.Object);

                var mock = new Mock<ContainerOrParser>(
                    TokenNames.Undefined,
                    new Mock<IParserPilot>().Object,
                    new[] { TokenNames.Undefined, TokenNames.Undefined })
                { CallBase = true };

                mock.Protected().Setup<ITokenResult>("TryConsumeOr",
                        ItExpr.Ref<ITokenParsingPosition>.IsAny,
                        ItExpr.IsAny<TokenNames[]>())
                    .Returns(result.Object);

                mock.Protected().Setup("AttachChild",
                    ItExpr.IsAny<IToken>()).Verifiable();

                var currentToken = new Mock<ContainerToken>();
                currentToken.Setup(m => m.AsTokenResult(It.IsAny<ITokenResult>()))
                    .Returns(result.Object);

                var calls = 0;
                mock.Protected()
                    .SetupGet<ContainerToken>("CurrentToken")
                    .Returns(() =>
                    {
                        calls++;
                        return currentToken.Object;
                    })
                    .Verifiable();

                var origin = TokenParsingPosition.DefaultStartingPosition;
                mock.Object.TryConsume(ref origin).Should().Be(result.Object);

                mock.Protected().Verify(
                    "TryConsumeOr",
                    Times.Once(),
                    ItExpr.Ref<ITokenParsingPosition>.IsAny,
                    ItExpr.IsAny<TokenNames[]>());
                mock.Protected().Verify(
                    "AttachChild",
                    Times.Once(),
                    ItExpr.Is<IToken>(o => o == token.Object));
                calls.Should().Be(1);
                result.VerifyGet(m => m.ResultToken);
                currentToken.Verify();
                origin.Should().Be(TokenParsingPosition.DefaultStartingPosition);
            }
        }
    }
}
