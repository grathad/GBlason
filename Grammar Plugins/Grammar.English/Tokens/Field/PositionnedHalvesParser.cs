using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parse tokens that represent a <see cref="TokenNames.PositionnedHalves"/>
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.PositionnedHalves"/> := 
    /// <see cref="TokenNames.FirstDivisionNumber"/>. <see cref="TokenNames.Shield"/>. <see cref="TokenNames.ChargeSeparator"/> <br/>
    /// <see cref="TokenNames.SecondDivisionNumber"/>. <see cref="TokenNames.Shield"/>.
    /// </para>
    /// </summary>
    internal class PositionnedHalvesParser : ContainerParser
    {
        public PositionnedHalvesParser(IParserPilot factory = null)
            : base(TokenNames.PositionnedHalves, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.FirstDivisionNumber)) { return null; }
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.Shield)) { return null; }
            TryConsumeAndAttachOne(ref origin, TokenNames.DivisionSeparator);

            if (!TryConsumeAndAttachOne(ref origin, TokenNames.SecondDivisionNumber)) { return null; }
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.Shield)) { return null; }

            return CurrentToken.AsTokenResult(origin);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}