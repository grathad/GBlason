using System.Globalization;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parse a number meant to be the 1st in the division rule
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="FirstDivisionNumberParser"/> := <see cref="TokenNames.DigitOne"/>
    /// </para>
    /// </summary>
    internal class FirstDivisionNumberParser : LeafParser
    {
        public FirstDivisionNumberParser(IParserPilot factory = null, IResources resources = null)
            : base(TokenNames.FirstDivisionNumber, factory, resources)
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
            return number == 1
                ? CreateLeaf(origin, kw)
                : null;
        }
    }
}