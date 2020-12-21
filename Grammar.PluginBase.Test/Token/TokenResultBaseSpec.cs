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
    public class TokenResultBaseSpec
    {
        public class Constructor
        {
            [Fact]
            public void Initialize()
            {
                var position = new Mock<ITokenParsingPosition>();
                var test = new Mock<TokenResultBase>(position.Object) { CallBase = true };
                test.Object.Position.Should().Be(position.Object);
            }
        }

        public class Position
        {
            internal class TestResult : TokenResultBase
            {
                public ITokenParsingPosition PositionOverride {
                    set => Position = value;
                }

                public TestResult(ITokenParsingPosition position) : base(position)
                {
                }
            }

            [Fact]
            public void Setter()
            {
                var position = new Mock<ITokenParsingPosition>();
                var test = new TestResult(position.Object);
                var replacement = new Mock<ITokenParsingPosition>();
                test.PositionOverride = replacement.Object;
                test.Position.Should().BeSameAs(replacement.Object);
                replacement.Object.Should().NotBeSameAs(position.Object);
            }
        }
    }
}
