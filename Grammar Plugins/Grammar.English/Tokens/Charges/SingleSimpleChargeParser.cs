using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    internal class SingleSimpleChargeParser : ContainerParser
    {
        /// <summary>
        /// This is an exception, an helper to represent a charge with minimalistic content.
        /// <para>
        /// <h3>Grammar:</h3>
        /// <see cref="TokenNames.SingleSimpleCharge"/> := <see cref="TokenNames.SingleDeterminer"/> <see cref="TokenNames.SingleChargeElement"/> <br/> 
        /// (<br/>
        /// (<see cref="TokenNames.Tincture"/> | <see cref="TokenNames.FieldVariation"/>)? <see cref="TokenNames.SharedProperties"/> |<br/>
        /// <see cref="TokenNames.SharedProperties"/>? (<see cref="TokenNames.Tincture"/> | <see cref="TokenNames.FieldVariation"/>)<br/>
        /// )
        /// </para>
        /// </summary>
        /// <example>
        /// A baton Gules
        /// </example>
        /// <remarks>The tincture is not optional, if not present the charge will be considered as a complex charge, with refactored tincture, or a different grammar for known ordinary (that implies their tincture)</remarks> 
        public SingleSimpleChargeParser(IParserPilot factory = null)
            : base(TokenNames.SingleSimpleCharge, factory) { }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            var tempColl = new List<IToken>();
            var determiner = Parse(origin, TokenNames.SingleDeterminer);
            if(determiner?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.SingleDeterminer, origin.Start);
                return null;
            }

            origin = determiner.Position;
            tempColl.Add(determiner.ResultToken);
            //AttachChild(determiner.ResultToken);

            //we try to consume everything possible and we take the solution that end up consuming the most parsed key words
            //for simplest charge

            var result = Parse(origin, TokenNames.SingleChargeElement);
            if (result?.ResultToken == null)
            {
                ErrorMandatoryTokenMissing(TokenNames.SingleChargeElement, origin.Start);
                return null;
            }
            origin = result.Position;
            tempColl.Add(result.ResultToken);
            //AttachChild(result.ResultToken);

            var results = TryConsumeOrAll(ref origin,
                TokenNames.LightSeparator,
                TokenNames.FieldVariation,
                TokenNames.Tincture,
                TokenNames.SharedProperties);

            if (results?.ResultToken != null)
            {
                tempColl.AddRange(results.ResultToken);
                //AttachChildren(results.ResultToken);
            }
            AttachChildren(tempColl);
            return CurrentToken.AsTokenResult(results?.Position ?? result.Position);
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

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}