﻿using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parse a token representing a simple charge. 
    /// The simple charge is a subset of a charge that can be used to define a charge used in a semy
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="SingleChargeElement"/> := <see cref="TokenNames.SingleOrdinary"/> | <see cref="TokenNames.Symbol"/> | <see cref="TokenNames.SymbolCross"/>
    /// </para>
    /// </summary>
    internal class SingleChargeElementParser : ContainerParser
    {
        public SingleChargeElementParser(IParserPilot factory = null)
            : base(TokenNames.SingleChargeElement, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //it is either an ordinary or a symbol
            var result = TryConsumeOr(ref origin,
                TokenNames.SingleOrdinary,
                TokenNames.Symbol,
                TokenNames.SymbolCross);
            if (result == null)
            {
                return null;
            }
            AttachChild(result.ResultToken);

            return CurrentToken.AsTokenResult(result);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}