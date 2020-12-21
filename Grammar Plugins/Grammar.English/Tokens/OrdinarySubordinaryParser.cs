using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parse a token that represent a sub ordinary
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.OrdinarySubordinary"/> := <see cref="TokenNames.OrdinaryMobile"/> | <see cref="TokenNames.OrdinaryFixed"/>
    /// </para>
    /// </summary>
    internal class OrdinarySubordinaryParser : ContainerParser
    {
        public OrdinarySubordinaryParser(IParserPilot factory = null) 
            : base(TokenNames.OrdinarySubordinary, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            var result = TryConsumeOr(ref origin,
                TokenNames.OrdinaryMobile,
                TokenNames.OrdinaryFixed);
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