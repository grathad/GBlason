using System;
using System.Composition;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Grammar.French
{
    /// <summary>
    /// First version of a english grammar parser
    /// </summary>
    [Export(typeof(IGrammarParser))]
    public class FrenchGrammar : IGrammarParser
    {
        /// <summary>
        /// <see cref="IGrammarParser.Parse(string)"/>
        /// </summary>
        /// <param name="blazon"><see cref="IGrammarParser.Parse(string)"/></param>
        /// <returns><see cref="IGrammarParser.Parse(string)"/></returns>
        public ParsingResult Parse(string blazon)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="IGrammarParser.Parse(Stream)"/>
        /// </summary>
        /// <param name="blazon"><see cref="IGrammarParser.Parse(Stream)"/></param>
        /// <returns><see cref="IGrammarParser.Parse(Stream)"/></returns>
        public ParsingResult Parse(Stream blazon)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="IGrammarParser.Parse(Stream, Encoding)"/>
        /// </summary>
        /// <param name="blazon"><see cref="IGrammarParser.Parse(Stream, Encoding)"/></param>
        /// <param name="encoding"><see cref="IGrammarParser.Parse(Stream, Encoding)"/></param>
        /// <returns><see cref="IGrammarParser.Parse(Stream, Encoding)"/></returns>
        public ParsingResult Parse(Stream blazon, Encoding encoding)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="IGrammarParser.ParseAsync(string)"/>
        /// </summary>
        /// <param name="blazon"><see cref="IGrammarParser.ParseAsync(string)"/></param>
        /// <returns><see cref="IGrammarParser.ParseAsync(string)"/></returns>
        public Task<ParsingResult> ParseAsync(string blazon)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="IGrammarParser.ParseAsync(Stream)"/>
        /// </summary>
        /// <param name="blazon"><see cref="IGrammarParser.ParseAsync(Stream)"/></param>
        /// <returns><see cref="IGrammarParser.ParseAsync(Stream)"/></returns>
        public Task<ParsingResult> ParseAsync(Stream blazon)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <see cref="IGrammarParser.ParseAsync(Stream, Encoding)"/>
        /// </summary>
        /// <param name="blazon"><see cref="IGrammarParser.ParseAsync(Stream, Encoding)"/></param>
        /// <param name="encoding"><see cref="IGrammarParser.ParseAsync(Stream, Encoding)"/></param>
        /// <returns><see cref="IGrammarParser.ParseAsync(Stream, Encoding)"/></returns>
        public Task<ParsingResult> ParseAsync(Stream blazon, Encoding encoding)
        {
            throw new NotImplementedException();
        }
    }
}
