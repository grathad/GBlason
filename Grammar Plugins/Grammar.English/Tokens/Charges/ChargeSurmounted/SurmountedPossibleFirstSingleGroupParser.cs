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
    /// <see cref="TokenNames.SurmountedPossibleFirstSingleGroup"/> := 
    /// <see cref="TokenNames.SingleSimpleCharge"/> | <see cref="TokenNames.SingleChargeCharged"/>
    /// </para>
    /// </summary>
    internal class SurmountedPossibleFirstSingleGroupParser : ContainerParser
    {
        public SurmountedPossibleFirstSingleGroupParser(IParserPilot factory = null)
            : base(TokenNames.SurmountedPossibleFirstSingleGroup, factory)
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