using System.Collections.Generic;
using System.Linq;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;

namespace Grammar.PluginBase.Attributes
{
    /// <inheritdoc />
    /// <summary>
    /// Attribute that can be used to mark specific token that need to not be optimized under specific condition.
    /// <para>
    /// When attempting to parse a specific token, the token parsing result is stored in the parser tree, for the origin position.
    /// The next time there is an attempt to parse the same token on the same position then the tree memory is used, and the parse NOT re attempted.
    /// If the parse failed because the parent prevented a valid child to be detected (due to redundancy for example) then the next attempt, under a different parent
    /// will read the failure of the previous attempt (from memory), even though, if it was re parsed it would work.
    /// In such situation it is possible to use this attribute to mark which token should be re parsed instead of loading them from memory. Based on which parent they have
    /// Every token where the grammar rule include a potential child that is the same than a potential parent should add this potential child-parent to this attribute
    /// </para>
    /// <remarks>
    /// An easy example is the <see cref="F:Grammar.PluginBase.Token.TokenNames.ComplexPositionnedCharges" /> and <see cref="F:Grammar.PluginBase.Token.TokenNames.MultiCharges" /> that (albeit indirectly) contains each other as their potential child (thourhg the <see cref="F:Grammar.PluginBase.Token.TokenNames.Charge" />)
    /// </remarks>
    /// </summary>
    public class ReAttemptParsingForParentAttribute : TreeMemoryAttribute
    {
        /// <summary>
        /// This contain the list of <see cref="TokenNames"/> or types, that, 
        /// if found as the parent when looking in the memory will force a second parsing of the current token.
        /// </summary>
        public List<TokenNames> Parents { get; } = new List<TokenNames>();

        /// <summary>
        /// Initialize an attribute instance with the list of type that will not be counted as valid memory if parent of the current token last parser result
        /// </summary>
        /// <param name="parents">The list of type that will not be counted as valid memory if they are the parent of the current token last parser result</param>
        public ReAttemptParsingForParentAttribute(params TokenNames[] parents)
        {
            if (parents != null)
            {
                Parents.AddRange(parents);
            }
        }

        /// <inheritdoc cref="TreeMemoryAttribute.Execute"/>
        public override bool Execute(IParserTree parserTree, ParserNode nodeFromMemory)
        {
            if (parserTree == null)
            {
                return true;
            }
            if (nodeFromMemory == null)
            {
                return true;
            }
            //if there is no parents defined we return true as we want to use the node as a valid memory item
            if (!(Parents?.Any() ?? false))
            {
                return true;
            }
            //so the logic want us to return false, if the parent of the node is of one of the defined parents types
            //first it needs to have a parent, if not then we will use it as is
            if (nodeFromMemory.Parent?.Parser == null)
            {
                return true;
            }
            //and then if all the parents allowed in the attribute does not match the actual parent node type, we will also use this node from memory
            //we need to look for such a parent in the hierarchy, not only for the direct parent, because we want to support multiple layer of tokens between the 2 that need a re parse
            //getting the list up to the root, or until we find one of the defined parents
            var parent = nodeFromMemory.Parent;
            while (parent != null)
            {
                if (parent.Parser != null && Parents.Any(p => p == parent.Parser.Type))
                {
                    return false;
                }
                if (parent == parserTree.Root)
                {
                    //to avoid potential infinite loop if the root get assigned a parent that is also one of its descendant
                    break;
                }
                parent = parent.Parent;
            }
            return true;
        }
    }
}