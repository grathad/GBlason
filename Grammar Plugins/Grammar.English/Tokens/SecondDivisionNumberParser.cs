using System.Globalization;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    internal class SecondDivisionNumberParser : LeafParser
    {
        public SecondDivisionNumberParser(IParserPilot factory = null, IResources resources = null)
            : base(TokenNames.SecondDivisionNumber, factory, resources)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            var result = base.TryConsume(ref origin);
            if (result != null)
            {
                return result;
            }
            //potential number
            var kw = ParserPilot.GetKeyword(origin.Start);
            if (!int.TryParse(kw.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number))
            {
                return null;
            }
            return number == 2 
                ? CreateLeaf(origin, kw)
                : null;
        }
    }
}