using System.Collections.Generic;
using Grammar.PluginBase.Keyword;

namespace Grammar.PluginBase.Token.Contracts
{
    public interface ILeafToken : IToken
    {
        /// <summary>
        /// The keywords that were used to construct this token
        /// </summary>
        List<ParsedKeyword> OriginalKw { get; set; }
    }
}