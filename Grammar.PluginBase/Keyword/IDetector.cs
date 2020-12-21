using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Grammar.PluginBase.Keyword
{
    /// <summary>
    /// Contract that represent the capacity of the "detector" part of the parsing process
    /// </summary>
    public interface IDetector
    {
        /// <summary>
        /// Do the first scan of the words in the blazon and assign each of them to their representative <see cref="ParsedKeyword"/>
        /// </summary>
        /// <param name="blazon">the source stream to read and extract the keywords from. This is copied and the position is reset to the start of the stream</param>
        /// <param name="encoding">the encoding of the input stream (optional, fall back to UTF8 if null)</param>
        /// <returns>The list of parsed terms in their original order</returns>
        IEnumerable<ParsedKeyword> DetectKeywords(Stream blazon, Encoding encoding = null);
    }
}