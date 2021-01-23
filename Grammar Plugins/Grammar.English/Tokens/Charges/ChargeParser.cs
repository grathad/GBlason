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
    /// Charge := (<see cref="TokenNames.SimpleCharge"/> | LocatedCharge | <see cref="TokenNames.MultiCharges"/>). <see cref="TokenNames.LightSeparator"/>?
    /// </code>
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
            var result = TryConsumeOr(ref origin, new[] { TokenNames.SimpleCharge, TokenNames.MultiCharges });
            if(result?.ResultToken == null)
            {
                return null;
            }
            AttachChild(result.ResultToken);
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
