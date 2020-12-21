using System;
using System.Collections.Generic;
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
    public class RedundancyAttributeSpec
    {
        public class Constructor
        {
            [Fact]
            public void EmptyConstructor()
            {
                var test = new RedundancyAttribute();
                test.CanBeRedundant.Should().Be(RedundancyAttribute.DefaultRedudancy);
                test.ChainBreakers.Should().BeNull();
            }

            [Fact]
            public void RedundancyInitialization()
            {
                var test = new RedundancyAttribute(!RedundancyAttribute.DefaultRedudancy);
                test.CanBeRedundant.Should().Be(!RedundancyAttribute.DefaultRedudancy);
                test.ChainBreakers.Should().BeNull();
            }


            [Fact]
            public void TokenNamesInitialization()
            {
                var test = new RedundancyAttribute(TokenNames.Undefined);
                test.CanBeRedundant.Should().Be(true);
                test.ChainBreakers.Should().HaveCount(1).And.Subject.Should().ContainInOrder(TokenNames.Undefined);
            }

            [Fact]
            public void TokenNamesNull_DefaultInitialization()
            {
                var test = new RedundancyAttribute(default(TokenNames[]));
                test.CanBeRedundant.Should().Be(RedundancyAttribute.DefaultRedudancy);
                test.ChainBreakers.Should().BeNull();
            }
        }

        public class Execute
        {
            private Mock<IParserTree> _treeMock;
            private Mock<ParserBase> _parserBaseMock;

            public Execute()
            {
                _treeMock = new Mock<IParserTree>();
                _parserBaseMock = new Mock<ParserBase>(default(TokenNames), default(IParserPilot));
            }

            [Fact]
            public void NullTree_ReturnTrue()
            {
                new RedundancyAttribute()
                .Execute(null, _parserBaseMock.Object, _parserBaseMock.Object, 0)
                .Should().BeTrue();
            }


            [Fact]
            public void NullCurrentParser_ReturnTrue()
            {
                new RedundancyAttribute()
                    .Execute(_treeMock.Object, null, _parserBaseMock.Object, 0)
                    .Should().BeTrue();
            }


            [Fact]
            public void NullParentParser_ReturnTrue()
            {
                new RedundancyAttribute()
                    .Execute(_treeMock.Object, _parserBaseMock.Object, null, 0)
                    .Should().BeTrue();
            }

            [Fact]
            public void NoChainDirectParentFound_ReturnFalse()
            {
                //both parser have the same type
                _parserBaseMock.SetupGet(m => m.Type).Returns(TokenNames.Undefined);

                //the redundant flag is set to true
                new RedundancyAttribute(true).Execute(_treeMock.Object, _parserBaseMock.Object, _parserBaseMock.Object, 0)
                .Should().BeFalse();
                _parserBaseMock.VerifyGet(m => m.Type, Times.Exactly(2));
            }


            [Fact]
            public void NoChainDirectParentNotFound_ReturnTrue()
            {
                //both parser have different types
                _parserBaseMock.SetupSequence(m => m.Type)
                    .Returns(TokenNames.Undefined)
                    .Returns(TokenNames.Shield);

                //the redundant flag is set to true
                new RedundancyAttribute(true).Execute(_treeMock.Object, _parserBaseMock.Object, _parserBaseMock.Object, 0)
                    .Should().BeTrue();
                _parserBaseMock.VerifyGet(m => m.Type, Times.Exactly(2));
            }

            [Fact]
            public void NoExecutedParentOfSameType_ReturnTrue()
            {
                //the tree find up from will return null to represent the lack of ancestry matching our type
                _treeMock.Setup(m => m.FindUpFrom(It.IsAny<ParserBase>(), It.IsAny<Func<ParserNode, bool>>()))
                    .Returns(default(ParserBase));

                //the redundant flag is set to false
                new RedundancyAttribute().Execute(_treeMock.Object, _parserBaseMock.Object, _parserBaseMock.Object, 0)
                    .Should().BeTrue();
                _treeMock.Verify();
            }

            [Fact]
            public void NoChainDefinedCantBeRedundantWithExecutedParent_ReturnFalse()
            {
                //the tree find up from will return a parser base to represent an ancestry matching our type
                _treeMock.Setup(m => m.FindUpFrom(It.IsAny<ParserBase>(), It.IsAny<Func<ParserNode, bool>>()))
                    .Returns(_parserBaseMock.Object);

                //the redundant flag is set to false
                new RedundancyAttribute().Execute(_treeMock.Object, _parserBaseMock.Object, _parserBaseMock.Object, 0)
                    .Should().BeFalse();
                _treeMock.Verify();
            }

            [Fact]
            public void ChainBreakerFoundAsTheParent_ReturnTrue()
            {
                //the tree find up from will return a parser base to represent an ancestry matching our type
                _treeMock.Setup(m => m.FindUpFrom(It.IsAny<ParserBase>(), It.IsAny<Func<ParserNode, bool>>()))
                    .Returns(_parserBaseMock.Object);

                //the parent parser type should be the same
                _parserBaseMock.SetupGet(m => m.Type).Returns(TokenNames.Undefined);

                //the chain breakers need definition
                new RedundancyAttribute(TokenNames.Undefined).Execute(_treeMock.Object, _parserBaseMock.Object, _parserBaseMock.Object, 0)
                    .Should().BeTrue();
                _treeMock.Verify();
            }

            [Fact]
            public void NoParserBetweenNowAndFirstResult_ReturnFalse()
            {
                //the tree find up from will return a parser base to represent an ancestry matching our type
                _treeMock.Setup(m => m.FindUpFrom(It.IsAny<ParserBase>(), It.IsAny<Func<ParserNode, bool>>()))
                    .Returns(_parserBaseMock.Object);
                //we find nothing on top of the parent to simulate the situation for the test
                _treeMock.Setup(m => m.AggregateUpUntil(It.IsAny<ParserBase>(), It.IsAny<Func<ParserNode, bool>>()))
                    .Returns(default(IEnumerable<ParserBase>));

                //the parent parser type should be different to access the next step
                _parserBaseMock.SetupGet(m => m.Type).Returns(TokenNames.Shield);

                //the chain breakers need definition
                new RedundancyAttribute(TokenNames.Undefined).Execute(_treeMock.Object, _parserBaseMock.Object, _parserBaseMock.Object, 0)
                    .Should().BeFalse();
                _treeMock.Verify();
            }

            [Fact]
            public void EmptyParserBetweenNowAndFirstResult_ReturnTrue()
            {
                //the tree find up from will return a parser base to represent an ancestry matching our type
                _treeMock.Setup(m => m.FindUpFrom(It.IsAny<ParserBase>(), It.IsAny<Func<ParserNode, bool>>()))
                    .Returns(_parserBaseMock.Object);
                //we find nothing on top of the parent to simulate the situation for the test
                _treeMock.Setup(m => m.AggregateUpUntil(It.IsAny<ParserBase>(), It.IsAny<Func<ParserNode, bool>>()))
                    .Returns(new List<ParserBase>());

                //the parent parser type should be different to access the next step
                _parserBaseMock.SetupGet(m => m.Type).Returns(TokenNames.Shield);

                //the chain breakers need definition
                new RedundancyAttribute(TokenNames.Undefined).Execute(_treeMock.Object, _parserBaseMock.Object, _parserBaseMock.Object, 0)
                    .Should().BeTrue();
                _treeMock.Verify();
            }

            [Fact]
            public void HappyPathBreakerFoundInAncestry_ReturnTrue()
            {
                //the tree find up from will return a parser base to represent an ancestry matching our type
                _treeMock.Setup(m => m.FindUpFrom(It.IsAny<ParserBase>(), It.IsAny<Func<ParserNode, bool>>()))
                    .Returns(_parserBaseMock.Object);
                //we find nothing on top of the parent to simulate the situation for the test


                _treeMock.Setup(m => m.AggregateUpUntil(It.IsAny<ParserBase>(), It.IsAny<Func<ParserNode, bool>>()))
                    .Returns(new List<ParserBase> { _parserBaseMock.Object });

                //the parent parser type should be different to access the next step
                _parserBaseMock.SetupSequence(m => m.Type)
                    .Returns(TokenNames.Shield)
                    .Returns(TokenNames.Undefined);//we return the one available in the list of parser between so we return true

                //the chain breakers need definition
                new RedundancyAttribute(TokenNames.Undefined).Execute(_treeMock.Object, _parserBaseMock.Object, _parserBaseMock.Object, 0)
                    .Should().BeTrue();
                _treeMock.Verify();
            }
        }
    }
}
