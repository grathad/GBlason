using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{

    /// <summary>
    /// Parse a list of token and try to generate a <see cref="TokenNames.FieldVariationKnownSemy"/> token
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.FieldVariationKnownSemy"/> := simpleTincture predefinedSemy
    /// <see cref="TokenNames.SimpleTincture"/>. <see cref="TokenNames.PredefinedSemy"/>.
    /// </para>
    /// </summary>
    internal class FieldVariationKnownSemyParser : ContainerParser
    {
        public FieldVariationKnownSemyParser(IParserPilot factory = null)
            : base(TokenNames.FieldVariationKnownSemy, factory)
        {
        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.Tincture)) { return null; }
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.PredefinedSemy)) { return null; }
            return CurrentToken.AsTokenResult(origin);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}