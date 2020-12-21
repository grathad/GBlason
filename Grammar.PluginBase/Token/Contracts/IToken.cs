using System;

namespace Grammar.PluginBase.Token.Contracts
{
    public interface IToken
    {
        /// <summary>
        /// The type of token
        /// </summary>
        TokenNames Type { get; set; }

        /// <summary>
        /// The unique identifier for an instance of Token for comparison purposes, especially when the same token is used as multiple answer
        /// </summary>
        Guid UniqueId { get; init; }

        /// <summary>
        /// This token's parent
        /// </summary>
        IToken Parent { get; set; }

        /// <summary>
        /// Get the number of leaves starting to this point in the tree
        /// </summary>
        /// <returns>The number of leaves</returns>
        int GetNbLeaves();

        /// <summary>
        /// Get the longest branches path to a leaf starting from this point in the tree.
        /// The number returned represent 1 value per branch node INCLUDING the final leaf
        /// </summary>
        /// <returns>the longest number of step to reach the further leaf from that point in the tree or 1 if in a leaf</returns>
        int GetDepth();

        /// <summary>
        /// Turn a token into a token result
        /// </summary>
        /// <param name="position">The position</param>
        /// <returns>Null if any of the input parameter is null, otherwise a new <see cref="TokenResult"/> with the 2 input as properties</returns>
        ITokenResult AsTokenResult(ITokenParsingPosition position);

        /// <summary>
        /// Turn a token into a token result
        /// </summary>
        /// <param name="lastValidResult">The last valid result to extract the position from</param>
        /// <returns>Null if any of the input parameter is null, otherwise a new <see cref="TokenResult"/> with the 2 input as properties</returns>
        ITokenResult AsTokenResult(ITokenResultBase lastValidResult);
    }
}