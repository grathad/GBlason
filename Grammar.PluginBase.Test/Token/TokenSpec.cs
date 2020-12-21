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
    public class TokenSpec
    {
        internal class NotSupported : PluginBase.Token.Token
        {

        }
        public class Constructor
        {
            [Fact]
            public void Intialization()
            {
                var parent = new Mock<IToken>();
                var test = new NotSupported { Type = TokenNames.Undefined, Parent = parent.Object };
                test.Type.Should().Be(TokenNames.Undefined);
                test.Parent.Should().Be(parent.Object);
            }
        }

        public class GetDepth
        {
            [Fact]
            public void TokenTypeNeitherLeafNorContainer_Throws()
            {
                Action test = () => new NotSupported().GetDepth();
                test.Should().Throw<NotSupportedException>()
                    .Which.Message.Should().Be(typeof(NotSupported).FullName);
            }

            [Fact]
            public void Leaf_Return1()
            {
                new LeafToken().GetDepth().Should().Be(1);
            }

            [Fact]
            public void Container_ReturnDepth()
            {
                var container = new ContainerToken();
                //first branch depth should be 2 (container and its leaf)
                container.Children.Add(new LeafToken());
                //second branch depth should be 3 (container and its container and its leaf)
                var secondBranch = new ContainerToken();
                secondBranch.Children.Add(new LeafToken());
                container.Children.Add(secondBranch);
                //third branch depth should be 2 (container and its leaf)
                container.Children.Add(new LeafToken());

                container.GetDepth().Should().Be(3);
                secondBranch.GetDepth().Should().Be(2);
            }

            [Fact]
            public void ContainerEmptyChildren_Return1()
            {
                var container = new ContainerToken();
                container.GetDepth().Should().Be(1);
            }
        }

        public class GetNbLeaves
        {

            [Fact]
            public void TokenTypeNeitherLeafNorContainer_Throws()
            {
                Action test = () => new NotSupported().GetNbLeaves();
                test.Should().Throw<NotSupportedException>()
                    .Which.Message.Should().Be(typeof(NotSupported).FullName);
            }

            [Fact]
            public void Leaf_Return1()
            {
                new LeafToken().GetNbLeaves().Should().Be(1);
            }

            [Fact]
            public void Container_ReturnChildrenSum()
            {
                var container = new ContainerToken();
                var childMock = new Mock<PluginBase.Token.Token> { CallBase = false };
                childMock.SetupSequence(m => m.GetNbLeaves())
                    .Returns(2)
                    .Returns(4)
                    .Returns(3);
                container.Children.AddRange(new[]
                {
                    childMock.Object,
                    childMock.Object,
                    childMock.Object
                });
                container.GetNbLeaves().Should().Be(9);
                childMock.Verify(m => m.GetNbLeaves(), Times.Exactly(3));
            }

            [Fact]
            public void ContainerEmptyChildren_Return0()
            {
                var container = new ContainerToken();
                container.GetNbLeaves().Should().Be(0);
            }
        }

        public class AsTokenResult
        {
            [Fact]
            public void FromPosition_PositionNull_ReturnNull()
            {
                var mock = new Mock<PluginBase.Token.Token> { CallBase = true };
                mock.Object.AsTokenResult(default(TokenParsingPosition)).Should().BeNull();
            }

            [Fact]
            public void FromPosition_PositionValid_ReturnResultWithPosition()
            {
                var mock = new Mock<PluginBase.Token.Token> { CallBase = true };
                var position = new TokenParsingPosition { Start = 10 };
                var result = mock.Object.AsTokenResult(position);
                result.Position.Should().Be(position);
            }

            [Fact]
            public void FromResult_ResultNull_ReturnNull()
            {
                var mock = new Mock<PluginBase.Token.Token> { CallBase = true };
                mock.Object.AsTokenResult(default(TokenResultBase)).Should().BeNull();
            }

            [Fact]
            public void FromResult_ResultValid_ReturnResultWithPosition()
            {
                var mock = new Mock<PluginBase.Token.Token> { CallBase = true };
                var resultToConvert = new Mock<TokenResultBase>(new TokenParsingPosition { Start = 10 });
                resultToConvert.SetupGet(m => m.Position).Returns(new TokenParsingPosition { Start = 10 });
                var result = mock.Object.AsTokenResult(resultToConvert.Object);
                result.Position.Start.Should().Be(10);
            }
        }
    }
}
