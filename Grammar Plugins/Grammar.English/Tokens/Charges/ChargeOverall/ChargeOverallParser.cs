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
    /// <see cref="TokenNames.ChargeOverall"/> := 
    /// <see cref="TokenNames.OverallPossibleFirstGroup"/> <see cref="TokenNames.Overall"/> <see cref="TokenNames.OverallPossibleSecondGroup"/>
    /// </para>
    /// </summary>
    internal class ChargeOverallParser : ContainerParser
    {
        public ChargeOverallParser(IParserPilot factory = null)
            : base(TokenNames.ChargeOverall, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            return null;
        }


        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}