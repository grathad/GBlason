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
    /// <see cref="TokenNames.BetweenStart"/> := 
    /// <see cref="TokenNames.Between"/> <see cref="TokenNames.BetweenPossibleFirstGroup"/> <see cref="TokenNames.BetweenPossibleSecondGroup"/> (<see cref="TokenNames.Between"/> <see cref="TokenNames.BetweenPossibleSecondGroup"/>)*
    /// </para>
    /// </summary>
    /// <example>
    /// <i>a chevron between </i>two cinquefoils in chief <b>and</b> a hunting-horn in base Or
    /// </example>
    internal class BetweenStartParser : ContainerParser
    {
        public BetweenStartParser(IParserPilot factory = null)
            : base(TokenNames.BetweenStart, factory)
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