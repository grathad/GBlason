using System.Linq;
using System.Threading.Tasks;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// The symbol is the most complex element in a blazon. It is defined by the fact that its name is NOT one of the reserved keywords <see cref="Resources"/>
    /// The structure of a symbol is very fluid and the few known key words that surround a symbol might be considered part of the symbol if the name is not in the resources.
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.Symbol"/> := <see cref="TokenNames.SymbolAlteration"/>? <see cref="TokenNames.SymbolName"/>. 
    /// <see cref="TokenNames.SymbolAttitude"/>* 
    /// <see cref="TokenNames.SymbolAttitudeAttribute"/>? <see cref="TokenNames.SharedProperty"/>?  <see cref="TokenNames.Tincture"/>? 
    /// <see cref="TokenNames.SymbolSubPartGroup"/>*
    /// </para>
    /// </summary>
    internal class SymbolParser : ContainerParser
    {
        public SymbolParser(IParserPilot factory = null)
            : base(TokenNames.Symbol, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //IEnumerable<ParsedKeyword> ckw;
            //a potential symbol alteration (if the alteration can be applied to ordinaries as well we need to pull it out at the charge level)
            TryConsumeAndAttachOne(ref origin, TokenNames.SymbolAlteration);
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.SymbolName))
            {
                ErrorMandatoryTokenMissing(TokenNames.SymbolName, origin.Start);
                return null;
            }

            //the main name, attitudes and tincture
            while (TryConsumeAndAttachOne(ref origin, TokenNames.SymbolAttitude)) { }
            TryConsumeAndAttachOne(ref origin, TokenNames.SymbolAttitudeAttribute);

            TryConsumeAndAttachOne(ref origin, TokenNames.SharedProperty);
            //we consume the tincture here, if it ends up being the last element
            //and the parent is capable of refactoring then it will be refactored
            TryConsumeAndAttachOne(ref origin, TokenNames.Tincture);



            //sometime a separator end up there even if it is incorrect, we do support this however
            //we only consume it if there is more to the current symbol, if not we ignore it
            var separator= Parse(origin, TokenNames.Separator);
            var separatorPredecessor = CurrentToken.Children.LastOrDefault();
            origin = separator?.Position ?? origin;
            
            while (origin.Start < ParserPilot.LastPosition)
            {
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.SymbolSubPartGroup))
                {
                    break;
                }
                if (separator == null)
                {
                    continue;
                }
                //we insert the separator since the follow up items are part of this token
                AttachChildAfter(separator.ResultToken, separatorPredecessor);
                ErrorOptionalTokenMissing(TokenNames.Separator, origin.Start);
                separator = null;
            }

            return new TokenResult(CurrentToken, origin);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}