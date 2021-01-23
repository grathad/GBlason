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
    /// <see cref="TokenNames.SurmountedSingle"/> := 
    /// <see cref="TokenNames.SurmountedPossibleFirstSingleGroup"/> <see cref="TokenNames.Surmounted"/> <see cref="TokenNames.SurmountedPossibleSecondGroup"/>
    /// </para>
    /// </summary>
    /// <example>
    /// <i>a chevron between </i>two cinquefoils in chief <b>and</b> a hunting-horn in base Or
    /// </example>
    internal class SurmountedSingleParser : ContainerParser
    {
        public SurmountedSingleParser(IParserPilot factory = null)
            : base(TokenNames.SurmountedSingle, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            if (!Exist(origin.Start, TokenNames.Surmounted))
            {
                ErrorMandatoryTokenMissing(TokenNames.Surmounted, origin.Start);
                return null;
            }

            var tempColl = new List<IToken>();
            var firstGroup = Parse(origin, TokenNames.SurmountedPossibleFirstSingleGroup);
            if (firstGroup?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.SurmountedPossibleFirstSingleGroup, origin.Start);
                return null;
            }
            tempColl.Add(firstGroup.ResultToken);
            origin = firstGroup.Position;

            var surmounted = Parse(origin, TokenNames.Surmounted);
            if (surmounted?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.Surmounted, origin.Start);
                return null;
            }
            tempColl.Add(surmounted.ResultToken);
            origin = surmounted.Position;

            var secondGroup = Parse(origin, TokenNames.SurmountedPossibleSecondGroup);
            if (secondGroup?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.SurmountedPossibleSecondGroup, origin.Start);
                return null;
            }
            tempColl.Add(secondGroup.ResultToken);
            origin = secondGroup.Position;

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