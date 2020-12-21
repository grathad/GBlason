using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Grammar.PluginBase.Parser;
using Xunit;

namespace Grammar.PluginBase.Test.Parser
{
    public class ParserErrorExceptionSpec
    {
        public class Constructor
        {
            [Fact]
            public void DefaultInitialization()
            {
                var test = new ParserErrorException("explanation");
                test.Message.Should().Be("explanation");
                test.CharNumber.Should().Be(0);
                test.Length.Should().Be(0);
            }

            [Fact]
            public void Initialization()
            {
                var test = new ParserErrorException("", 2, 20);
                test.Message.Should().Be("");
                test.CharNumber.Should().Be(2);
                test.Length.Should().Be(20);
            }
        }
    }
}
