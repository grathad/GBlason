using System;
using System.Collections.ObjectModel;
using System.Linq;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;

namespace Grammar.PluginBase.Attributes
{
    /// <inheritdoc />
    /// <summary>
    /// Used to calculate redundancy and avoid execution of a type that contains itself.
    /// This attribute only work if the type contains itself at the exact same position, this should, by default, be assigned to ALL the types.
    /// <para>
    /// This contains 2 modes:<br />
    /// * All: that represent that the type can be redundant with everything (including itself) with the exception of direct parent-child dependency
    /// This is defined by the <see cref="P:Grammar.PluginBase.Attributes.RedundancyAttribute.CanBeRedundant" /> flag. Which is <see cref="F:Grammar.PluginBase.Attributes.RedundancyAttribute.DefaultRedudancy" /> by default<br />
    /// * Specific: that represent the list of type that make the redundancy acceptable IF they are present between the 2 redundant nodes
    /// This is defined by the list of types passed as parameters. If a list is passed then it ALWAYS override the <see cref="P:Grammar.PluginBase.Attributes.RedundancyAttribute.CanBeRedundant" /> status as if it was set to true
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class RedundancyAttribute : PreParsingAttribute
    {
        /// <summary>
        /// The default redundancy capability that can be overwritten to avoid the attribute execution
        /// </summary>
        public const bool DefaultRedudancy = false;

        /// <summary>
        /// True if the type can read itself, and thus will force the execution to always return true
        /// </summary>
        public bool CanBeRedundant { get; }

        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyCollection<TokenNames> ChainBreakers { get; }

        /// <summary>
        /// Initialize an attribute instance with the value to use to mark it as <see cref="CanBeRedundant"/>
        /// </summary>
        /// <param name="canBeRedundant"></param>
        public RedundancyAttribute(bool canBeRedundant = DefaultRedudancy)
        {
            CanBeRedundant = canBeRedundant;
        }

        /// <summary>
        /// Initialize an attribute instance that is always considered potentially redundant (<see cref="CanBeRedundant"/> is set to true)<br/>
        /// And where the list of types acceptable to validate the redundance is provided.<br/>
        /// Sending an empty or null list is the same as calling <see cref="RedundancyAttribute(bool)"/> with the <see cref="DefaultRedudancy"/>
        /// </summary>
        /// <param name="chainBreakers"></param>
        public RedundancyAttribute(params TokenNames[] chainBreakers)
        {
            if (!(chainBreakers?.Any() ?? false))
            {
                CanBeRedundant = DefaultRedudancy;
                return;
            }
            CanBeRedundant = true;
            ChainBreakers = new ReadOnlyCollection<TokenNames>(chainBreakers);
        }

        /// <inheritdoc />
        /// <summary>
        /// This validate if the current type passed in the current parser exist <b>anywhere</b> in the chain of parent as given in the parser base.
        /// <remarks>The <paramref name="currentParser" /> does not need to be in the tree BUT the parent need, as the parser tree start looking from that point</remarks>
        /// The redundance is defined like this:
        /// <para>
        /// Check if the <paramref name="currentParser" /> exist in the current <see cref="T:Grammar.Generics.IParserTree" />, 
        /// starting in the tree from the <paramref name="parentParser" />.
        /// The similarity is defined as equals <see cref="T:System.Type" /> and <see cref="T:Format.Elements.Position" />.
        /// If the similar parser is found and have NOT been executed, then this is considered redundant.
        /// If the similar parser is the direct parent of the parser to test, then this is considered redundant.
        /// We are trying to find a token within a token for the same type, that is potentially infinite we should not consider this case
        /// </para>
        /// <remarks>This redundance can't be overwritten and should be enforced at the pilotage level</remarks>
        /// </summary>
        /// <example>
        /// The different cases of redundancy that we want to be able to detect or not detect:
        /// * Charge &gt; PoisitonnedCharge &gt; Charge. Is a valid example of a charge starting at the same position as its parent
        /// * Charge &gt; PositionnedCharges &gt; Charge &gt; MultiCharge &gt; Charge &gt; PositionnedCharges &gt; Charge Is another valid example of acceptable redundancy
        /// </example>
        /// <param name="tree">the current <see cref="T:Grammar.Generics.IParserTree" /> to use for checking redundancy</param>
        /// <param name="currentParser">The current parser to verify, if it was already in progress or not. The function always return false if this parameter is null. This does NOT need to be in the tree yet</param>
        /// <param name="parentParser">The parent parser of the current parser used to make sure the parser is not trying to parse itself and to start the ancestry research. This parser NEED to be in a node in the tree</param>
        /// <param name="currentPosition">The position of the current parsing attempt. The function always return false if this parameter is null</param>
        /// <returns>True if the parser for the type is already present on the same position and not executed otherwise false</returns>
        public override bool Execute(IParserTree tree, ParserBase currentParser, ParserBase parentParser, int currentPosition)
        {
            if (tree == null)
            {
                return true;
            }
            if (currentParser == null)
            {
                return true;
            }
            if (parentParser == null)
            {
                return true;
            }

            if (CanBeRedundant && ChainBreakers == null)
            {
                //if it can be a redundant token with no special chain defined, 
                //then we only consider it redundant if the direct parent is the same type as the current parser
                //and thus stop the execution to prevent infinite loop. 
                //So even if we mark "yes can be redundant" we still make sure we don't read the same over and over again
                return parentParser.Type != currentParser.Type;
            }

            //we find if we have a parser for the same type of token on the same position that is not executed
            var result = tree.FindUpFrom(parentParser,
                p => p != null &&
                     p.Status != NodeParserStatus.Executed
                     && p.Position.Start == currentPosition
                     && p.Parser.Type == currentParser.Type);

            //nothing found, it can't be redundant then, we let the execution continue
            if (result == null)
            {
                return true;
            }


            if (ChainBreakers == null)
            {
                //else we are now trying to parse the same token at the same position that one we already are in the process of parsing 
                //(not executed)
                //the attribute says it can't be redundant, so we return that the current parsing is not ok for execution
                return false;
            }

            //we have another change, we need to look if any of the chain breakers is present BEFORE the same type as the one we want to try
            //if there is any chain breakers in the list of parser between the parent and the result, then we are ok. If not we can't continue
            if (ChainBreakers.Any(cb => cb == parentParser.Type))
            {
                //the parent is a breaker, so we are good, if it was the same type we would have returned before we reach here
                return true;
            }
            //scanning all the items between the current and the first result.
            var listOfParserBetween = tree.AggregateUpUntil(parentParser, pn => pn.Parser == result)?.ToList();
            if (listOfParserBetween == null)
            {
                //there is nothing between the parent and the result, so we do not even need to check if there is a breaker in the chain, since
                //we tried to check that the parent was a breaker type previously, and there is nothing more to verify on top of the parent
                //since we can't be redundant we stop the execution
                return false;
            }
            //either the list is empty (return true)
            return !listOfParserBetween.Any()
                //or it contains one of the type that is defined in the chain breakers, 
                //thus breaking the chain and allowing the parse to continue (return true as well)
                || ChainBreakers.Any(cb => listOfParserBetween.Any(lopb => lopb.Type == cb));
        }
    }
}