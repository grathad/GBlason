using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Represent a field variation filing a field
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="FieldVariation"/> := <see cref="FieldVariation2Tinctures"/> | <see cref="FieldVariationSemy"/> | <see cref="TokenNames.FieldVariationKnownSemy"/>
    /// </para>
    /// </summary>
    internal class FieldVariationParser : ContainerParser
    {
        public FieldVariationParser(IParserPilot factory = null)
            : base(TokenNames.FieldVariation, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //we try to consume everything possible and we take the solution that end up consuming the most parsed key words
            //for simplest charge
            var result = TryConsumeOr(ref origin,
                TokenNames.FieldVariation2Tinctures,
                TokenNames.FieldVariationSemy,
                TokenNames.FieldVariationKnownSemy);
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