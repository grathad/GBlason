using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parser for tokens representing a fur
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.TinctureFur"/> := <see cref="TokenNames.SimpleFur"/> | <see cref="TokenNames.Vair"/> | <see cref="TokenNames.Vaire"/>;
    /// </para>
    /// </summary>
    internal class TinctureFurParser : ContainerParser
    {
        public TinctureFurParser(IParserPilot factory = null)
            : base(TokenNames.TinctureFur, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //we try to consume everything possible and we take the solution that end up consuming the most parsed key words
            //for simplest charge
            var result = TryConsumeOr(ref origin,
                TokenNames.SimpleFur,
                TokenNames.Vair,
                TokenNames.Vaire);
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