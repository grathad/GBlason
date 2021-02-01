using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parse a token representing a charge used for a custom semy. 
    /// This has to be a single element, with a simple tincture (no semy or variation of the field)
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="SemyCharge"/> := (<see cref="TokenNames.Ordinary"/> | <see cref="TokenNames.Symbol"/> | <see cref="TokenNames.SymbolCross"/>)
    /// </para>
    /// </summary>
    internal class SemyChargeParser : ContainerParser
    {
        public SemyChargeParser(IParserPilot factory = null)
            : base(TokenNames.SemyCharge, factory)
        {

        }
        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //it is either an ordinary or a symbol
            var result = TryConsumeOr(ref origin,
                TokenNames.Ordinary,
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