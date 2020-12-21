using System;

namespace Grammar
{
    /// <summary>
    /// The definition of an error during a parsing of a Blazon string
    /// </summary>
    public class ParserError : EventArgs
    {
        /// <summary>
        /// Create and initialize an error
        /// </summary>
        /// <param name="characterNumber"><see cref="CharacterNumber"/></param>
        /// <param name="explanation"><see cref="Explanation"/></param>
        public ParserError(int characterNumber, string explanation) : this(characterNumber, 0, explanation)
        {
        }


        /// <summary>
        /// Create and initialize an error
        /// </summary>
        /// <param name="characterNumber"><see cref="CharacterNumber"/></param>
        /// <param name="explanation"><see cref="Explanation"/></param>
        /// <param name="length"><see cref="Length"/></param>
        public ParserError(int characterNumber, int length, string explanation)
        {
            Length = length;
            CharacterNumber = characterNumber;
            Explanation = explanation;
        }

        /// <summary>
        /// The character number that triggered the issue
        /// </summary>
        public int CharacterNumber { get;  }

        /// <summary>
        /// The length of the text from the character number that contains the issue
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// The reason why this is an issue
        /// </summary>
        public string Explanation { get; }
    }
}