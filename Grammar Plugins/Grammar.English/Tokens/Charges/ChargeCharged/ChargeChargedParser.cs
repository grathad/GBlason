using System;
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
    /// <see cref="TokenNames.SimpleCharge"/> <see cref="TokenNames.Charged"/> <see cref="TokenNames.ChargedPossibleGroup"/>
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
            throw new NotImplementedException();
        }


        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}