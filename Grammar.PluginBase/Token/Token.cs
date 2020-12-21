using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Grammar.PluginBase.Token.Contracts;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Grammar.PluginBase.Token
{
    /// <summary>
    /// Represent one element on the tree
    /// </summary>
    public abstract class Token : IToken
    {
        public Token()
        {
            UniqueId = Guid.NewGuid();
        }

        /// <inheritdoc/>
        public TokenNames Type { get; set; }

        /// <inheritdoc/>
        public IToken Parent { get; set; }

        /// <inheritdoc/>
        public Guid UniqueId { get; init; }

        /// <inheritdoc/>
        public virtual int GetNbLeaves()
        {
            switch (this)
            {
                case LeafToken _:
                    return 1;
                case ContainerToken container:
                    var sum = container.Children.Sum(child => child.GetNbLeaves());
                    return sum;
                default:
                    throw new NotSupportedException(GetType().FullName);
            }
        }

        /// <inheritdoc/>
        public virtual int GetDepth()
        {
            switch (this)
            {
                case LeafToken _:
                    return 1;
                case ContainerToken container:
                    if (!(container.Children?.Any() ?? false))
                    {
                        return 1;
                    }
                    var max = container.Children.Max(c => c?.GetDepth() ?? 0) + 1;
                    return max;
                default:
                    throw new NotSupportedException(GetType().FullName);
            }
        }

        /// <inheritdoc/>
        public virtual ITokenResult AsTokenResult(ITokenParsingPosition position)
        {
            return position == null
                ? null
                : new TokenResult(this, position);
        }

        /// <inheritdoc/>
        public virtual ITokenResult AsTokenResult(ITokenResultBase lastValidResult)
        {
            return lastValidResult == null
                ? null
                : new TokenResult(this, lastValidResult.Position);
        }
    }
}