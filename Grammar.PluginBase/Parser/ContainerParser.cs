using System;
using System.Collections.Generic;
using System.Linq;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.PluginBase.Parser
{
    /// <summary>
    /// Represent a parser that is used to handle types that have children (not final leaves)
    /// </summary>
    public abstract class ContainerParser : ParserBase
    {
        /// <summary>
        /// The token that is currently being created in the parser
        /// </summary>
        protected virtual ContainerToken CurrentToken
        {
            get;
        }
        
        #region ParserBase

        /// <inheritdoc cref="TryConsume(ref ITokenParsingPosition)"/>
        public abstract override ITokenResult TryConsume(ref ITokenParsingPosition origin);

        #endregion

        /// <summary>
        /// Default constructor for the base of the container parser
        /// </summary>
        /// <param name="type">The type that is meant to be parsed</param>
        /// <param name="pilot">the pilot that is used for the parsing</param>
        protected ContainerParser(TokenNames type, IParserPilot pilot)
            : base(type, pilot)
        {
            CurrentToken = new ContainerToken { Type = type };
        }
        
        #region container helpers

        /// <summary>
        /// Attach a Token as the next child to this container (always attach as the last child).
        /// This is a neutral action that does not edit anything else than the list of children for this container
        /// </summary>
        /// <param name="children">The children tokens to add to the <see cref="CurrentToken"/></param>
        protected void AttachChildren(IEnumerable<IToken> children)
        {
            if (children == null)
            {
                throw new ArgumentNullException(nameof(children));
            }
            foreach (var child in children)
            {
                AttachChildAfter(child, CurrentToken.Children.LastOrDefault());
            }
        }

        /// <summary>
        /// Attach a Token as the next child to this container (always attach as the last child).
        /// This is a neutral action that does not edit anything else than the list of children for this container
        /// </summary>
        /// <param name="child">The children token to add to the <see cref="CurrentToken"/></param>
        protected virtual void AttachChild(IToken child)
        {
            AttachChildAfter(child, CurrentToken?.Children?.LastOrDefault());
        }

        /// <summary>
        /// Attach the <paramref name="child"/> at a certain position in the list of <see cref="ContainerToken.Children"/> 
        /// of this <see cref="CurrentToken"/><br/>
        /// This is a neutral action that does not edit anything else than the list of children for this container
        /// <para>
        /// Assign the parent of the token <see cref="ParserNode.Parent"/> to the current container
        /// </para>
        /// </summary>
        /// <param name="child">The child to attach</param>
        /// <param name="childBefore">The child that will end up being before the <paramref name="child"/> that are being attached, 
        /// set to null if the <paramref name="child"/> should be attached at the start</param>
        protected virtual void AttachChildAfter(
            IToken child,
            IToken childBefore = null)
        {
            if (child == null)
            {
                return;
            }
            if (childBefore == null)
            {
                CurrentToken.InsertChildren(new[] { child }, 0);
            }
            else
            {
                var position = CurrentToken.Children.ToList().IndexOf(childBefore);
                CurrentToken.InsertChildren(new[] { child }, position + 1);
            }
            //attaching the parent
            child.Parent = CurrentToken;
        }

        /// <summary>
        /// Try to consume any of the given names and return the first result that consume the most key words
        /// <remarks>This version DOES update the origin position to the same one as the longest returned option after execution</remarks>
        /// </summary>
        /// <param name="origin">The original key words to parse for tokens. The origin is not altered after execution</param>
        /// <param name="names">The names that can be used to consume (only one of them will be consumed)</param>
        /// <returns>A list of one token that represent one of the possible tokens or null if none match</returns>
        protected virtual ITokenResult TryConsumeOr(
            ref ITokenParsingPosition origin, 
            params TokenNames[] names)
        {
            if (origin == null)
            {
                throw new ArgumentNullException(nameof(origin));
            }
            if (names == null)
            {
                throw new ArgumentNullException(nameof(names));
            }
            if (!names.Any())
            {
                return null;
            }

            ITokenResult longestLastResult = null;

            var maxConsumption = 0;
            var startConsumption = origin.Start;
            foreach (var tokenName in names)
            {
                var result = Parse(origin, tokenName);
                var consumed = result?.Position;
                if (consumed == null)
                {
                    //we are in a cyclic reference, where the parser try to get a result from a token that is not parsed yet (because it contains itself)
                    //for now those cases are always considered false, and we need to potentially add support for cyclic references later
                    continue;
                }
                var nbConsumed = consumed.Start - startConsumption;
                if (maxConsumption >= nbConsumed)
                {
                    continue;
                }
                longestLastResult = result;
                maxConsumption = nbConsumed;
            }
            if (longestLastResult != null)
            {
                origin = longestLastResult.Position;
            }
            return longestLastResult;
        }

        /// <summary>
        /// Try to consume any of the given names and return the first longest result.
        /// <remarks>This version does NOT update the origin position after execution</remarks>
        /// </summary>
        /// <param name="originPosition"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        protected virtual ITokenResult TryConsumeOr(
            int originPosition, 
            params TokenNames[] names)
        {
            ITokenParsingPosition clonedPosition = new TokenParsingPosition
            {
                Start = originPosition
            };
            return TryConsumeOr(ref clonedPosition, names);
        }

        /// <summary>
        /// Simplest way to test and attach a child in the grammar.
        /// <para>
        /// Use this call when you know you expect a specific <see cref="TokenNames"/> next and thus try to read it.
        /// The token can be mandatory or optional. the function will return true, if the token is read, false otherwise
        /// The caller can then decide what to do if the call fails (if mandatory, usually the path is incorrect, if optional then the path continue)
        /// </para>
        /// <para>
        /// this code edit the object itself, so there is no manual update after calling that function.
        /// Change the content of the token to include the child that is being attached (unless it fails in this case the function return false)
        /// Update the position to the represent the consummed tokens in the child attachment logic
        /// </para>
        /// </summary>
        /// <param name="position">The position to start the consumption from</param>
        /// <param name="typeName">The name of the token expected to try to parse</param>
        /// <returns>True if the parsing worked, otherwise false</returns>
        /// <remarks>The child parsed is only attached to the current container if the parse worked</remarks>
        /// <remarks>If the attempt return false then it is expected that the position is the same as the one passed as a parameter</remarks>
        protected virtual bool TryConsumeAndAttachOne(
            ref ITokenParsingPosition position, 
            TokenNames typeName)
        {
            if (position == null)
            {
                throw new ArgumentNullException(nameof(position));
            }
            var tempPosition = new TokenParsingPosition(position);
            var result = Parse(tempPosition, typeName);
            if (result == null)
            {
                return false;
            }
            AttachChild(result.ResultToken);
            position = result.Position;
            return true;
        }
        #endregion
    }
}