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
    /// <see cref="TokenNames.ChargeCharged"/> := 
    /// <see cref="TokenNames.SimpleCharge"/> <see cref="TokenNames.ChargedKeyword"/> <see cref="TokenNames.ChargedPossibleGroup"/>
    /// </para>
    /// </summary>
    internal class ChargeChargedParser : ContainerParser
    {
        public ChargeChargedParser(IParserPilot factory = null)
            : base(TokenNames.ChargeCharged, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            var tempColl = new List<IToken>();

            if (!Exist(origin.Start, TokenNames.Charged))
            {
                ErrorMandatoryTokenMissing(TokenNames.Charged, origin.Start);
                return null;
            }

            var simpleCharge = Parse(origin, TokenNames.SimpleCharge);
            if (simpleCharge?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.SimpleCharge, origin.Start);
                return null;
            }
            origin = simpleCharge.Position;
            tempColl.Add(simpleCharge.ResultToken);

            var keywrd = Parse(origin, TokenNames.ChargedKeyword);
            if (keywrd?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.ChargedKeyword, origin.Start);
                return null;
            }
            origin = keywrd.Position;
            tempColl.Add(keywrd.ResultToken);

            var lastGroup = Parse(origin, TokenNames.ChargedPossibleGroup);
            if (lastGroup?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.ChargedPossibleGroup, origin.Start);
                return null;
            }
            origin = lastGroup.Position;
            tempColl.Add(lastGroup.ResultToken);

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