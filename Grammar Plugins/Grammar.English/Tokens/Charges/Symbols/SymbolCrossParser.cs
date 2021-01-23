using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// The cross is an honourable, but when modified it behave like a symbol, this parser handle the case when the cross is not an honourable and consume the potential extra properties.
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.SymbolCross"/> := <see cref="TokenNames.Cross"/>. <see cref="TokenNames.LineVariationDefinition"/>? (<see cref="TokenNames.CrossVariation"/> | <see cref="TokenNames.SymbolName"/>)+
    /// </para>
    /// </summary>
    /// <remarks>As the list of variation of the cross are virtually infinite, the variation can either be detected or considered as a symbol name and thus can be anything</remarks>
    internal class SymbolCrossParser : ContainerParser
    {
        public SymbolCrossParser(IParserPilot factory = null)
            : base(TokenNames.SymbolCross, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //the name have to be cross
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.Cross))
            {
                return null;

            }
            //then the potential line variation
            TryConsumeAndAttachOne(ref origin, TokenNames.LineVariationDefinition);
            //then as many cross variations as possible
            ITokenResult result;
            var atLeastOne = false;
            do
            {
                result = TryConsumeOr(ref origin, TokenNames.CrossVariation, TokenNames.SymbolName);
                if (result?.ResultToken == null)
                {
                    continue;
                }
                AttachChild(result.ResultToken);
                origin = result.Position;
                atLeastOne = true;
            } while (result?.ResultToken != null);

            return !atLeastOne
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