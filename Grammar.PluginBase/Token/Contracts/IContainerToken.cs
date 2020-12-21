using System.Collections.Generic;

namespace Grammar.PluginBase.Token.Contracts
{
    /// <summary>
    /// Contract representing the capabilities of a token containing other tokens
    /// </summary>
    public interface IContainerToken : IToken
    {
        /// <summary>
        /// This token children, added in the order of the keyword and then the order of the grammar
        /// </summary>
        List<IToken> Children { get; }

        /// <summary>
        /// Helper to insert children to the token
        /// </summary>
        /// <param name="toAdd">the token to insert</param>
        /// <param name="position">the position where to add the tokens</param>
        /// <returns>The list of children or null if the list is null</returns>
        IEnumerable<IToken> InsertChildren(IEnumerable<IToken> toAdd, int position);
    }
}