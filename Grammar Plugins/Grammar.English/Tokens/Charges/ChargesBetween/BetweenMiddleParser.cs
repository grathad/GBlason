using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// This represent multiple charges that are listed together, and optionally share a property
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.BetweenMiddle"/> := 
    /// <see cref="TokenNames.BetweenInsideGroup"/>
    /// <see cref="TokenNames.LightSeparator"/>? <see cref="TokenNames.Between"/> <see cref="TokenNames.BetweenSurroundingGroup"/> 
    /// (<see cref="TokenNames.Between"/> <see cref="TokenNames.BetweenSurroundingGroup"/>)* 
    /// </para>
    /// </summary>
    /// <example>
    /// OR, A CROSS BETWEEN FOUR KEYS GULES
    /// </example>
    internal class BetweenMiddleParser : ContainerParser
    {
        public BetweenMiddleParser(IParserPilot factory = null)
            : base(TokenNames.BetweenMiddle, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            var tempColl = new List<IToken>();
            //the mandatory between keyword
            var firstGroup = Parse(origin, TokenNames.BetweenInsideGroup);
            if (firstGroup?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.BetweenInsideGroup, origin.Start);
                return null;
            }
            origin = firstGroup.Position;
            tempColl.Add(firstGroup.ResultToken);

            //the optional light separator
            var ls = Parse(origin, TokenNames.LightSeparator);
            if (ls?.ResultToken != null)
            {
                origin = ls.Position;
                tempColl.Add(ls.ResultToken);
            }

            var btwn = Parse(origin, TokenNames.Between);
            if (btwn?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.Between, origin.Start);
                return null;
            }
            origin = btwn.Position;
            tempColl.Add(btwn.ResultToken);

            var secondGroup = Parse(origin, TokenNames.BetweenSurroundingGroup);
            if (secondGroup?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.BetweenSurroundingGroup, origin.Start);
                return null;
            }
            origin = secondGroup.Position;
            tempColl.Add(secondGroup.ResultToken);

            //Todo: multiple between (need an example first)
            //this one is wrong, since the couple closes are just an extension of the chevron: GULES; A CHEVRON ERMINE, BETWEEN TWO COUPLE CLOSES OR, BETWEEN THREE ESCALLOPS ERMINE

            //int i = 0;
            //while (i < Configurations.GrammarMaxLoop)
            //{
                
            //    btwn = Parse(origin, TokenNames.Between);
            //    if (btwn?.ResultToken == null)
            //    {
            //        break;
            //    }
            //    secondGroup = Parse(btwn.Position, TokenNames.BetweenSurroundingGroup);
            //    if (secondGroup?.ResultToken == null)
            //    {
            //        break;
            //    }
            //    tempColl.AddRange(new[] { btwn.ResultToken, secondGroup.ResultToken });
            //    origin = secondGroup.Position;
            //    i++;
            //}

            //we found our matching grammar, the token return positively
            AttachChildren(tempColl);
            return CurrentToken.AsTokenResult(origin);
        }


        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}