using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;
using Moq;
using Xunit;

namespace Grammar.PluginBase.Test.Token
{
    public class MultiTokenResultSpec
    {
        public class Constructor
        {
            [Fact]
            public void Initialization()
            {
                var test = new MultiTokenResult(TokenParsingPosition.DefaultStartingPosition);
                test.ResultToken.Should().NotBeNull();
                test.ResultToken.Should().BeEmpty();
                test.Position.Should().Be(TokenParsingPosition.DefaultStartingPosition);
            }
        }

        public class AddToken
        {
            [Fact]
            public void UpdatePositionAndToken()
            {
                var token = new Mock<PluginBase.Token.Token>().Object;
                var position = new Mock<TokenParsingPosition>().Object;
                var result = new MultiTokenResult(TokenParsingPosition.DefaultStartingPosition);
                result.AddToken(token, position);
                result.ResultToken.Should().HaveCount(1);
                result.ResultToken.Should().ContainInOrder(token);
                result.Position.Should().BeSameAs(position);
            }
        }


        public class AddResult
        {
            [Fact]
            public void AddResultCallAddToken()
            {
                var tokenResult = new Mock<ITokenResult>();
                var token = new Mock<IToken>().Object;
                var position = new Mock<ITokenParsingPosition>().Object;
                tokenResult.SetupGet(m => m.ResultToken).Returns(token).Verifiable();
                tokenResult.SetupGet(m => m.Position).Returns(position).Verifiable();
                var result = new Mock<MultiTokenResult>(TokenParsingPosition.DefaultStartingPosition);
                result.Setup(m => m.AddToken(It.IsAny<IToken>(), It.IsAny<ITokenParsingPosition>())).Verifiable();
                result.Object.AddResult(tokenResult.Object);
                result.Verify(m => m.AddToken(token, position), Times.Once);
                tokenResult.VerifyGet(m => m.ResultToken, Times.Once);
                tokenResult.VerifyGet(m => m.Position, Times.Once);
            }
        }

        public class AddResults
        {
            [Fact]
            public void AddResultCallAddTokenOncePerResult()
            {
                var tokenResult = new Mock<MultiTokenResult>();
                var token = new Mock<PluginBase.Token.Token>().Object;
                var position = new Mock<TokenParsingPosition>().Object;
                tokenResult.SetupGet(m => m.ResultToken).Returns(new List<IToken> { token, token }).Verifiable();
                tokenResult.SetupGet(m => m.Position).Returns(position).Verifiable();

                var result = new Mock<MultiTokenResult>(TokenParsingPosition.DefaultStartingPosition);
                result.Setup(m => m.AddToken(It.IsAny<PluginBase.Token.Token>(), It.IsAny<TokenParsingPosition>())).Verifiable();

                result.Object.AddResults(tokenResult.Object);

                result.Verify(m => m.AddToken(token, position), Times.Exactly(2));
                tokenResult.VerifyGet(m => m.ResultToken, Times.Once);
                tokenResult.VerifyGet(m => m.Position, Times.Exactly(2));
            }
        }
    }
}
