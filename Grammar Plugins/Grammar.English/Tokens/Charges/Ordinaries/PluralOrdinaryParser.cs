using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parse a token that represent an ordinary that can be a multiple instance of itself on the field
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.PluralOrdinary"/> := <see cref="TokenNames.PluralOrdinaryName"/> <see cref="TokenNames.LineVariationDefinition"/>
    /// </para>
    /// </summary>
    internal class PluralOrdinaryParser : ContainerParser
    {
        public PluralOrdinaryParser(IParserPilot factory = null)
            : base(TokenNames.PluralOrdinary, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            var result = TryConsumeOr(ref origin,
                TokenNames.PluralOrdinaryName);
            if (result == null)
            {
                return null;
            }
            AttachChild(result.ResultToken);
            origin = result.Position;
            TryConsumeAndAttachOne(ref origin, TokenNames.LineVariationDefinition);
            return CurrentToken.AsTokenResult(result);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}