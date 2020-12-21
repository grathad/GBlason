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
    public class TokenResultSpec
    {
        public class Constructor
        {
            [Fact]
            public void Initialization()
            {
                var token = new Mock<IToken>();
                new TokenResult(token.Object, null).ResultToken.Should().Be(token.Object);
            }
        }
    }
}
