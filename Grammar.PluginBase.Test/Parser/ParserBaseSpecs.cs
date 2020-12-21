using System;
using System.Threading.Tasks;
using FluentAssertions;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;
using Moq;
using Moq.Protected;
using Xunit;

namespace Grammar.PluginBase.Test.Parser
{
    public class ParserBaseSpecs
    {

        internal class ParserWrap : ParserBase
        {
            public ParserWrap(TokenNames type, IParserPilot parserPilot) : base(type, parserPilot)
            {
            }

            public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
            {
                throw new System.NotImplementedException();
            }

            /// <inheritdoc/>
            public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
            {
                throw new NotImplementedException();
            }

            public ITokenResult ParseOverride(ITokenParsingPosition currentPosition, TokenNames token = TokenNames.Shield)
            {
                return Parse(currentPosition, token);
            }

            public ITokenResult ParseOverride(int currentPosition, TokenNames token = TokenNames.Shield)
            {
                return Parse(currentPosition, token);
            }
        }

        public class Constructor
        {
            [Fact]
            public void ConstructParserBaseUndefined()
            {
                var baseMock = new Mock<ParserBase>(
                    TokenNames.Undefined,
                    default(IParserPilot))
                {
                    CallBase = true
                };

                baseMock.Object.ParserPilot.Should().NotBeNull();
                baseMock.Object.Type.Should().Be(TokenNames.Undefined);
            }

            [Fact]
            public void ConstructParserBaseWithParameters()
            {
                var pilotMock = new Mock<IParserPilot>(MockBehavior.Default);
                var pilotInstance = pilotMock.Object;
                var baseMock = new Mock<ParserBase>(
                    TokenNames.Undefined,
                    pilotInstance);

                baseMock.Object.ParserPilot.Should().Be(pilotInstance);
                baseMock.Object.Type.Should().Be(TokenNames.Undefined);
            }
        }

        public class Parse
        {
            private ParserWrap _mock;
            private Mock<IParserPilot> _pilot;
            public Parse()
            {
                _pilot = new Mock<IParserPilot>();
                _pilot.Setup(m => m.Parse(
                    It.IsAny<ITokenParsingPosition>(),
                    It.IsAny<ParserBase>(),
                    It.IsAny<TokenNames>())).Verifiable();
                _mock = new ParserWrap(TokenNames.Undefined, _pilot.Object);
            }

            [Fact]
            public void NullPosition_UseDefault()
            {
                _mock.ParseOverride(default(TokenParsingPosition), TokenNames.Undefined);
                _pilot.Verify(m => m.Parse(
                    It.Is<TokenParsingPosition>(o => o.Start == 0),
                    _mock,
                    TokenNames.Undefined));
            }

            [Fact]
            public void ValidPosition_UseCopy()
            {
                var position = TokenParsingPosition.DefaultStartingPosition;
                position.Start = 8;
                _mock.ParseOverride(position, TokenNames.Undefined);
                _pilot.Verify(m => m.Parse(
                    It.Is<TokenParsingPosition>(o => o.Start == 8 && !ReferenceEquals(o, position)),
                    _mock,
                    TokenNames.Undefined));
            }

            [Fact]
            public void NoToken_UseDefault()
            {
                var position = TokenParsingPosition.DefaultStartingPosition;
                position.Start = 8;
                _mock.ParseOverride(position);
                _pilot.Verify(m => m.Parse(
                    It.Is<TokenParsingPosition>(o => o.Start == 8 && !ReferenceEquals(o, position)),
                    _mock,
                    TokenNames.Shield));
            }

            [Fact]
            public void ParseInt_CallReference()
            {
                _pilot.Setup(m => m.Parse(
                    It.IsAny<TokenParsingPosition>(),
                    It.IsAny<ParserBase>(),
                    It.IsAny<TokenNames>()))
                    .Verifiable();
                _mock.ParseOverride(10, TokenNames.Charge);
                _pilot.Verify(m => m.Parse(
                    It.Is<TokenParsingPosition>(o => o.Start == 10),
                    _mock,
                    TokenNames.Charge));
            }
        }

        public class TryConsume
        {
            [Fact]
            public void CallTheAbstractVersion()
            {
                var mock = new Mock<ParserBase>(TokenNames.Undefined, default(IParserPilot)) { CallBase = true };

                mock.Setup(m => m.TryConsume(ref It.Ref<ITokenParsingPosition>.IsAny))
                    .Returns<TokenParsingPosition>(o => new TokenResult(null, o))
                    .Verifiable();
                mock.Object.TryConsume(10).Position.Start.Should().Be(10);
                mock.Verify();
            }
        }

        public class ErrorMandatoryTokenMissing
        {

        }

        public class ErrorOptionalTokenMissing
        {

        }

        public class ErrorNoTokenKeywords
        {

        }

        public class ErrorNotEnoughChildren
        {

        }
    }
}
