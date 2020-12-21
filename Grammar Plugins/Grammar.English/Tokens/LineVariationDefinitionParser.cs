using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parse tokens that represent a variation of line construct <see cref="TokenNames.LineVariationDefinition"/>.
    /// Composed of <see cref="TokenNames.LineVariation"/>
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.LineVariationDefinition"/> := 
    /// <see cref="TokenNames.LineVariation"/>. (<see cref="TokenNames.Counter"/>. <see cref="TokenNames.LineVariation"/>)?
    /// </para>
    /// </summary>
    internal class LineVariationDefinitionParser : ContainerParser
    {
        public LineVariationDefinitionParser(IParserPilot factory = null)
            : base(TokenNames.LineVariationDefinition, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //mandatory line variation name
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.LineVariation)) { return null; }
            //potential counter. If there is a counter then it have to be followed by a line of variation to be able to consume both
            var counter = Parse(origin, TokenNames.Counter);
            if (counter != null)
            {
                var lastLine = Parse(counter.Position, TokenNames.LineVariation);
                if (lastLine != null)
                {
                    origin = lastLine.Position;
                    AttachChild(counter.ResultToken);
                    AttachChild(lastLine.ResultToken);
                }
            }

            return CurrentToken.AsTokenResult(origin);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}