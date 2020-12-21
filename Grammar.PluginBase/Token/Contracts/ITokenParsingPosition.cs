using System;

namespace Grammar.PluginBase.Token.Contracts
{
    public interface ITokenParsingPosition : IEquatable<ITokenParsingPosition>
    {
        /// <summary>
        /// Quick copy of an instance of a position
        /// </summary>
        /// <returns>A new instance of the same position</returns>
        ITokenParsingPosition Copy();

        /// <summary>
        /// The starting index of the current position in the total chain of positions
        /// </summary>
        int Start { get; set; }
    }
}