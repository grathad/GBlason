using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Format.Elements;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Try to parse the <see cref="ParsedKeyword"/> into a <see cref="Charge"/>
    /// <para>
    /// <h3>Grammar:</h3> 
    /// <code>
    /// Charge := Location ? 
    /// (
    ///     SimplestCharge |
    ///     (PositionnedCharges | MultiCharges) LightSeparator?
    ///     (
    ///         (Tincture | FieldVariation)? LightSeparator? SharedProperties |
    ///         SharedProperties LightSeparator? (Tincture | FieldVariation) ?
    ///     )?
    /// ) Cadency ?
    /// </code>
    /// <see cref="TokenNames.Location"/>, <see cref="TokenNames.SimplestCharge"/>,
    /// <see cref="TokenNames.PositionnedCharges"/>, <see cref="TokenNames.MultiCharges"/>,
    /// <see cref="TokenNames.LightSeparator"/>, <see cref="TokenNames.Tincture"/>,
    /// <see cref="TokenNames.FieldVariation"/>, <see cref="TokenNames.SharedProperties"/>
    /// , <see cref="TokenNames.Cadency"/>
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Is it possible for a charge, that have a location, to also be a positionned charge ? <br/>
    /// Reminder: a location is an absolute definition, usually defining the location of the object on the parent (mostly the field)<br/>
    /// a position is a relative positioning between 2 objects. Usually wihtin, or between.<br/>
    /// <b>Party per fesse or and sable, in chief a greyhound courant in base an owl within a bordure engrailed all counter-changed</b><br/>
    /// the within is for the owl ? (unlikely) or for the whole parent (here the whole shield) - likely...
    /// If so, how to distinguish the 2 cases ? Need to ask I think
    /// The counter example with a "between" position and a location is in HendersonofStLawrence_Henry
    /// </para>
    /// </remarks>
    internal class ChargeParser : ContainerParser
    {
        public ChargeParser(IParserPilot factory = null)
            : base(TokenNames.Charge, factory) { }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //optional location
            TryConsumeAndAttachOne(ref origin, TokenNames.Location);

            //this is common to all the choices ahead, so we do not include them in the options either for counting the number of token 
            //or to store the tokens consumed, we aggregate at the end
            //we try to consume everything possible and we take the solution that end up consuming the most parsed key words

            ITokenResultBase longestOption = null;

            #region multi charges

            var multiCharges = TryConsumeOption(TokenNames.MultiCharges, origin);
            if (multiCharges != null)
            {
                //it can't be a multi charges, if the multi charge second charge have a location and we have too...
                //but the fact that the logic is to be put here seems wrong, need more time to think this through
                longestOption = multiCharges;
            }

            #endregion

            #region positionned charges

            var positionnedCharges = TryConsumeOption(TokenNames.PositionnedCharges, origin);
            if (positionnedCharges != null)
            {
                if (longestOption == null ||
                    longestOption.Position.Start < positionnedCharges.Position.Start)
                {
                    longestOption = positionnedCharges;
                }
            }

            #endregion

            #region simple charge

            var sc = Parse(origin, TokenNames.SimpleCharge);
            if (sc?.ResultToken != null)
            {
                //because we "remember" the result from one call to another, we can't "fake" a call to a charge and return the result of a "simplestcharge"
                //all calls have to be uniquely defined relatively to the choice available
                //if not when trying to recover the latest result we will use the one for the "wrong" type
                //in this case the memory was saving the result from the simplestcharge parsing with the type "charge"
                //this means that the tree will be less clean to read (the simplest charge will be the child of a charge)
                if (longestOption == null ||
                    longestOption.Position.Start < sc.Position.Start)
                {
                    longestOption = sc;
                    origin = sc.Position;
                }
            }
            #endregion

            switch (longestOption)
            {
                case null:
                    ErrorNotEnoughChildren(origin.Start);
                    return null;
                case TokenResult str:
                    AttachChild(str.ResultToken);
                    break;
                case MultiTokenResult mstr:
                    AttachChildren(mstr.ResultToken);
                    break;
                default:
                    throw new InvalidCastException(nameof(TokenResult));
            }
            origin = longestOption.Position;

            //now we try to consume what is after the main part (optional mark of cadency)
            TryConsumeAndAttachOne(ref origin, TokenNames.Cadency);

            return CurrentToken.AsTokenResult(origin);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }

        private MultiTokenResult TryConsumeOption(TokenNames name, ITokenParsingPosition origin)
        {
            var results = new MultiTokenResult(origin);
            var result = Parse(origin, name);

            if (result?.ResultToken == null)
            {
                return null;
            }
            results.AddResult(result);
            //the potential issue is that we can't accept a field variation AND a tincture after the charge, it's either one or the other
            //but for now we try with this definition, might end up needing refactoring
            ITokenParsingPosition tempPosition = new TokenParsingPosition(origin);
            var extra = TryConsumeOrAll(ref tempPosition,
                TokenNames.LightSeparator,
                TokenNames.FieldVariation,
                TokenNames.Tincture,
                TokenNames.SharedProperties);

            if (extra?.ResultToken == null)
            {
                return results;
            }
            results.AddResults(extra);
            return results;
        }

        /// <summary>
        /// This function try to consume EXACTLY ONE of each of the given types, potentially separated by a given token
        /// The order does not matter.
        /// If a separator between those objects is provided, it will only be consumed if the separator IS followed by another consumable
        /// If not then the separator is left for the parent to consume
        /// <remarks>The passed <paramref name="origin"/> is altered to the latest position of the result if at least one is found</remarks>
        /// </summary>
        /// <param name="origin">The original position in the list of keywords we try to consume</param>
        /// <param name="namesToLookFor">The token names to look for in the list of keywords available, the order does NOT matter</param>
        /// <param name="separator">The potential separator between those tokens, use null if there are no separator, always define the value as it is the same type as the list of params</param>
        /// <returns>The list of token succesfully created from our keywords consumed</returns>
        /// <exception cref="NotSupportedException">When a found parsed result does not match the excepted names to look for - to avoid forever loop - should not happen</exception>
        protected MultiTokenResult TryConsumeOrAll(
            ref ITokenParsingPosition origin,
            TokenNames? separator = null,
            params TokenNames[] namesToLookFor)
        {
            if (origin == null)
            {
                throw new ArgumentNullException(nameof(origin));
            }
            if (ParserPilot == null)
            {
                return null;
            }
            //trying to consume either the shared property, or the tincture
            var acceptedToken = new List<TokenNames>(namesToLookFor);
            ITokenResult potentialSeparator = null;
            var results = new MultiTokenResult(origin);
            while (origin.Start <= ParserPilot.LastPosition
                && acceptedToken.Any())
            {
                var tempPosition = results.Position;
                //we might start with a separator, that will be consumed if we have an item after
                if (separator != null)
                {
                    potentialSeparator = Parse(results.Position, separator.Value);
                    if (potentialSeparator?.Position != null)
                    {
                        tempPosition = potentialSeparator.Position;
                    }
                }

                var result = TryConsumeOr(ref tempPosition, acceptedToken.ToArray());

                if (result?.ResultToken == null)
                {
                    //no valid match we return what we got so far without even consuming our latest separator
                    return Enumerable.Any(results.ResultToken)
                        ? results
                        : null;
                }
                //we found something
                //we consume our separator, then our token
                if (potentialSeparator != null)
                {
                    results.AddResult(potentialSeparator);
                    separator = null;
                }
                results.AddResult(result);

                //if the retrieved token have no relation ship with the types available we have an error (and a potential forever loop)
                if (acceptedToken.All(r => r != result.ResultToken.Type))
                {
                    throw new NotSupportedException(nameof(namesToLookFor));
                }
                //we remove the token we found so we can start looking for the potential second
                acceptedToken.Remove(result.ResultToken.Type);
                //we have a second potential separator in between !! (pretty much we restart the same process one last time)
            }
            var finalResult = !Enumerable.Any(results.ResultToken)
                ? null
                : results;
            if (finalResult != null)
            {
                origin = finalResult.Position;
            }
            return finalResult;
        }
    }
}
