using Grammar.PluginBase.Token.Contracts;

namespace Grammar.PluginBase.Token
{
    /// <inheritdoc />
    /// <summary>
    /// Models that represents the result after consumming a token in the origin
    /// This include the token and the new position in the origin after consumption
    /// </summary>
    public abstract class TokenResultBase : ITokenResultBase
    {
        private ITokenParsingPosition _position;

        /// <summary>
        /// Create a new result with a token and a position
        /// </summary>
        /// <param name="position">The position</param>
        protected TokenResultBase(ITokenParsingPosition position)
        {
            _position = position;
        }

        /// <inheritdoc />
        public virtual ITokenParsingPosition Position
        {
            get => _position;
            protected set => _position = value;
        }
    }
}