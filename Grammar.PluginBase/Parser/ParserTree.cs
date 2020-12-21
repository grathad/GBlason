using System;
using System.Collections.Generic;
using System.Linq;
using Grammar.PluginBase.Parser.Contracts;

namespace Grammar.PluginBase.Parser
{
    /// <summary>
    /// Represent the tree composed of parsers that are built by the parser in order to read the keywords extracted from a blazon
    /// </summary>
    public class ParserTree : IParserTree
    {
        /// <inheritdoc/>
        public virtual ParserNode Root { get; set; }

        /// <inheritdoc cref="GetFirstOrDefault(Func{ParserNode,bool})"/>
        public virtual ParserNode GetFirstOrDefault(Func<ParserNode, bool> predicate)
        {
            return GetFirstOrDefault(Root, predicate);
        }

        /// <inheritdoc cref="GetFirstOrDefault(ParserNode,Func{ParserNode,bool})"/>
        public virtual ParserNode GetFirstOrDefault(ParserNode root, Func<ParserNode, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            if (root == null)
            {
                return null;
            }
            return predicate.Invoke(root)
                ? root
                : root.Children.Select(parserNode => GetFirstOrDefault(parserNode, predicate))
                .FirstOrDefault(candidate => candidate != null);
        }

        /// <inheritdoc cref="Get(ParserBase)"/>
        public virtual ParserNode Get(ParserBase parser)
        {
            return GetFirstOrDefault(p => ReferenceEquals(p.Parser, parser));
        }

        /// <inheritdoc cref="AggregateUpUntil(ParserBase,System.Func{ParserNode,bool})"/>
        /// <summary>
        /// <list type="bullet">
        /// <element>
        /// Returns null if the descendant is null or not in the tree.
        /// </element>
        /// <element>
        /// Returns null if the predicate is null.
        /// </element>
        /// <element>
        /// Returns an empty list if the <paramref name="descendant"/> related <see cref="ParserNode"/> have no <see cref="ParserNode.Parent"/>.
        /// </element>
        /// <element>
        /// Returns all the way to the root if the predicate is not matched
        /// </element>
        /// </list>
        /// <remarks>The list will be reversed: start from the parent of the child, all the way up to the predicate matching parser</remarks>
        /// </summary>
        public IEnumerable<ParserBase> AggregateUpUntil(ParserBase descendant, Func<ParserNode, bool> predicate)
        {
            if (descendant == null)
            {
                return null;
            }
            if (predicate == null)
            {
                return null;
            }
            var startNode = Get(descendant);
            if (startNode == null)
            {
                return null;
            }
            var result = new List<ParserBase>();
            var node = startNode.Parent;
            while (node != null)
            {
                if (predicate.Invoke(node))
                {
                    result.Add(node.Parser);
                }
                else
                {
                    return result;
                }
                node = node.Parent;
            }
            return result;
        }

        /// <inheritdoc cref="AddChild(ParserBase,ParserNode)"/>
        public void AddChild(ParserBase parent, ParserNode child)
        {
            if (child == null)
            {
                return;
            }
            if (parent == null || Root == null)
            {
                Root = child;
                Root.Parent = null;
                return;
            }

            var parentNode = GetFirstOrDefault(node => node.Parser == parent);
            if (parentNode == null)
            {
                throw new ArgumentException(nameof(parent));
            }
            child.Parent = parentNode;
            parentNode.Children.Add(child);
        }

        /// <summary>
        /// Find a node maching the predicate, by starting from the given <paramref name="parser"/> all the way up to the <see cref="Root"/>
        /// </summary>
        /// <param name="parser">The node to start from. The tree use the reference to assert which node it is to start with while exploring itself</param>
        /// <param name="predicate">The logic to select the node from the start. Remember that the parameter is a node and not a parser use <see cref="Parser"/> to access the parser</param>
        /// <returns>The node that match the predicate or null if nothing is matching</returns>
        public virtual ParserBase FindUpFrom(ParserBase parser, Func<ParserNode, bool> predicate)
        {
            var startingPoint = Get(parser);
            return FindUpFrom(startingPoint, predicate);
        }

        /// <summary>
        /// Find a node maching the predicate, by starting from the given <paramref name="startingPoint"/> all the way up to the <see cref="Root"/>
        /// </summary>
        /// <param name="startingPoint">The node from where to start</param>
        /// <param name="predicate">The logic to find the match</param>
        /// <returns>The parser in the node that matched the predicate or null if nothing matches</returns>
        public virtual ParserBase FindUpFrom(ParserNode startingPoint, Func<ParserNode, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            while (startingPoint != null)
            {
                if (predicate.Invoke(startingPoint))
                {
                    return startingPoint.Parser;
                }
                startingPoint = startingPoint.Parent;
            }
            return null;
        }
    }
}