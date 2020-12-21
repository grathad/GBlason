using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parser for Token representing a symbol state
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.SymbolState"/> := <see cref="TokenNames.SymbolStateDeterminer"/>. <see cref="TokenNames.SymbolName"/>.
    /// </para>
    /// </summary>
    internal class SymbolStateParser : ContainerParser
    {
        public SymbolStateParser(IParserPilot factory = null) 
            : base(TokenNames.SymbolState, factory)
        {
        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //throw new NotImplementedException();
            return null;
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }

        //internal override Token Parse(List<ParsedKeyword> kw)
        //{
        //    if (!CheckParseInput(kw))
        //    {
        //        return null;
        //    }
        //    //the grammar
        //    //SymbolState := SymbolStateDeterminer SymbolName

        //    AttachChild(GetParser(TokenNames.SymbolStateDeterminer).Parse(kw));
        //    AttachChild(GetParser(TokenNames.SymbolName).Parse(kw));

        //    return CurrentToken;
        //}

    }
}