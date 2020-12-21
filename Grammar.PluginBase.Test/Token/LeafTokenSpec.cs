using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Token;
using Moq;
using Xunit;

namespace Grammar.PluginBase.Test.Token
{
    public class LeafTokenSpec
    {
        public class Constructor
        {
            [Fact]
            public void Initialization()
            {
                new LeafToken().OriginalKw.Should().NotBeNull()
                    .And.Subject.As<List<ParsedKeyword>>().Should().BeEmpty();
            }
        }

        public new class ToString
        {
            private Mock<LeafToken> _mock;
            public ToString()
            {
                _mock = new Mock<LeafToken> { CallBase = true };
            }

            [Fact]
            public void NoKeyWords_ReturnNullString()
            {
                _mock.SetupGet(m => m.OriginalKw).Returns(default(List<ParsedKeyword>));
                _mock.Object.ToString().Should().NotBeEmpty();

                _mock.SetupGet(m => m.OriginalKw).Returns(new List<ParsedKeyword>());
                _mock.Object.ToString().Should().NotBeEmpty();

                _mock.VerifyGet(m => m.OriginalKw, Times.Exactly(3));
            }

            [Fact]
            public void MultipleKeywords_ReturnProperString()
            {
                var pkw = new Mock<ParsedKeyword>();
                pkw.SetupSequence(m => m.Value)
                    .Returns("1")
                    .Returns("2")
                    .Returns("3");
                _mock.SetupGet(m => m.OriginalKw).Returns(
                    new List<ParsedKeyword>
                    {
                        pkw.Object,
                        pkw.Object,
                        pkw.Object
                    });
                _mock.Object.ToString().Should().Be("1 2 3 ");
            }
        }
    }
}
