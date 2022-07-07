using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parse tokens that represent a simple ordinary
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="Ordinary"/> := 
    /// (<see cref="TokenNames.OrdinaryHonourable"/> | <see cref="TokenNames.OrdinarySubordinary"/> | <see cref="TokenNames.OrdinaryDiminutive"/>) 
    /// <see cref="TokenNames.LineVariationDefinition"/>?
    /// </para>
    /// </summary>
    internal class SimpleOrdinaryParser : ContainerParser
    {
        public SimpleOrdinaryParser(IParserPilot factory = null)
            : base(TokenNames.SimpleOrdinary, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            var result = TryConsumeOr(ref origin,
                TokenNames.OrdinaryHonourable,
                TokenNames.OrdinarySubordinary,
                TokenNames.OrdinaryDiminutive);
            if (result == null)
            {
                return null;
            }
            
            AttachChild(result.ResultToken);

            //then we try to consume the linevariation
            TryConsumeAndAttachOne(ref origin, TokenNames.LineVariationDefinition);

            return CurrentToken.AsTokenResult(origin);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}