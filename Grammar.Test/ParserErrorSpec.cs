using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Grammar.Test
{
    public class ParserErrorSpec
    {
        public class Constructor
        {
            [Fact]
            public void DefaultLength()
            {
                var test = new ParserError(0,"");
                test.Length.Should().Be(0);
            }

            [Fact]
            public void Initialization()
            {
                var test = new ParserError(10, -5, "exp");
                test.Length.Should().Be(-5);
                test.CharacterNumber.Should().Be(10);
                test.Explanation.Should().Be("exp");
            }
        }
    }
}
