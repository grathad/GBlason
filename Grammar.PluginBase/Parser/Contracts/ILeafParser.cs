using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;
using Utils.LinqHelper;

namespace Grammar.PluginBase.Parser
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILeafParser
    {
        /// <summary>
        /// Return the existance of a token matching the current leaf parser at the given position
        /// </summary>
        /// <param name="position">The position (index of) the key to look at in the list of keyword</param>
        /// <returns>true if the leaf exist at the given position (can include multiple keyword AFTER the position), otherwise false</returns>
        public bool Exist(int position);

    }
}