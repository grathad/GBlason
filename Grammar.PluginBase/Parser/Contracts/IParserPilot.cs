using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Format.Elements;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.PluginBase.Parser.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IParserPilot
    {
        /// <summary>
        /// Return the value of the last position from the origin list of keywords. 
        /// The value is a 0 starting index, and the position of the last item is provided (so all items - 1)
        /// </summary>
        /// <returns>the value of the last position from the origin</returns>
        int LastPosition { get; }

        /// <summary>
        /// Check if the token exist in the list of remaining tokens after the given position.
        /// Only support leaf parsers
        /// </summary>
        /// <param name="origin">the position from which to start looking (included)</param>
        /// <param name="token">the token to look for</param>
        /// <returns>true if the token is present otherwise false</returns>
        bool Exist(int origin, TokenNames token);

        /// <summary>
        /// Create the tree and return the element asked as the root.
        /// </summary>
        /// <param name="currentPosition">The current position in the source. Default to null (when it is starting as it is the root)</param>
        /// <param name="parent">The parent parser that is requesting for a parsing. Default to null (when starting as it is the root)</param>
        /// <param name="token">The token to try to read (the ask). Default to <see cref="Shield"/></param>
        /// <returns>The parsed token along with the new position post reading (<see cref="TokenResult"/>) if the token parsing worked, or null if the reading was in error.</returns>
        ITokenResult Parse(
            ITokenParsingPosition currentPosition = null,
            ParserBase parent = null, 
            TokenNames token = TokenNames.Shield);

        /// <summary>
        /// Create the tree and return the element asked as the root, asynchronously
        /// </summary>
        /// <param name="currentPosition">The current position in the source. Default to null (when it is starting as it is the root)</param>
        /// <param name="parent">The parent parser that is requesting for a parsing. Default to null (when starting as it is the root)</param>
        /// <param name="token">The token to try to read (the ask). Default to <see cref="Shield"/></param>
        /// <returns>The task executing the parsing of the token along with the new position post reading (<see cref="TokenResult"/>) if the token parsing worked, or null if the reading was in error.</returns>
        Task<ITokenResult> ParseAsync(
            ITokenParsingPosition currentPosition = null,
            ParserBase parent = null,
            TokenNames token = TokenNames.Shield);

        /// <summary>
        /// Return the keywords from the parsed list at the given position
        /// </summary>
        /// <param name="position">The position of the keyword to return</param>
        /// <returns>The keyword</returns>
        /// <exception cref="IndexOutOfRangeException">If the position is not within the collection range</exception>
        ParsedKeyword GetKeyword(int position);

        /// <summary>
        /// Return the list of key words that are located after the start position
        /// </summary>
        /// <param name="position">the position to look from</param>
        /// <returns>the remaining list of keywords, an empty list if the position is invalid</returns>
        IList<ParsedKeyword> GetRemainingKeywords(int position);

        /// <summary>
        /// Store and make the tree of calls readable to the pilot user (for debug purpose)
        /// </summary>
        IParserTree CallTree { get; }
    }
}