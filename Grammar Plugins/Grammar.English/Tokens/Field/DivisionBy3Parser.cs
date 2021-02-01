using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parse tokens that represent a <see cref="DivisionBy3"/>
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="DivisionBy3"/> := 
    /// <see cref="TokenNames.DivisionBy3Name"/>. <see cref="TokenNames.LightSeparator"/>? 
    /// <see cref="TokenNames.LineVariationDefinition"/>? <see cref="TokenNames.SimpleDivisionBy3Field"/>
    /// </para>
    /// </summary>
    internal class DivisionBy3Parser : ContainerParser
    {
        public DivisionBy3Parser(IParserPilot factory = null)
            : base(TokenNames.DivisionBy3, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.DivisionBy3Name)) { return null; }
            TryConsumeAndAttachOne(ref origin, TokenNames.LightSeparator);
            TryConsumeAndAttachOne(ref origin, TokenNames.LineVariationDefinition);
            //either a simple quarter or a positionned halves. This is not a regular OR this is priority based.
            //Because the simple division by 2 will always consume more than the simple division by 2 fields (a shield is always a field + something)
            //but if we match a simple division by 2 we pick it as priority
            return !TryConsumeAndAttachOne(ref origin, TokenNames.SimpleDivisionBy3Field)
                ? null 
                : CurrentToken.AsTokenResult(origin);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}