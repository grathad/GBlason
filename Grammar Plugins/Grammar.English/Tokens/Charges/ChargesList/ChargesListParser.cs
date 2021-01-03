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
    /// <see cref="TokenNames.ChargesList"/> := 
    /// <see cref="TokenNames.AndPossibleGroup"/> ((<see cref="TokenNames.Separator"/> <see cref="TokenNames.AndPossibleGroup"/>)* <see cref="TokenNames.And"/> <see cref="TokenNames.AndPossibleGroup"/>).
    /// </para>
    /// </summary>
    /// <example>
    /// <i>a chevron between </i>two cinquefoils in chief <b>and</b> a hunting-horn in base Or
    /// </example>
    internal class ChargesListParser : ContainerParser
    {
        public ChargesListParser(IParserPilot factory = null)
            : base(TokenNames.ChargesList, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //optimization, we do know there is a mandatory "AND" keyword we expect (as of the current grammar)
            //if there are no And available in the following token, then it is not worth trying to parse a and group
            if(!Exist(origin.Start, TokenNames.And))
            {
                //ErrorMandatoryTokenMissing(TokenNames.And, origin.Start);
                return null;
            }
            var tempColl = new List<IToken>();
            var and = Parse(origin, TokenNames.AndPossibleGroup);
            if (and?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.AndPossibleGroup, origin.Start);
                return null;
            }
            tempColl.Add(and.ResultToken);

            origin = and.Position;

            while (true)
            {
                var separator = Parse(origin, TokenNames.Separator);
                if(separator?.ResultToken == null)
                {
                    break;
                }
                var newand = Parse(separator.Position, TokenNames.AndPossibleGroup);
                if(newand?.ResultToken == null)
                {
                    break;
                }
                tempColl.AddRange(new[] { separator.ResultToken, newand.ResultToken });
                origin = newand.Position;
            }

            var finalAnd = Parse(origin, TokenNames.And);
            if(finalAnd?.ResultToken == null)
            {
                return null;
            }
            var finalGroup = Parse(and.Position, TokenNames.AndPossibleGroup);
            if(finalGroup?.ResultToken == null)
            {
                return null;
            }
            AttachChildren(tempColl);
            AttachChild(finalAnd.ResultToken);
            AttachChild(finalGroup.ResultToken);
            origin = finalGroup.Position;
            return CurrentToken.AsTokenResult(origin);
        }


        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}