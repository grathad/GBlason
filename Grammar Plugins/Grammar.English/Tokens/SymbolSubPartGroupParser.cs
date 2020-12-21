using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parser for tokens that represent a <see cref="TokenNames.SymbolSubPartGroup"/>
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.SymbolSubPartGroup"/> :=  
    /// (<see cref="TokenNames.SymbolSubPart"/>. |<br/>
    /// <see cref="TokenNames.SymbolSubPart"/>. 
    /// (<see cref="TokenNames.LightSeparator"/>. <see cref="TokenNames.SymbolSubPart"/>)* <see cref="TokenNames.And"/>
    /// <see cref="TokenNames.SymbolSubPart"/>.)
    /// </para>
    /// </summary>
    /// <remarks>The tincture is potentially the responsibility of the parent, since multiple subpart can share the same, but of course can belong to the children if they have different ones</remarks>
    internal class SymbolSubPartGroupParser : ContainerParser
    {
        public SymbolSubPartGroupParser(IParserPilot factory = null)
            : base(TokenNames.SymbolSubPartGroup, factory)
        {
        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.SymbolSubPart)) { return null; }

            //then we consume as many light seprator followed by a symbol sub part as we can
            while (origin.Start < ParserPilot.LastPosition)
            {
                //trying the coma
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.LightSeparator))
                {
                    //no match we bail out
                    break;
                }
                //it should be followed by a symbol sub part
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.SymbolSubPart)) { break; }
            }
            //then we try to consume the and sub part to complete
            var pand = Parse(origin, TokenNames.And);

            if (pand != null)
            {
                var p = Parse(pand.Position, TokenNames.SymbolSubPart);
                if (p != null)
                {
                    AttachChild(pand.ResultToken);
                    AttachChild(p.ResultToken);
                    origin = p.Position;
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