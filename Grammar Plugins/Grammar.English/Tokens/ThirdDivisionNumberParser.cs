﻿using System.Globalization;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    internal class ThirdDivisionNumberParser : LeafParser
    {
        public ThirdDivisionNumberParser(IParserPilot factory = null)
            : base(TokenNames.ThirdDivisionNumber, factory)
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
            return number == 3 
                ? CreateLeaf(origin, kw)
                : null;
        }
    }
}