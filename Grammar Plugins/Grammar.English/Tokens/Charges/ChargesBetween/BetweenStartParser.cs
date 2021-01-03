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
    /// <see cref="TokenNames.BetweenStart"/> := 
    /// <see cref="TokenNames.Between"/> <see cref="TokenNames.BetweenPossibleFirstGroup"/> <see cref="TokenNames.BetweenPossibleSecondGroup"/>
    /// (<see cref="TokenNames.Between"/> <see cref="TokenNames.BetweenPossibleSecondGroup"/>)*
    /// </para>
    /// </summary>
    /// <example>
    /// <i>a chevron between </i>two cinquefoils in chief <b>and</b> a hunting-horn in base Or
    /// </example>
    internal class BetweenStartParser : ContainerParser
    {
        public BetweenStartParser(IParserPilot factory = null)
            : base(TokenNames.BetweenStart, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            var tempColl = new List<IToken>();
            //the mandatory between keyword
            var btwn = Parse(origin, TokenNames.Between);
            if(btwn?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.Between, origin.Start);
                return null;
            }
            origin = btwn.Position;
            tempColl.Add(btwn.ResultToken);

            var firstGroup = Parse(origin, TokenNames.BetweenPossibleFirstGroup);
            if(firstGroup?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.BetweenPossibleFirstGroup, origin.Start);
                return null;
            }
            origin = firstGroup.Position;
            tempColl.Add(firstGroup.ResultToken);

            var secondGroup = Parse(origin, TokenNames.BetweenPossibleSecondGroup);
            if (secondGroup?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.BetweenPossibleSecondGroup, origin.Start);
                return null;
            }
            origin = secondGroup.Position;
            tempColl.Add(secondGroup.ResultToken);

            //Todo: multiple between (need an example first)

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