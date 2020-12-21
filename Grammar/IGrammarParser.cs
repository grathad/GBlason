using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Grammar
{
    /// <summary>
    /// This is the contract for all the parsers that can be used in the Blazon project.
    /// Every plugin that want to manage the parsing level of an input (as a string or as a stream) need to inherit and implement this contract
    /// <remarks>
    /// There is only 1 contract with different flavors and access
    /// </remarks>
    /// </summary>
    public interface IGrammarParser
    {
        /// <summary>
        /// Simple direct parsing method that synchronously return a <see cref="ParsingResult"/> from the given <paramref name="blazon"/> string
        /// </summary>
        /// <param name="blazon">the string to parse</param>
        /// <returns>the result of the parsing</returns>
        ParsingResult Parse(string blazon);

        /// <summary>
        /// Simple direct parsing method that synchronously return a <see cref="ParsingResult"/> from the given <paramref name="blazon"/> stream
        /// </summary>
        /// <param name="blazon">the stream to parse using by default a <see cref="Encoding.UTF8"/> encoding. Use the overload to define another encoding</param>
        /// <returns>the result of the parsing</returns>
        ParsingResult Parse(Stream blazon);

        /// <summary>
        /// Simple direct parsing method that synchronously return a <see cref="ParsingResult"/> from the given <paramref name="blazon"/> stream
        /// </summary>
        /// <param name="blazon">the stream to parse using the defined encoding</param>
        /// <param name="encoding">the encoding of the stream to read</param>
        /// <returns>the result of the parsing</returns>
        ParsingResult Parse(Stream blazon, Encoding encoding);

        /// <summary>
        /// Asynchronous parsing method that return a <see cref="ParsingResult"/> from the given <paramref name="blazon"/> string
        /// </summary>
        /// <param name="blazon">the string to parse</param>
        /// <returns>the result of the parsing</returns>
        Task<ParsingResult> ParseAsync(string blazon);
        /// <summary>
        /// Asynchronous parsing method that return a <see cref="ParsingResult"/> from the given <paramref name="blazon"/> stream
        /// </summary>
        /// <param name="blazon">the stream to parse using by default a <see cref="Encoding.UTF8"/> encoding. Use the overload to define another encoding</param>
        /// <returns>the result of the parsing</returns>
        Task<ParsingResult> ParseAsync(Stream blazon);

        /// <summary>
        /// Asynchronous parsing method that return a <see cref="ParsingResult"/> from the given <paramref name="blazon"/> stream
        /// </summary>
        /// <param name="blazon">the stream to parse using the defined encoding</param>
        /// <param name="encoding">the encoding of the stream to read</param>
        /// <returns>the result of the parsing</returns>
        Task<ParsingResult> ParseAsync(Stream blazon, Encoding encoding);
    }
}