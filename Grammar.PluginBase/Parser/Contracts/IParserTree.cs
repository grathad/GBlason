using System;
using System.Collections.Generic;

namespace Grammar.PluginBase.Parser.Contracts
{
    /// <summary>
    /// the contract definition for the parsing tree
    /// </summary>
    public interface IParserTree
    {
        /// <summary>
        /// The root of the tree
        /// </summary>
        ParserNode Root { get; set; }

        /// <summary>
        /// Get the first <see cref="ParserNode"/> that correspond to the node that match the predicate passed in parameter.
        /// <remarks>the way the tree is scanned is by leaf (going all the way to the left leaf, then moving one up and restarting)</remarks>
        /// </summary>
        /// <param name="predicate">The predicate applied to all the element of the tree</param>
        /// <returns>the parser if found, null if not</returns>
        ParserNode GetFirstOrDefault(Func<ParserNode, bool> predicate);

        /// <summary>
        /// Get the first <see cref="ParserNode"/> that correspond to the node that match the predicate passed in parameter.
        /// <remarks>the way the tree is scanned is by leaf (going all the way to the left leaf, then moving one up and restarting)</remarks>
        /// </summary>
        /// <param name="root">The origin of the search (the actual root by default)</param>
        /// <param name="predicate">The predicate applied to all the element of the tree</param>
        /// <returns>the parser if found, null if not</returns>
        ParserNode GetFirstOrDefault(ParserNode root, Func<ParserNode, bool> predicate);

        /// <summary>
        /// Add the child to the parent, if the parent is in the tree. 
        /// <remarks>
        /// If the parent is null, then add the child as the root (replace the old root and destroy the old tree)
        /// </remarks>
        /// </summary>
        /// <param name="parent">The reference to the <paramref name="parent"/> for which we want to add a <paramref name="child"/></param>
        /// <param name="child">The reference to add as a <paramref name="child"/> to the <paramref name="parent"/></param>
        /// <exception cref="ArgumentException">if the <paramref name="parent"/> is not null and does not exist in the tree</exception>
        void AddChild(ParserBase parent, ParserNode child);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        ParserBase FindUpFrom(ParserBase parser, Func<ParserNode, bool> predicate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startingPoint"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        ParserBase FindUpFrom(ParserNode startingPoint, Func<ParserNode, bool> predicate);

        /// <summary>
        /// Find a specific reference of a parser in the tree. And provide the corresponding <see cref="ParserNode"/>
        /// The search is made from the root
        /// </summary>
        /// <param name="parser">The parser to find in the tree</param>
        /// <returns>The parsernode containing the parser or null if nothing is found</returns>
        ParserNode Get(ParserBase parser);

        /// <summary>
        /// Returns the list of parser that are present between the descendant and the ancestor. 
        /// Not including the descendant. Including the last element matching the predicate
        /// </summary>
        /// <param name="descendant">The parser from which to start the aggregation (not included in the final result)</param>
        /// <param name="predicate">The logic to apply on every step to the root, return true to continue the aggregation</param>
        /// <returns>The list of elements that are from the parent of the <paramref name="descendant"/> all the way to the matching <paramref name="predicate"/> or the <see cref="Root"/></returns>
        IEnumerable<ParserBase> AggregateUpUntil(ParserBase descendant, Func<ParserNode, bool> predicate);
    }
}