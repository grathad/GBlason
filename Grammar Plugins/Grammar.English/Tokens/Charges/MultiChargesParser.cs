using System;
using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// This represent a complex construct that includes multiple charges, with their own definitions.
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.MultiCharges"/> := 
    /// <see cref="TokenNames.ChargesList"/> | <see cref="TokenNames.ChargeBetweenPosition"/> | 
    /// <see cref="TokenNames.ChargeOnPosition"/> | <see cref="TokenNames.ChargeWithin"/> | 
    /// <see cref="TokenNames.ChargeSurmounted"/> | <see cref="TokenNames.ChargeOverall"/> | <see cref="TokenNames.ChargeCharged"/>
    /// </para>
    /// </summary>
    /// <example>
    /// <i>a chevron between </i>two cinquefoils in chief <b>and</b> a hunting-horn in base Or
    /// </example>
    internal class MultiChargesParser : ContainerParser
    {
        public MultiChargesParser(IParserPilot factory = null)
            : base(TokenNames.MultiCharges, factory)
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