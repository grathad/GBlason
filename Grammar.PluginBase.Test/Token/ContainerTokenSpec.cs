using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using FluentAssertions;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;
using Moq;
using Xunit;

namespace Grammar.PluginBase.Test.Token
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ContainerTokenSpec
    {
        public class Constructor
        {
            [Fact]
            public void Initialization()
            {
                var test = new ContainerToken();
                test.Children.Should().NotBeNull();
                test.Children.Should().BeEmpty();
            }
        }

        public class InsertChildren
        {
            private Mock<ContainerToken> _mock;

            public InsertChildren()
            {
                _mock = new Mock<ContainerToken> { CallBase = true };
            }

            [Fact]
            public void ChildrenInvalid_ReturnNull()
            {
                _mock.SetupGet(m => m.Children).Returns(default(List<IToken>));
                _mock.Object.InsertChildren(null, 0).Should().BeNull();

                _mock.VerifyGet(m => m.Children, Times.Exactly(1));
            }

            [Fact]
            public void AttemptingToInsertNull_Throw()
            {
                _mock.SetupGet(m => m.Children).Returns(new List<IToken>());
                Action test = () => _mock.Object.InsertChildren(null, 0);
                test.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("toAdd");
                _mock.VerifyGet(m => m.Children, Times.Exactly(1));
            }

            [Fact]
            public void ChildToInsertNull_Throw()
            {
                Action test = () => _mock.Object.InsertChildren(null, 0);
                test.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void PositionToInsertOutOfRange_Throw()
            {
                Action test = () => _mock.Object.InsertChildren(new List<PluginBase.Token.Token>(), -1);
                test.Should().Throw<ArgumentOutOfRangeException>();

                _mock.SetupGet(m => m.Children).Returns(new List<IToken>());
                test = () => _mock.Object.InsertChildren(new List<PluginBase.Token.Token>(), 10);
                test.Should().Throw<ArgumentOutOfRangeException>();
            }

            [Fact]
            public void ChildToInsertEmpty_ReturnEmpty()
            {
                _mock.Object.InsertChildren(new List<PluginBase.Token.Token>(), 0).Should().BeEmpty();
            }

            [Fact]
            public void ChildToInsertValid_ChildrenUpdated()
            {
                var token = new Mock<PluginBase.Token.Token>().Object;
                var result = new ContainerToken().InsertChildren(new List<PluginBase.Token.Token> { token, token }, 0).ToList();
                result.Should().HaveCount(2);
                result.Should().ContainInOrder(token, token);
            }
        }

        public new class ToString
        {
            [Fact]
            public void ReturnTypeName()
            {
                var test = new ContainerToken { Type = TokenNames.Undefined };
                test.ToString().Should().Be(TokenNames.Undefined.ToString());
            }
        }
    }
}
