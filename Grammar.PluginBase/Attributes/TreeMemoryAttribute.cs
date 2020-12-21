using System;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;

namespace Grammar.PluginBase.Attributes
{
    /// <inheritdoc />
    /// <summary>
    /// Represent all the attribute that base their decision on the current status of the tree, and the node recovered from memory
    /// </summary>
    public abstract class TreeMemoryAttribute : Attribute
    {
        /// <summary>
        /// Define if the execution should continue or not
        /// </summary>
        /// <param name="parserTree">The tree that contain the current parsing status</param>
        /// <param name="nodeFromMemory">The node that exist in the memory</param>
        /// <returns>True if the parsing should proceed, otherwise false</returns>
        public abstract bool Execute(IParserTree parserTree, ParserNode nodeFromMemory);
    }
}