using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parser for tokens that represent a symbol sub part
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.SymbolSubPart"/>  :=  <see cref="TokenNames.SymbolSubPartPossession"/>? <see cref="TokenNames.Determiner"/>? 
    /// <see cref="TokenNames.SymbolName"/>. <see cref="TokenNames.SymbolState"/>? <see cref="TokenNames.Tincture"/>?
    /// </para>
    /// </summary>
    /// <remarks>The tincture is the responsibility of the parent, since multiple subpart can share the same</remarks>
    internal class SymbolSubPartParser : ContainerParser
    {
        public SymbolSubPartParser(IParserPilot factory = null)
            : base(TokenNames.SymbolSubPart, factory)
        {
        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            TryConsumeAndAttachOne(ref origin, TokenNames.SymbolSubPartPossession);
            TryConsumeAndAttachOne(ref origin, TokenNames.Determiner);
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.SymbolName))
            {
                return null;
            }
            TryConsumeAndAttachOne(ref origin, TokenNames.SymbolState);
            TryConsumeAndAttachOne(ref origin, TokenNames.Tincture);
            return CurrentToken.AsTokenResult(origin);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}