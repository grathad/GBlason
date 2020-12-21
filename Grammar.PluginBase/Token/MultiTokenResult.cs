using System.Collections.Generic;
using System.Linq;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.PluginBase.Token
{
    /// <summary>
    /// Represent a result containing multiple tokens
    /// </summary>
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class MultiTokenResult : TokenResultBase, IMultiTokenResult
    {
        /// <inheritdoc />
        /// <summary>
        /// Default constructor for internal testing
        /// </summary>
        internal MultiTokenResult() : base(null)
        {
            
        }

        /// <summary>
        /// Create a token result with the new position for multiple tokens
        /// </summary>
        /// <param name="position">The position after token consumption from the chain</param>
        public MultiTokenResult(ITokenParsingPosition position)
            : base(position)
        {

        }

        ///<inheritdoc/>
        public virtual void AddToken(IToken toAdd, ITokenParsingPosition lastPosition)
        {
            Position = lastPosition;
            ResultToken.Add(toAdd);
        }

        ///<inheritdoc/>
        public void AddResult(ITokenResult toAdd)
        {
            AddToken(toAdd.ResultToken, toAdd.Position);
        }

        ///<inheritdoc/>
        public void AddResults(IMultiTokenResult toAdd)
        {
            foreach (var tokenResult in toAdd.ResultToken)
            {
                AddToken(tokenResult, toAdd.Position);
            }
        }

        ///<inheritdoc/>
        public virtual IList<IToken> ResultToken { get; } = new List<IToken>();
    }
}