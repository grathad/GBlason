using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parse a token that represent an ordinary that is alone on the field, in term of grammar ALL ordinaries can be defined alone (I think) thus this includes all the plural ordinary names as well
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.SingleOrdinary"/> := (<see cref="TokenNames.SingleOrdinaryName"/> | <see cref="TokenNames.PluralOrdinaryName"/>) <see cref="TokenNames.LineVariationDefinition"/>
    /// </para>
    /// </summary>
    /// <remarks>The ownership of the article for singularity is out, so this grammar rule, on itself, can detect any ordinary, no matter the determiner. 
    /// The <see cref="PluralOrdinaryParser"/> on the other hand is a subset that only accept ordinaries that can be made plural
    /// The line variation is at that level because I do not think the current grammar (legacy) is correct, and will likely need to evolve later, thus the lower level the easier to evolve later
    /// </remarks>
    internal class SingleOrdinaryParser : ContainerParser
    {
        public SingleOrdinaryParser(IParserPilot factory = null)
            : base(TokenNames.SingleOrdinary, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            var result = TryConsumeOr(ref origin,
                TokenNames.SingleOrdinaryName,
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