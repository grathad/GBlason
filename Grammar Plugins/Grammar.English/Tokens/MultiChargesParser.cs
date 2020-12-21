using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// This represent multiple charges that are listed together, and optionally share a property
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.MultiCharges"/> := 
    /// <see cref="TokenNames.Charge"/>. ((<see cref="TokenNames.LightSeparator"/> | <see cref="TokenNames.And"/>)
    /// <see cref="TokenNames.Charge"/>.)* <see cref="TokenNames.And"/>. <see cref="TokenNames.Charge"/>.
    /// </para>
    /// </summary>
    /// <example>
    /// <i>a chevron between </i>two cinquefoils in chief <b>and</b> a hunting-horn in base Or
    /// </example>
    /// <remarks>
    /// <para>
    /// As for other cases where the grammar content is cyclic, the definition of the <see cref="TokenNames.Charge"/> 
    /// should not be including another <see cref="TokenNames.MultiCharges"/>. But can be something else like a positionned charge.
    /// In this sense we have a case where the multi charge child can't be a charge that contain a multi charge without a separation (like a new shield in between)<br/>
    /// The currently proposed solution is like this: another attribute, more severe than "is redundant" is created
    /// This attribute mark the types that can't exist if another type is present as their parent with a list of type that break the chain
    /// in the case of the multi charges, it is impossible unless there is a shield in somewhere.
    /// (Maybe later we can add the positionned charge as well) but this will break as soon as the positionned charge can support charge and not simplest charge in their grammar
    /// </para>
    /// </remarks>
    internal class MultiChargesParser : ContainerParser
    {
        public MultiChargesParser(IParserPilot factory = null)
            : base(TokenNames.MultiCharges, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //try to get the first charge
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.Charge))
            {
                return null;
            }
            var andConsumed = false;
            var atLeastOneExtraCharge = false;
            //then we consume as many light seprator followed by a charge as we can
            while (origin.Start < ParserPilot.LastPosition)
            {
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.And))
                {
                    //trying the coma
                    if (!TryConsumeAndAttachOne(ref origin, TokenNames.LightSeparator))
                    {
                        //no match we bail out
                        break;
                    }
                    andConsumed = false;
                }
                else
                {
                    andConsumed = true;
                }
                //in the grammar for multi charge, if we have a complex list of charge that contain positioned charges and other multi charges
                //then the logic of the parsing will always return the LONGEST consumption chain
                //in our case the following line, that tries to read a charge
                //if (!TryConsumeAndAttachOne(ref origin, TokenNames.Charge)) { break; }
                //will always return the longest possible charge, even if the actual result for now would be a list of smaller charge that consume as much
                //in order to solve this issue, we try to read a multi charges from there, if it does return one, then we attach all the children to this parent multi charges
                //the problem is that our tree of parsing won't be correct
                
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.Charge)) { break; }
                atLeastOneExtraCharge = true;
            }
            //we should have consumed a and separator already and it have to be the last one we consume
            //also we should have had at least one charge in the loop, not only the separator or the and...
            if (!andConsumed || !atLeastOneExtraCharge)
            {
                return null;
            }
            return CurrentToken.AsTokenResult(origin); ;
        }


        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}