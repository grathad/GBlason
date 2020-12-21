using System;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;

namespace Grammar.PluginBase.Attributes
{
    /// <inheritdoc />
    /// <summary>
    /// Attributes that can be executed to know if we can contine to parse the tree of node in the pilot
    /// </summary>
    public abstract class PreParsingAttribute : Attribute
    {
        /// <summary>
        /// The only madnatory method is getting the tree, the current parser that we want to try to parse
        /// And the parent that is part of the parsed item and in the current tree
        /// </summary>
        /// <param name="tree">The tree of parser as it was executed so far</param>
        /// <param name="currentParser">The current parser that we will use to parse the current type</param>
        /// <param name="parentParser">The current parser parent that is (likely) already part of the tree</param>
        /// <param name="currentPosition">The current position in the source that triggered the parse attempt of the current type</param>
        /// <returns>True if the parsing is to continue, false otherwise</returns>
        public abstract bool Execute(
            IParserTree tree, 
            ParserBase currentParser, 
            ParserBase parentParser,
            int currentPosition);
    }
}