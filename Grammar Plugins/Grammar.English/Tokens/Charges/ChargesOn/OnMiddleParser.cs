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
    /// <see cref="TokenNames.OnMiddle"/> := 
    /// (<see cref="TokenNames.SimpleCharge"/> | <see cref="TokenNames.Division"/>) <see cref="TokenNames.On"/> <see cref="TokenNames.OnPossibleGroup"/>
    /// </para>
    /// </summary>
    /// <example>
    /// OR, A CROSS BETWEEN FOUR KEYS GULES
    /// </example>
    internal class OnMiddleParser : ContainerParser
    {
        public OnMiddleParser(IParserPilot factory = null)
            : base(TokenNames.OnMiddle, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            var tempColl = new List<IToken>();
            //the mandatory between keyword
            var beforeOn = TryConsumeOr(ref origin, new[] { TokenNames.SimpleCharge, TokenNames.Division });
            if (beforeOn?.ResultToken == null)
            {
                ErrorNoOptionFound(origin.Start);
                return null;
            }

            origin = beforeOn.Position;
            tempColl.Add(beforeOn.ResultToken);

            var on = Parse(origin, TokenNames.On);
            if (on?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.On, origin.Start);
                return null;
            }

            origin = on.Position;
            tempColl.Add(on.ResultToken);

            var onGroup = Parse(origin, TokenNames.OnPossibleGroup);
            if (onGroup?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.OnPossibleGroup, origin.Start);
                return null;
            }

            origin = onGroup.Position;
            tempColl.Add(onGroup.ResultToken);
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