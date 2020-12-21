using System.Collections.Generic;
using FluentAssertions;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Moq;
using Xunit;

namespace Grammar.PluginBase.Test.Parser
{
    public class ParserNodeSpec
    {
        public class Constructor
        {
            [Fact]
            public void Initialization()
            {
                var parser = new Mock<ParserBase>(TokenNames.Undefined, new Mock<IParserPilot>().Object);
                var test = new ParserNode(parser.Object, TokenParsingPosition.DefaultStartingPosition);
                test.Position.Should().Be(TokenParsingPosition.DefaultStartingPosition);
                test.Parser.Should().Be(parser.Object);
            }

            [Fact]
            public void NullParser_StillInitialize()
            {
                var test = new ParserNode(default(ParserBase), TokenParsingPosition.DefaultStartingPosition);
                test.Position.Should().Be(TokenParsingPosition.DefaultStartingPosition);
                test.Parser.Should().BeNull();
            }
        }

        public class ParserOnError
        {
            [Fact]
            public void AddNewError()
            {
                var newError = new Mock<ParserError>(0, 0, "").Object;
                var test = new ParserNode(default(ParserBase), TokenParsingPosition.DefaultStartingPosition);
                test.ParserOnError(null, newError);
                test.Errors.Should().HaveCount(1);
                test.Errors.Should().ContainInOrder(newError);
            }
        }

        public new class ToString
        {
            private readonly Mock<ParserNode> _mock;
            private readonly Mock<ParserBase> _parserBase;
            public ToString()
            {
                _parserBase = new Mock<ParserBase>(TokenNames.Undefined, new Mock<IParserPilot>().Object);
                _mock = new Mock<ParserNode>(_parserBase.Object, TokenParsingPosition.DefaultStartingPosition)
                { CallBase = true };
            }

            [Fact]
            public void NoPositionNoTypeNotExecuted()
            {
                _mock.SetupGet(m => m.Position).Returns(default(TokenParsingPosition));
                _mock.SetupGet(m => m.Parser).Returns(default(ParserBase));
                _mock.SetupGet(m => m.Executed).Returns(false);
                _mock.Object.ToString().Should()
                    .Be($"{ParserNode.NoPositionText},{ParserNode.NoTypeText},{ParserNode.NotExecutedText}");
            }

            [Fact]
            public void ResultFoundWithPositionAndType()
            {
                _mock.SetupGet(m => m.Position).Returns(TokenParsingPosition.DefaultStartingPosition);
                _parserBase.SetupGet(m => m.Type).Returns(TokenNames.Undefined);
                _mock.SetupGet(m => m.Parser).Returns(_parserBase.Object);
                _mock.SetupGet(m => m.Executed).Returns(true);
                var token = new Mock<PluginBase.Token.Token>();
                var tokenResult = new Mock<TokenResult>(
                    token.Object,
                    TokenParsingPosition.DefaultStartingPosition);
                tokenResult.SetupGet(m => m.ResultToken).Returns(token.Object);
                tokenResult.SetupGet(m => m.Position).Returns(TokenParsingPosition.DefaultStartingPosition);
                _mock.SetupGet(m => m.Result).Returns(tokenResult.Object);
                _mock.Object.ToString().Should()
                    .Be($"0,{TokenNames.Undefined},found using 0 key words");
            }

            [Fact]
            public void ErrorsFound()
            {
                _mock.SetupGet(m => m.Position).Returns(TokenParsingPosition.DefaultStartingPosition);
                _parserBase.SetupGet(m => m.Type).Returns(TokenNames.Undefined);
                _mock.SetupGet(m => m.Parser).Returns(_parserBase.Object);
                _mock.SetupGet(m => m.Executed).Returns(true);
                var tokenResult = new Mock<TokenResult>(
                    default(PluginBase.Token.Token),
                    TokenParsingPosition.DefaultStartingPosition);
                tokenResult.SetupGet(m => m.ResultToken).Returns(default(PluginBase.Token.Token));
                _mock.SetupGet(m => m.Errors).Returns(new List<ParserError> { default(ParserError) });
                _mock.Object.ToString().Should()
                    .Be($"0,{TokenNames.Undefined},1 error(s)");
            }

            [Fact]
            public void NoResult()
            {
                _mock.SetupGet(m => m.Position).Returns(TokenParsingPosition.DefaultStartingPosition);
                _parserBase.SetupGet(m => m.Type).Returns(TokenNames.Undefined);
                _mock.SetupGet(m => m.Parser).Returns(_parserBase.Object);
                _mock.SetupGet(m => m.Executed).Returns(true);
                var tokenResult = new Mock<TokenResult>(
                    default(PluginBase.Token.Token),
                    TokenParsingPosition.DefaultStartingPosition);
                tokenResult.SetupGet(m => m.ResultToken).Returns(default(PluginBase.Token.Token));
                _mock.SetupGet(m => m.Errors).Returns(default(IList<ParserError>));
                _mock.Object.ToString().Should()
                    .Be($"0,{TokenNames.Undefined},{ParserNode.NoResultText}");
            }
        }
    }
}
