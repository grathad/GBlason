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
    public class TokenParsingPositionSpec
    {
        public class CopyConstructor
        {
            [Fact]
            public void CopyDifferentInstance()
            {
                var origin = new Mock<ITokenParsingPosition>();
                origin.SetupGet(m => m.Start).Returns(10);
                var copy = new TokenParsingPosition(origin.Object);
                copy.Should().NotBeSameAs(origin.Object);
                copy.Start.Should().Be(10);
                origin.VerifyGet(m => m.Start, Times.Once);
            }
        }

        public class DefaultStartingPosition
        {
            [Fact]
            public void DefaultStartingPositionIs0()
            {
                TokenParsingPosition.DefaultStartingPosition.Start.Should().Be(0);
            }

            [Fact]
            public void DefaultStartingPosition_DifferentInstances_SameValues()
            {
                var first = TokenParsingPosition.DefaultStartingPosition;
                var second = TokenParsingPosition.DefaultStartingPosition;
                first.Should().NotBeSameAs(second);
                first.Should().Be(second);
            }
        }

        public class Copy
        {
            [Fact]
            public void Copy_ReturnDifferentInstance()
            {
                var origin = new TokenParsingPosition { Start = 10 };
                var copy = origin.Copy();
                origin.Should().NotBeSameAs(copy);
                origin.Should().Be(copy);
            }
        }

        public class EqualityOperator
        {
            [Fact]
            public void BothSideNull_ReturnTrue()
            {
                var defaultPosition = default(TokenParsingPosition);
                var secondPosition = default(TokenParsingPosition);
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                (defaultPosition == secondPosition).Should().BeTrue();
            }

            [Fact]
            public void OnlyOneSideNull_ReturnFalse()
            {
                var defaultPosition = default(TokenParsingPosition);
                var secondPosition = new TokenParsingPosition();
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                (defaultPosition == secondPosition).Should().BeFalse();
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                (secondPosition == defaultPosition).Should().BeFalse();
            }

            [Fact]
            public void SameNonNullReference_ReturnTrue()
            {
                var secondPosition = new TokenParsingPosition();
                var same = secondPosition;
                (same == secondPosition).Should().BeTrue();
                (secondPosition == same).Should().BeTrue();
            }

            [Fact]
            public void DifferentReferenceSameStart_ReturnTrue()
            {
                var secondPosition = new TokenParsingPosition { Start = 2 };
                var same = new TokenParsingPosition { Start = 2 };
                (same == secondPosition).Should().BeTrue();
                (secondPosition == same).Should().BeTrue();
            }

            [Fact]
            public void DifferentReferenceDifferentStart_ReturnFalse()
            {
                var secondPosition = new TokenParsingPosition { Start = 2 };
                var same = new TokenParsingPosition { Start = 3 };
                (same == secondPosition).Should().BeFalse();
                (secondPosition == same).Should().BeFalse();
            }
        }

        public class DifferenceOperator
        {
            [Fact]
            public void DifferentReferenceSameStart_ReturnFalse()
            {
                var secondPosition = new TokenParsingPosition { Start = 2 };
                var same = new TokenParsingPosition { Start = 2 };
                (same != secondPosition).Should().BeFalse();
                (secondPosition != same).Should().BeFalse();
            }

            [Fact]
            public void DifferentReferenceDifferentStart_ReturnTrue()
            {
                var secondPosition = new TokenParsingPosition { Start = 2 };
                var same = new TokenParsingPosition { Start = 3 };
                (same != secondPosition).Should().BeTrue();
                (secondPosition != same).Should().BeTrue();
            }

        }

        public new class Equals
        {
            [Fact]
            public void NullComparison_ReturnFalse()
            {
                var position = new TokenParsingPosition();
                position.Equals(null).Should().BeFalse();
                position.Equals((object)null).Should().BeFalse();
            }

            [Fact]
            public void SameReference_ReturnTrue()
            {
                var position = new TokenParsingPosition();
                var same = position;
                position.Equals(same).Should().BeTrue();
                position.Equals((object)same).Should().BeTrue();
            }

            [Fact]
            public void DifferentReference_SameStart_ReturnTrue()
            {
                var position = new TokenParsingPosition { Start = 10 };
                var same = new TokenParsingPosition { Start = 10 };
                position.Equals(same).Should().BeTrue();
                position.Equals((object)same).Should().BeTrue();
            }

            [Fact]
            public void DifferentReference_DifferentStart_ReturnFalse()
            {
                var position = new TokenParsingPosition { Start = 15 };
                var same = new TokenParsingPosition { Start = 8 };
                position.Equals(same).Should().BeFalse();
                position.Equals((object)same).Should().BeFalse();
            }
        }

        public new class GetHashCode
        {
            [Fact]
            public void ReturnStart()
            {
                new TokenParsingPosition { Start = 10 }.GetHashCode().Should().Be(10);
                new TokenParsingPosition { Start = 120 }.GetHashCode().Should().Be(120);
            }
        }
    }
}
