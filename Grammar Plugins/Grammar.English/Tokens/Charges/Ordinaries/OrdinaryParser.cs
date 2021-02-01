using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parse a token that represent an ordinary (high level)
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="Ordinary"/> := <see cref="TokenNames.OrdinaryAlteration"/>? <see cref="TokenNames.SimpleOrdinary"/> | <see cref="TokenNames.MultiOrdinary"/>
    /// </para>
    /// </summary>
    internal class OrdinaryParser : ContainerParser
    {
        public OrdinaryParser(IParserPilot factory = null)
            : base(TokenNames.Ordinary, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //potential ordinary alteration
            TryConsumeAndAttachOne(ref origin, TokenNames.OrdinaryAlteration);
            //we try to consume everything possible and we take the solution that end up consuming the most parsed key words
            //for simplest charge
            var result = TryConsumeOr(ref origin,
                TokenNames.SimpleOrdinary,
                TokenNames.MultiOrdinary);
            if (result == null)
            {
                return null;
            }
            AttachChild(result.ResultToken);
            return CurrentToken.AsTokenResult(result);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}