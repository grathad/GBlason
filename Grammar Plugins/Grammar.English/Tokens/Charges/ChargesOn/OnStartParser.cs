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
    /// <see cref="TokenNames.OnStart"/> := 
    /// <see cref="TokenNames.On"/> <see cref="TokenNames.OnPossibleGroup"/> <see cref="TokenNames.LightSeparator"/>? <see cref="TokenNames.OnPossibleGroup"/>
    /// </para>
    /// </summary>
    internal class OnStartParser : ContainerParser
    {
        public OnStartParser(IParserPilot factory = null)
            : base(TokenNames.OnStart, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            var tempColl = new List<IToken>();
            //the mandatory between keyword
            
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

            //potential separator
            var ls = Parse(origin, TokenNames.LightSeparator);
            if(ls?.ResultToken != null)
            {
                origin = ls.Position;
                tempColl.Add(ls.ResultToken);
            }

            onGroup = Parse(origin, TokenNames.OnPossibleGroup);
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