using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using FluentAssertions;
using Grammar.PluginBase.Attributes;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Moq;
using Xunit;

namespace Grammar.PluginBase.Test.Attributes
{
    public class ContainsOrAttributeSpec
    {
        public class Constructor
        {
            [Fact]
            public void NullInput()
            {
                new ContainsOrAttribute(default(TokenNames[])).Names.Should().BeNull();
            }

            [Fact]
            public void ValidInput()
            {
                var input = new[] { TokenNames.Undefined, TokenNames.Undefined };
                var test = new ContainsOrAttribute(input);
                test.Names.Should().HaveCount(2);
                test.Names.Should().ContainInOrder(TokenNames.Undefined, TokenNames.Undefined);
            }
        }

        public class GetGenericParserType
        {
            [Fact]
            public void ReturnContainerOrParserTypeInfo()
            {
                new ContainsOrAttribute(default(TokenNames[])).GetGenericParserType().Should()
                    .Be(typeof(ContainerOrParser).GetTypeInfo());
            }
        }

        public class CreateGenericParserInstance
        {
            [Fact]
            public void ReturnContainerOrParserInstance()
            {
                var mock = new Mock<ContainsOrAttribute>(default(TokenNames[])) { CallBase = true };
                mock.SetupGet(m => m.Names).Verifiable();
                var result = mock.Object.CreateGenericParserInstance(TokenNames.Undefined, new Mock<IParserPilot>().Object);
                result.Should().NotBeNull()
                    .And.Subject.Should().BeOfType<ContainerOrParser>();
                mock.VerifyGet(m => m.Names, Times.Once);
            }
        }
    }
}
