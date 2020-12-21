using System;
using System.Collections.Generic;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.PluginBase.Token
{
    /// <inheritdoc cref="IContainerToken" />
    /// <summary>
    /// Represent branches in the tree of tokens. A container is meant to have children, when a leaf is menat to be terminal
    /// </summary>
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class ContainerToken : Token, IContainerToken
    {
        /// <inheritdoc/>
        public virtual List<IToken> Children { get; } = new List<IToken>();

        /// <inheritdoc/>
        public virtual IEnumerable<IToken> InsertChildren(IEnumerable<IToken> toAdd, int position)
        {
            if (Children is null)
            {
                return null;
            }

            if (toAdd is null)
            {
                throw new ArgumentNullException(nameof(toAdd));
            }

            Children.InsertRange(position, toAdd);
            return Children;
        }

        /// <summary>
        /// Override the to String to return the type so it is easier to debug
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Type.ToString();
        }
    }
}