using System.Collections.Generic;
using Grammar.PluginBase.Token;

namespace Grammar.PluginBase.Keyword
{
    /// <summary>
    /// The contract representing the resources available for token parsing.
    /// Defines the tools used in the default architecture (internal plugins for different parsing languages).
    /// It is not mandatory to use in your own custom parser implementation.
    /// </summary>
    public interface IResources
    {
        /// <summary>
        /// Get all the keywords in the resources as a dictionary (key) and list of values matching each key
        /// </summary>
        /// <returns></returns>
        Dictionary<string, IEnumerable<string>> GetKeywords();
        /// <summary>
        /// Get all the tokens in the resource as a dictionary (token's key) and the list of chain of keywords that can be used to validate the token key
        /// </summary>
        /// <returns></returns>
        Dictionary<string, IEnumerable<IEnumerable<string>>> GetTokens();
        /// <summary>
        /// The list of chain of keywords that can be used to validate the token key, for the given token name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IEnumerable<IEnumerable<string>> GetTokens(TokenNames name);
        /// <summary>
        /// The list of all the words that are considered "meregeable" meaning that their reading works if they are in one word or a chain of words (with any serparators)
        /// </summary>
        /// <returns></returns>
        List<string> GetMergeableWords();
    }
}