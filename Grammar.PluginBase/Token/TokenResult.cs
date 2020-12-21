using Grammar.PluginBase.Token.Contracts;

namespace Grammar.PluginBase.Token
{
    /// <summary>
    /// Represent a result containing a single token
    /// </summary>
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class TokenResult :TokenResultBase, ITokenResult
    {
        /// <inheritdoc />
        public TokenResult(IToken result, ITokenParsingPosition position) :
            base(position)
        {
            ResultToken = result;
        }

        /// <inheritdoc />
        public virtual IToken ResultToken { get; }
    }
}