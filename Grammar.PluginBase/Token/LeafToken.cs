using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.PluginBase.Token
{
    /// <summary>
    /// Represent leaves in the tree of tokens
    /// </summary>
    public class LeafToken : Token, ILeafToken
    {
        /// <inheritdoc/>
        public virtual List<PluginBase.Keyword.ParsedKeyword> OriginalKw { get; set; } = new List<PluginBase.Keyword.ParsedKeyword>();

        /// <summary>
        /// Debugger help to read the name of the token
        /// <remarks>
        /// Does not display the actual input, but a reconstruction, 
        /// the keywords are separated by spaces even though originally they could have been using other separators
        /// </remarks>
        /// </summary>
        /// <returns>A string representation of the current leaf token</returns>
        public override string ToString()
        {
            if (OriginalKw == null || !OriginalKw.Any())
            {
                return base.ToString();
            }
            var tr = new StringBuilder();
            foreach (var parsedKeyword in OriginalKw)
            {
                tr.Append(parsedKeyword.Value);
                tr.Append(" ");
            }
            return tr.ToString();
        }
    }
}