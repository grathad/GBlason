using System;
using System.Linq;
using FluentAssertions;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Moq;
using Xunit;

namespace Grammar.PluginBase.Test.Parser
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ParserTreeSpecs
    {
        public class GetFirstOrDefault
        {
            private ParserTree PopulatedTree { get; }

            public GetFirstOrDefault()
            {
                PopulatedTree = new ParserTree();
                var pn = new ParserNode(
                    new Mock<ParserBase>(TokenNames.Shield, default(IParserPilot)).Object,
                    TokenParsingPosition.DefaultStartingPosition);
                PopulatedTree.AddChild(null, pn);
                var child = new ParserNode(
                    new Mock<ParserBase>(TokenNames.Field, default(IParserPilot)).Object,
                    TokenParsingPosition.DefaultStartingPosition);
                PopulatedTree.AddChild(pn.Parser, child);
                pn = child;
                child = new ParserNode(
                    new Mock<ParserBase>(TokenNames.Tincture, default(IParserPilot)).Object,
                    TokenParsingPosition.DefaultStartingPosition);
                PopulatedTree.AddChild(pn.Parser, child);
                pn = child;
                child = new ParserNode(
                    new Mock<ParserBase>(TokenNames.SimpleTincture, default(IParserPilot)).Object,
                    TokenParsingPosition.DefaultStartingPosition);
                PopulatedTree.AddChild(pn.Parser, child);
                pn = child;
                child = new ParserNode(new LeafParser(TokenNames.TinctureColour), TokenParsingPosition.DefaultStartingPosition);
                PopulatedTree.AddChild(pn.Parser, child);
            }

            [Fact]
            public void NullPredicate_Throw()
            {
                var tree = new ParserTree();
                Action test = () => tree.GetFirstOrDefault(null);
                test.Should().Throw<ArgumentNullException>();

                tree.GetFirstOrDefault(n => true).Should().BeNull();
            }

            [Fact]
            public void NullRoot_ReturnNull()
            {
                var tree = new ParserTree();
                tree.GetFirstOrDefault(n => true).Should().BeNull();
            }

            [Fact]
            public void FromPopulatedTree()
            {
                var tree = PopulatedTree;

                var first = tree.GetFirstOrDefault(n => true);
                first.Should().NotBeNull().And.Subject
                    .As<ParserNode>().Should().Be(tree.Root);

                var firstLeaf = tree.GetFirstOrDefault(n => !n.Children.Any());
                firstLeaf.Should().NotBeNull().And.Subject
                    .As<ParserNode>().Children.Should().BeEmpty();
            }
        }

        public class Get
        {
            [Fact]
            public void CallGetFirstOrDefault_ComparingReferences()
            {
                var comparisonResult = false;
                var mock = new Mock<ParserTree> { CallBase = true };
                var parserMock = new Mock<ParserBase>(TokenNames.Undefined, new Mock<IParserPilot>().Object);
                var parserNode = new Mock<ParserNode>(parserMock.Object, TokenParsingPosition.DefaultStartingPosition);
                parserNode.SetupGet(m => m.Parser).Returns(parserMock.Object);
                mock.Setup(m => m.GetFirstOrDefault(It.IsAny<Func<ParserNode, bool>>()))
                    .Callback<Func<ParserNode, bool>>(fun =>
                    {
                        comparisonResult = fun.Invoke(parserNode.Object);
                    });
                mock.Object.Get(parserMock.Object);
                mock.Verify();
                comparisonResult.Should().BeTrue();
            }

        }

        public class AggregateUpUntil
        {
            [Fact]
            public void DescendantNull_ReturnNull()
            {
                new ParserTree().AggregateUpUntil(null, null).Should().BeNull();
            }
            [Fact]
            public void PredicateNull_ReturnNull()
            {
                new ParserTree().AggregateUpUntil(
                    new Mock<ParserBase>(TokenNames.Undefined, default(IParserPilot)).Object,
                    null).Should().BeNull();
            }
            [Fact]
            public void DescendantNotInTree_ReturnNull()
            {
                var itself = new Mock<ParserTree> { CallBase = true };
                itself.Setup(m => m.Get(It.IsAny<ParserBase>())).Returns(default(ParserNode));
                var fp = new Mock<ParserBase>(TokenNames.Undefined, default(IParserPilot)).Object;
                itself.Object.AggregateUpUntil(fp, a => true).Should().BeNull();
                itself.Verify(m => m.Get(fp));
            }

            [Fact]
            public void DescendantNoParent_ReturnEmpty()
            {
                var itself = new Mock<ParserTree> { CallBase = true };
                var fp = new Mock<ParserBase>(TokenNames.Undefined, default(IParserPilot)).Object;
                var fpNode = new Mock<ParserNode>(fp, TokenParsingPosition.DefaultStartingPosition) { CallBase = true };
                fpNode.SetupGet(m => m.Parent).Returns(default(ParserNode));
                itself.Setup(m => m.Get(It.IsAny<ParserBase>())).Returns(fpNode.Object);

                itself.Object.AggregateUpUntil(fp, a => true).Should().BeEmpty();
                itself.Verify(m => m.Get(fp));
                fpNode.VerifyGet(m => m.Parent);
            }

            [Fact]
            public void PredicateAlwaysTrue_ReturnUntilRoot()
            {
                var itself = new Mock<ParserTree> { CallBase = true };
                var fp = new Mock<ParserBase>(TokenNames.Undefined, default(IParserPilot)).Object;
                var fpNode = new Mock<ParserNode>(fp, TokenParsingPosition.DefaultStartingPosition) { CallBase = true };
                var count = 0;
                fpNode.SetupGet(m => m.Parent).Returns(() => count++ < 2 ? fpNode.Object : default(ParserNode));
                itself.Setup(m => m.Get(It.IsAny<ParserBase>())).Returns(fpNode.Object);

                var result = itself.Object.AggregateUpUntil(fp, a => true);
                itself.Verify(m => m.Get(fp));
                fpNode.VerifyGet(m => m.Parent);
                result.Should().HaveCount(2);
                result.Should().Contain(fp);
            }


            [Fact]
            public void HappyPath_ReturnWhenPredicateFalse()
            {
                var itself = new Mock<ParserTree> { CallBase = true };
                var fp = new Mock<ParserBase>(TokenNames.Undefined, default(IParserPilot)).Object;
                var fpNode = new Mock<ParserNode>(fp, TokenParsingPosition.DefaultStartingPosition) { CallBase = true };
                var count = 0;
                fpNode.SetupGet(m => m.Parent).Returns(fpNode.Object);
                itself.Setup(m => m.Get(It.IsAny<ParserBase>())).Returns(fpNode.Object);

                var result = itself.Object.AggregateUpUntil(fp, a => count++ < 2);
                itself.Verify(m => m.Get(fp));
                fpNode.VerifyGet(m => m.Parent);
                result.Should().HaveCount(2);
                result.Should().Contain(fp);
            }
        }

        public class AddChild
        {

            [Fact]
            public void NullInput()
            {
                var tree = new ParserTree();
                //empty input (empty child test)
                tree.AddChild(null, null);
                tree.Root.Should().BeNull();
            }

            [Fact]
            public void OnlyRoot()
            {
                var tree = new ParserTree();
                //setup the root (empty parent)
                var root = new ParserNode(
                    new Mock<ParserBase>(TokenNames.Shield, default(IParserPilot)).Object,
                    TokenParsingPosition.DefaultStartingPosition);
                tree.AddChild(null, root);
                tree.Root.Should().Be(root);
            }

            [Fact]
            public void NoRoot()
            {
                //setup the root (root non existent)
                var root = new ParserNode(
                    new Mock<ParserBase>(TokenNames.Shield, default(IParserPilot)).Object,
                    TokenParsingPosition.DefaultStartingPosition);
                var tree = new ParserTree();
                tree.AddChild(root.Parser, root);
                tree.Root.Should().Be(root);
            }

            [Fact]
            public void NoParent()
            {
                var tree = new ParserTree();
                var root = new ParserNode(
                    new Mock<ParserBase>(TokenNames.Shield, default(IParserPilot)).Object,
                    TokenParsingPosition.DefaultStartingPosition);
                tree.AddChild(null, root);
                //parent not found
                Action test = () =>
                {
                    tree.AddChild(
  new Mock<ParserBase>(TokenNames.Shield, default(IParserPilot)).Object,
  root);
                };
                test.Should().Throw<ArgumentException>().And.Message.Should().Be("parent");
            }
        }
        
        public class FindUpFrom
        {
            private readonly Mock<ParserTree> _mock;
            private readonly Mock<ParserNode> _parserNode;
            private readonly Mock<ParserBase> _parserBase;
            public FindUpFrom()
            {
                _parserBase = new Mock<ParserBase>(TokenNames.Undefined, new Mock<IParserPilot>().Object);
                _parserNode = new Mock<ParserNode>(_parserBase.Object, TokenParsingPosition.DefaultStartingPosition);
                _mock = new Mock<ParserTree> { CallBase = true };
            }

            [Fact]
            public void ParserBaseVersion_CallParserNodeVersion()
            {
                _mock.Setup(m => m.FindUpFrom(It.IsAny<ParserNode>(), It.IsAny<Func<ParserNode, bool>>()))
                    .Verifiable();
                _mock.Setup(m => m.Get(It.IsAny<ParserBase>())).Returns(_parserNode.Object).Verifiable();

                _mock.Object.FindUpFrom(_parserBase.Object, node => true);

                _mock.Verify(m => m.FindUpFrom(_parserNode.Object, It.IsAny<Func<ParserNode, bool>>()), Times.Once);
                _mock.Verify(m => m.Get(_parserBase.Object), Times.Once);
            }

            [Fact]
            public void PredicateNull_Throw()
            {
                Action test = () => _mock.Object.FindUpFrom(_parserNode.Object, null);
                test.Should().Throw<ArgumentNullException>()
                    .Which.ParamName.Should().Be("predicate");
            }

            [Fact]
            public void StartingPointNull_ReturnNull()
            {
                _mock.Object.FindUpFrom(default(ParserNode), node => true).Should().BeNull();
            }

            [Fact]
            public void PredicateTrue_ReturnMatching()
            {
                _parserNode.SetupGet(m => m.Parser).Returns(_parserBase.Object);
                _mock.Object.FindUpFrom(_parserNode.Object, node => true)
                    .Should().Be(_parserBase.Object);
            }

            [Fact]
            public void PredicateFalse_ReturnNull()
            {
                _parserNode.SetupGet(m => m.Parser).Returns(_parserBase.Object);
                _parserNode.SetupSequence(m => m.Parent)
                    .Returns(_parserNode.Object)
                    .Returns(default(ParserNode));
                _mock.Object.FindUpFrom(_parserNode.Object, node => false)
                    .Should().BeNull();
            }
        }
    }
}