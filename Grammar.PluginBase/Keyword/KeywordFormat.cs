using System.Collections.Generic;

namespace Grammar.PluginBase.Keyword
{
    /// <summary>
    /// The format of the file containing all the keywords
    /// </summary>
    public class Format
    {
        /// <summary>
        /// The list of words that can be included within a more complex words (no word separator when reading them)
        /// </summary>
        public List<string> MergeableWords { get; set; }

        /// <summary>
        /// The list of all the keywords
        /// </summary>
        public Dictionary<string, IEnumerable<string>> Keywords { get; set; }

        /// <summary>
        /// the list of tokens names to match keywords types when parsing the tokens
        /// </summary>
        public Dictionary<string, IEnumerable<IEnumerable<string>>> Tokens { get; set; }
    }
}