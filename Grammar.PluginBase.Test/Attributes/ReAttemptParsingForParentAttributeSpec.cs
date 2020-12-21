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
    public class ReAttemptParsingForParentAttributeSpec
    {
        public class Constructor
        {
            [Fact]
            public void Default()
            {
                new ReAttemptParsingForParentAttribute().Parents.Should().BeEmpty();
            }

            [Fact]
            public void WithInitialValue()
            {
                new ReAttemptParsingForParentAttribute(TokenNames.Undefined, TokenNames.Undefined)
                    .Parents.Should().HaveCount(2);
            }
        }

        public class Execute
        {
            [Fact]
            public void NullParserTree_ReturnTrue()
            {
                new ReAttemptParsingForParentAttribute()
                    .Execute(null, new Mock<ParserNode>(default(ParserBase), default(TokenParsingPosition)).Object)
                    .Should().BeTrue();
            }

            [Fact]
            public void NullNode_ReturnTrue()
            {
                new ReAttemptParsingForParentAttribute().Execute(new Mock<IParserTree>().Object, null)
                    .Should().BeTrue();
            }

            [Fact]
            public void ParentEmpty_ReturnTrue()
            {
                new ReAttemptParsingForParentAttribute()
                    .Execute(new Mock<IParserTree>().Object, new Mock<ParserNode>(default(ParserBase), default(TokenParsingPosition)).Object)
                    .Should().BeTrue();
            }

            [Fact]
            public void ParentNullOrParentParserNull_ReturnTrue()
            {
                //parent null
                var parserNodeMock = new Mock<ParserNode>(default(ParserBase), default(TokenParsingPosition));
                parserNodeMock.SetupGet(n => n.Parent).Returns(default(ParserNode));
                new ReAttemptParsingForParentAttribute(TokenNames.Undefined)
                    .Execute(new Mock<IParserTree>().Object, parserNodeMock.Object)
                    .Should().BeTrue();
                //parent parser null
                var nodeNoParser = new Mock<ParserNode>(default(ParserBase), default(TokenParsingPosition));
                nodeNoParser.SetupGet(n => n.Parser).Returns(default(ParserBase));
                parserNodeMock.SetupGet(n => n.Parent).Returns(nodeNoParser.Object);
                new ReAttemptParsingForParentAttribute(TokenNames.Undefined)
                    .Execute(new Mock<IParserTree>().Object, parserNodeMock.Object)
                    .Should().BeTrue();
            }

            /// <summary>
            /// Happy path
            /// </summary>
            /// <code>
            /// <![CDATA[
            /// if (parent.Parser != null && Parents.Any(p => p == parent.Parser.Type))
            /// {
            ///    return false;
            /// }
            /// ]]>
            /// </code>
            [Fact]
            public void NodeParentInTheList_ReturnFalse()
            {
                //parser mock
                var parserMockUndefined = new Mock<ParserBase>(TokenNames.Undefined, default(IParserPilot));
                parserMockUndefined.SetupGet(m => m.Type).Returns(TokenNames.Undefined);

                var parserMockShield = new Mock<ParserBase>(TokenNames.Undefined, default(IParserPilot));
                parserMockShield.SetupGet(m => m.Type).Returns(TokenNames.Shield);

                //parser nodes
                var parserNodeMock = new Mock<ParserNode>(default(ParserBase), default(TokenParsingPosition));
                //same, we handle the parent as always being itself, so the loop only stop when a match is found
                parserNodeMock.SetupGet(m => m.Parent).Returns(parserNodeMock.Object);
                parserNodeMock.SetupSequence(m => m.Parser)
                    .Returns(parserMockUndefined.Object) //if (nodeFromMemory.Parent?.Parser == null)
                    .Returns(parserMockUndefined.Object) //if (parent.Parser != null
                    .Returns(parserMockUndefined.Object) //Parents.Any(p => p == parent.Parser.Type)
                    .Returns(parserMockShield.Object) //if (parent.Parser != null
                    .Returns(parserMockShield.Object); //Parents.Any(p => p == parent.Parser.Type)

                //we expect to stop when we meet the shield, thus at the second call to get the parser type
                new ReAttemptParsingForParentAttribute(TokenNames.Shield)
                    .Execute(new Mock<IParserTree>().Object, parserNodeMock.Object)
                    .Should().BeFalse();
                parserMockUndefined.Verify();
                parserMockShield.Verify();
                parserNodeMock.VerifyGet(node => node.Parent, Times.Exactly(3));
                parserNodeMock.VerifyGet(node => node.Parser, Times.Exactly(6));
            }

            [Fact]
            public void NodeParentNotInTheList_ReturnTrue()
            {
                //we need to find that the parent is the root and no parser type matched the list
                //parser mock
                var parserMockUndefined = new Mock<ParserBase>(TokenNames.Undefined, default(IParserPilot));
                parserMockUndefined.SetupGet(m => m.Type).Returns(TokenNames.Undefined);

                //parser nodes
                var parserNodeMock = new Mock<ParserNode>(default(ParserBase), default(TokenParsingPosition));

                parserNodeMock.SetupGet(m => m.Parent).Returns(parserNodeMock.Object);
                parserNodeMock.SetupGet(m => m.Parser).Returns(parserMockUndefined.Object);

                var parserTreeMock = new Mock<IParserTree>();
                parserTreeMock.SetupGet(m => m.Root).Returns(parserNodeMock.Object);

                //we expect to stop when we meet the root
                new ReAttemptParsingForParentAttribute(TokenNames.Shield)
                    .Execute(parserTreeMock.Object, parserNodeMock.Object)
                    .Should().BeTrue();
                parserMockUndefined.Verify();
                parserNodeMock.VerifyGet(node => node.Parent, Times.Exactly(2));
                parserNodeMock.VerifyGet(node => node.Parser, Times.Exactly(4));
                parserTreeMock.VerifyGet(tree => tree.Root, Times.Once);
            }
        }
    }
}
