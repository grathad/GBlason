using FluentAssertions;
using Format.Elements;
using Moq;
using Xunit;

namespace Grammar.Test
{
    public class ParsingResultSpec
    {
        public class Constructor
        {
            [Fact]
            public void Default()
            {
                new ParsingResult(null).Format.Should().BeNull();
                var input = new Mock<BaseElement>().Object;
                new ParsingResult(input).Format.Should().Be(input);
            }
        }
    }
}
