using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Token that represent a tincture that should fill up a field or a surface
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="Tincture"/> := <see cref="TokenNames.SimpleTincture"/> | <see cref="TokenNames.TinctureFur"/>
    /// | <see cref="TokenNames.CounterChanged"/>
    /// </para>
    /// </summary>
    internal class TinctureParser : ContainerParser
    {
        public TinctureParser(
            IParserPilot factory = null)
            : base(TokenNames.Tincture, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //we try to consume everything possible and we take the solution that end up consuming the most parsed key words
            //for simplest charge
            var result = TryConsumeOr(ref origin,
                TokenNames.SimpleTincture,
                TokenNames.TinctureFur,
                TokenNames.CounterChanged);
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