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
    /// <see cref="TokenNames.EachCharged"/> := 
    /// <see cref="TokenNames.Each"/> <see cref="TokenNames.ChargedKeyword"/>
    /// </para>
    /// </summary>
    internal class EachChargedParser : ContainerParser
    {
        public EachChargedParser(IParserPilot factory = null)
            : base(TokenNames.EachCharged, factory)
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

            var simpleCharge = Parse(origin, TokenNames.Each);
            if (simpleCharge?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.Each, origin.Start);
                return null;
            }
            origin = simpleCharge.Position;
            tempColl.Add(simpleCharge.ResultToken);

            var keywrd = Parse(origin, TokenNames.Charged);
            if (keywrd?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.Charged, origin.Start);
                return null;
            }
            origin = keywrd.Position;
            tempColl.Add(keywrd.ResultToken);

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