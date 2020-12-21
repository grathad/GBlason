using System;

namespace Grammar.PluginBase.Parser
{
    /// <summary>
    /// Default class to transfer error exception in the parsers, does not need to be used, only a helper
    /// </summary>
    public class ParserErrorException : Exception
    {
        /// <summary>
        /// The character number (position) of the parsing exception
        /// </summary>
        public int CharNumber { get; }
        /// <summary>
        /// The length of the parsed word
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Default helper constructor
        /// </summary>
        /// <param name="explanation">The explanation of the exception</param>
        /// <param name="charNumber">The character number <see cref="CharNumber"/></param>
        /// <param name="length">The lenght <see cref="Length"/></param>
        public ParserErrorException(string explanation, int charNumber = 0, int length = 0) : base(explanation)
        {
            CharNumber = charNumber;
            Length = length;
        }
    }
}