using System.Collections.Generic;
using FluentAssertions;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;
using Moq;
using Xunit;

namespace Grammar.PluginBase.Test.Parser
{
    public class LeafParserSpecs
    { 
        public class Constructor
        {
            [Fact]
            public void ConstructLeafParserUndefined()
            {
                var lp = new LeafParser(TokenNames.Undefined);
                lp.CurrentToken.Type.Should().Be(TokenNames.Undefined);
                lp.Resources.Should().BeNull();
            }

            [Fact]
            public void ConstructLeafWithParameters()
            {
                var pilotMock = new Mock<IParserPilot>(MockBehavior.Default);
                var resourceMock = new Mock<PluginBase.Keyword.IResources>(MockBehavior.Default);
                var lp = new LeafParser(
                    TokenNames.Undefined,
                    pilotMock.Object,
                    resourceMock.Object);

                lp.CurrentToken.Type.Should().Be(TokenNames.Undefined);
                lp.Resources.Should().Be(resourceMock.Object);
                lp.ParserPilot.Should().Be(pilotMock.Object);
            }
        }

        //public class TryConsume
        //{

        //    [Fact]
        //    public void TryConsumeNoTokenError()
        //    {
        //        var nullMock = new Mock<IResources>();
        //        nullMock.Setup(f => f.GetTokens(It.IsAny<TokenNames>())).Returns(default(IEnumerable<IEnumerable<string>>));
        //        var lp = new LeafParser(TokenNames.Undefined, resources: nullMock.Object);

        //        var position = TokenParsingPosition.DefaultStartingPosition;

        //        var error = Assert.Raises<ParserError>(
        //            handler => lp.Error += handler,
        //            handler => lp.Error -= handler,
        //            () => lp.TryConsume(ref position));
        //        error.Arguments.Explanation.Should().Be(string.Format(ParserBase.ErrorNoTokenKeywordsMessage,
        //            TokenNames.Undefined));

        //    }

        //    [Fact]
        //    public void TryConsumeFindOneMatch()
        //    {
        //        //prepare the resources with only one match that correspond to what we want to find
        //        const string testWord = "Test";
        //        var resMock = new Mock<IResources>();
        //        resMock.Setup(m => m.GetTokens(TokenNames.Undefined))
        //            .Returns(new List<IEnumerable<string>>
        //            {
        //                new List<string> { testWord }
        //            });

        //        var testPkw = new ParsedKeyword { Value = testWord, Key = testWord };
        //        var pilotMock = new Mock<IParserPilot>();
        //        pilotMock.Setup(p => p.GetRemainingKeywords(It.IsAny<int>()))
        //            //we need 2 to test the case when the expected number of key words does not match the count of tokens
        //            .Returns(new List<ParsedKeyword> { testPkw });

        //        var lptoTest = new LeafParser(
        //            TokenNames.Undefined,
        //            pilotMock.Object,
        //            resources: resMock.Object);
        //        var position = TokenParsingPosition.DefaultStartingPosition;
        //        var resultToken = lptoTest.TryConsume(ref position)?.ResultToken;
        //        resultToken.Should().NotBeNull();
        //        resultToken.Type.Should().Be(TokenNames.Undefined);
        //        resultToken.Parent.Should().BeNull();
        //        resultToken.Should().BeOfType<LeafToken>().And.Subject.As<LeafToken>().OriginalKw.Should()
        //            .HaveCount(1).And.Subject.As<List<ParsedKeyword>>().Should().Contain(testPkw);
        //    }

        //    [Fact]
        //    public void TryConsumeFindNoMatch()
        //    {
        //        //prepare the resources with only one match that correspond to what we want to find
        //        const string testWord = "Test";
        //        var resMock = new Mock<IResources>();
        //        resMock.Setup(m => m.GetTokens(TokenNames.Undefined))
        //            .Returns(new List<IEnumerable<string>>
        //            {
        //                new List<string> { "not valid" }
        //            });

        //        var testPkw = new ParsedKeyword { Value = testWord, Key = testWord };
        //        var pilotMock = new Mock<IParserPilot>();
        //        pilotMock.Setup(p => p.GetRemainingKeywords(It.IsAny<int>()))
        //            .Returns(new List<ParsedKeyword> { testPkw });

        //        var lptoTest = new LeafParser(
        //            TokenNames.Undefined,
        //            pilotMock.Object,
        //            resources: resMock.Object);
        //        var position = TokenParsingPosition.DefaultStartingPosition;
        //        lptoTest.TryConsume(ref position).Should().BeNull();
        //    }
        //}

        public class CreateLeaf
        {
            private class LpMock : LeafParser
            {
                public LpMock(TokenNames type) : base(type)
                {

                }

                new internal LeafToken CreateLeaf(ITokenParsingPosition original, params ParsedKeyword[] keywordsUsed)
                {
                    return CreateLeaf(original, keywordsUsed);
                }
            }

            [Fact]
            public void CreateLeafNullInput()
            {
                var lp = new LpMock(TokenNames.Undefined);
                var result = lp.CreateLeaf(null, null);
                result.Should().NotBeNull().And.
                    Subject.As<LeafToken>().Type.Should().Be(TokenNames.Undefined);
                result.OriginalKw.Should().BeEmpty();
                result.Parent.Should().BeNull();
            }

            [Fact]
            public void CreateLeafValidInput()
            {
                var pkw = new List<ParsedKeyword>
                {
                    new ParsedKeyword(),
                    new ParsedKeyword()
                };
                var lp = new LpMock(TokenNames.Undefined);
                var original = TokenParsingPosition.DefaultStartingPosition;
                var result = lp.CreateLeaf(original, pkw.ToArray());
                result.Should().NotBeNull().And.
                    Subject.As<LeafToken>().Type.Should().Be(TokenNames.Undefined);
                result.OriginalKw.Should().HaveCount(2);
                result.Parent.Should().BeNull();
                original.Start.Should().Be(2);
            }

            [Fact]
            public void CreateLeafEmptyInput()
            {
                var pkw = new List<ParsedKeyword>();
                var lp = new LpMock(TokenNames.Undefined);
                var original = TokenParsingPosition.DefaultStartingPosition;
                var result = lp.CreateLeaf(original, pkw.ToArray());
                result.Should().NotBeNull().And.
                    Subject.As<LeafToken>().Type.Should().Be(TokenNames.Undefined);
                result.OriginalKw.Should().BeEmpty();
                result.Parent.Should().BeNull();
                original.Start.Should().Be(0);
            }
        }
    }
}
