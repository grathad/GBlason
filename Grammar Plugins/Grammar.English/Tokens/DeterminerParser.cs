using System.Linq;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    internal class DeterminerParser : LeafParser
    {
        public DeterminerParser(IParserPilot factory = null, IResources resources = null)
            : base(TokenNames.Determiner, factory, resources)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            var parsedKeywords = ParserPilot.GetRemainingKeywords(origin.Start);
            var number = parsedKeywords.First();
            if (!int.TryParse(number.Value, out var _))
            {
                return base.TryConsume(ref origin);
            }

            return CreateLeaf(origin, parsedKeywords.First());
        }
    }
}