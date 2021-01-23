using System.Collections.Generic;
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
    /// Thus most complex symbols blazon will end up in error through grammar parsers. This version of the parser focus on simple definition that covers most of the cases
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.Symbol"/> := <see cref="TokenNames.SymbolAlteration"/>? <see cref="TokenNames.SymbolName"/>. 
    /// <see cref="TokenNames.SymbolAttitude"/>* 
    /// <see cref="TokenNames.SymbolAttitudeAttribute"/>? <see cref="TokenNames.SharedProperty"/>? (<see cref="TokenNames.Tincture"/> | <see cref="TokenNames.FieldVariation"/>)
    /// <see cref="TokenNames.SymbolSubPartList"/>*
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

            var tempColl = new List<IToken>();

            var symbolAlteration = Parse(origin, TokenNames.SymbolAlteration);
            if (symbolAlteration?.ResultToken != null)
            {
                origin = symbolAlteration.Position;
                tempColl.Add(symbolAlteration.ResultToken);
            }

            //the main name, attitudes and tincture
            var symbolName = Parse(origin, TokenNames.SymbolName);
            if (symbolName?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.SymbolName, origin.Start);
                return null;
            }
            origin = symbolName.Position;
            tempColl.Add(symbolName.ResultToken);

            int i = 0;
            ITokenResult symbolAttitude;
            while (i < Configurations.GrammarMaxLoop)
            {
                i++;
                symbolAttitude = Parse(origin, TokenNames.SymbolAttitude);
                if (symbolAttitude?.ResultToken == null)
                {
                    break;
                }
                origin = symbolAttitude.Position;
                tempColl.Add(symbolAttitude.ResultToken);
            }
            var symbolAttitudeAttribute = Parse(origin, TokenNames.SymbolAttitudeAttribute);
            if (symbolAttitudeAttribute?.ResultToken != null)
            {
                origin = symbolAttitudeAttribute.Position;
                tempColl.Add(symbolAttitudeAttribute.ResultToken);
            }
            var sharedProperty = Parse(origin, TokenNames.SharedProperty);
            if (sharedProperty?.ResultToken != null)
            {
                origin = sharedProperty.Position;
                tempColl.Add(sharedProperty.ResultToken);
            }

            //The symbol handle its own tincture, for more complex collection with shared tinctures (like "all") a different grammar handle those
            var filling = TryConsumeOr(ref origin, new[] { TokenNames.Tincture, TokenNames.FieldVariation });
            if (filling?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.Tincture, origin.Start);
                return null;
            }
            //origin = filling.Position; since I use the tryconsume the origin is altered within
            tempColl.Add(filling.ResultToken);

            //sometime a separator end up there even if it is incorrect, we do support this however
            //we only consume it if there is more to the current symbol, if not we ignore it
            var separator = Parse(origin, TokenNames.Separator);
            var tempPos = separator?.Position ?? origin;

            //symbol subpart list potential
            i = 0;
            while (tempPos.Start < ParserPilot.LastPosition && i < Configurations.GrammarMaxLoop)
            {
                i++;
                var subpartList = Parse(tempPos, TokenNames.SymbolSubPartList);
                if (subpartList?.ResultToken == null)
                {
                    break;
                }
                if (separator != null)
                {
                    //we insert the separator since the follow up items are part of this token (it would be the same as splitting the grammar between separator (mandatory) with subpartlist or subpartlist)
                    tempColl.Add(separator.ResultToken);
                    separator = null;
                }
                tempPos = subpartList.Position;
                //we only update the origin with the current position here IF there is a subpart list, no need to change the position while consuming the separator
                //if the separator is not meant to be consumed.
                origin = tempPos;
                tempColl.Add(subpartList.ResultToken);
            }

            AttachChildren(tempColl);
            return new TokenResult(CurrentToken, origin);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}