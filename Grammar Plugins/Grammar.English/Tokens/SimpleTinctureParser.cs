using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Represent a token that contain a simple tincture (so not a vair or vaire)
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.SimpleTincture"/> := <see cref="TokenNames.TinctureColour"/> | <see cref="TokenNames.TinctureMetal"/> | <see cref="TokenNames.TinctureReference"/> | <see cref="TokenNames.TinctureProper"/>
    /// </para>
    /// </summary>
    /// <example>
    /// <h4>Metal</h4>
    /// Argent
    /// <h4>Colour</h4>
    /// Sable
    /// <h4>Reference</h4>
    /// Of the last
    /// </example>
    internal class SimpleTinctureParser : ContainerParser
    {
        public SimpleTinctureParser(IParserPilot factory = null)
            : base(TokenNames.SimpleTincture, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //we try to consume everything possible and we take the solution that end up consuming the most parsed key words
            //for simplest charge
            var result = TryConsumeOr(ref origin,
                TokenNames.TinctureColour,
                TokenNames.TinctureMetal,
                TokenNames.TinctureReference,
                TokenNames.TinctureProper);
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