using System.Collections.Generic;

namespace Grammar.PluginBase.Token.Contracts
{
    public interface IMultiTokenResult: ITokenResultBase
    {
        /// <summary>
        /// Add a token and update the position with the latest result
        /// </summary>
        /// <param name="toAdd">The token to add</param>
        /// <param name="lastPosition">The latest position related to the latest token consumption</param>
        void AddToken(IToken toAdd, ITokenParsingPosition lastPosition);

        /// <summary>
        /// Add a token and update the position with the latest result
        /// </summary>
        /// <param name="toAdd">The result token to add, the result position will be used as the last position of this multi result</param>
        void AddResult(ITokenResult toAdd);

        /// <summary>
        /// Add the results of a multi token result in the list of result of this multi token result
        /// </summary>
        /// <param name="toAdd">the multi token result to add</param>
        void AddResults(IMultiTokenResult toAdd);

        /// <summary>
        /// The resulting token
        /// </summary>
        IList<IToken> ResultToken { get; }
    }
}