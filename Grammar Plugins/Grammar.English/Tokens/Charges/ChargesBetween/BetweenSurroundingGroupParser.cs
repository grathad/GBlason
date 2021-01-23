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
    /// <see cref="TokenNames.BetweenSurroundingGroup"/> := 
    /// (<see cref="TokenNames.PluralSimpleCharge"/> | <see cref="TokenNames.ChargesList"/> | <see cref="TokenNames.PluralSurmounted"/> | <see cref="TokenNames.PluralCharged"/>)
    /// </para>
    /// </summary>
    /// <example>
    /// OR, A CROSS BETWEEN FOUR KEYS GULES
    /// </example>
    internal class BetweenSurroundingGroupParser : ContainerParser
    {
        public BetweenSurroundingGroupParser(IParserPilot factory = null)
            : base(TokenNames.BetweenSurroundingGroup, factory)
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