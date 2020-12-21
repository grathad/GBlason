using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parser to handle tokens that represent a <see cref="TokenNames.Vair"/> (with default colours)
    /// <para>
    /// <h3>Grammar: </h3>
    /// <see cref="TokenNames.Vair"/> := <see cref="TokenNames.Counter"/>? <see cref="TokenNames.VairName"/> 
    /// (<see cref="TokenNames.SymbolStateDeterminer"/>? <see cref="TokenNames.FurOrientationName"/>)?
    /// </para>
    /// </summary>
    internal class VairParser : ContainerParser
    {
        public VairParser(IParserPilot factory = null)
            : base(TokenNames.Vair, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            TryConsumeAndAttachOne(ref origin, TokenNames.Counter);
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.VairName)) { return null; }

            var resultDeterminer = Parse(origin, TokenNames.SymbolStateDeterminer);
            var resultOrientation = Parse(resultDeterminer?.Position ?? origin, TokenNames.FurOrientationName);
            if (resultOrientation == null)
            {
                return new TokenResult(CurrentToken, origin);
            }
            if (resultDeterminer != null)
            {
                AttachChild(resultDeterminer.ResultToken);
            }
            AttachChild(resultOrientation.ResultToken);

            return CurrentToken.AsTokenResult(resultOrientation.Position);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}