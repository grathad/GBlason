using System.Diagnostics;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.PluginBase.Token
{
    /// <summary>
    /// This represent the position in the source of data where to start the reading
    /// </summary>
    public class TokenParsingPosition : ITokenParsingPosition
    {
        private int _start;

        /// <summary>
        /// The copy constructor used to avoid updating the values while we are still not sure if the position will need to change or not (children consumption)
        /// </summary>
        /// <param name="toCopy"></param>
        public TokenParsingPosition(ITokenParsingPosition toCopy)
        {
            _start = toCopy.Start;
        }

        /// <summary>
        /// Empty constructor mainly for testing or for non initialized circumstances
        /// </summary>
        public TokenParsingPosition()
        {

        }

        /// <summary>
        /// The default starting position
        /// </summary>
        public static ITokenParsingPosition DefaultStartingPosition => new TokenParsingPosition { Start = 0 };

        #region ITokenParsingPosition

        /// <inheritdoc/>
        public ITokenParsingPosition Copy()
        {
            return new TokenParsingPosition(this);
        }
        
        /// <inheritdoc/>
        public virtual int Start
        {
            get => _start;
            set => _start = value;
        }

        #endregion

        #region Equality Logic

        /// <summary>
        /// Compare 2 poisitions by comparing their <see cref="Start"/>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(TokenParsingPosition left, ITokenParsingPosition right)
        {
            if (left is null && right is null) { return true; }
            if (left is null) { return false; }
            if (right is null) { return false; }
            if (ReferenceEquals(left, right)) { return true; }
            return left.Start == right.Start;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(TokenParsingPosition left, ITokenParsingPosition right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public bool Equals(ITokenParsingPosition other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Start == other.Start;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((TokenParsingPosition)obj);
        }

        /// <summary>
        /// Default hashcode for the position
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Start;
        }

        #endregion
    }
}