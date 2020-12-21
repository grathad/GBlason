using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// A Simple positionned charge is at least 2 charges that are positionned compared to each other separated with the position definition or a simple separator
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.SimplePositionnedCharges"/> := 
    /// (<see cref="TokenNames.Charge"/>. <see cref="TokenNames.LightSeparator"/>? <see cref="TokenNames.PositionBetween"/>. <see cref="TokenNames.SimplestCharge"/>). | <br/>
    /// (<see cref="TokenNames.Charge"/>. <see cref="TokenNames.LightSeparator"/>? <see cref="TokenNames.Charged"/>. <see cref="TokenNames.Charge"/>.) | <br/>
    /// (<see cref="TokenNames.PositionBefore"/>. <see cref="TokenNames.Charge"/>. <see cref="TokenNames.LightSeparator"/>? <see cref="TokenNames.Charge"/>.) | <br/>
    /// (<see cref="TokenNames.PositionOverall"/>. <see cref="TokenNames.SimplestCharge"/>.)
    /// </para>
    /// </summary>
    /// <example>
    /// <h4>Position in the middle</h4>
    /// three piles Sable within a bordure Gules
    /// <h4>Position at the start</h4>
    /// on a cross gules five mullets or
    /// </example>
    /// <remarks>
    /// Since this parser represent a potential collection of <see cref="TokenNames.Charge"/> it needs to support refactoring on the <see cref="TokenNames.Tincture"/>.<br/>
    /// This means that if the charges in the list, only declare one tincture, and this tincture is the last token of the latest charge, then this is actually belonging to the list and not to the charge
    ///  
    /// <para>
    /// Is it possible for a positionned charge, that have a "surmounted by" as the position between, to be followed (as the second charge) by a multiple charge with itself a location or a "surmounted by" ?<br/>
    /// Reminder: a location is an absolute definition, usually defining the location of the object on the parent (mostly the field)<br/>
    /// a position is a relative positioning between 2 objects. Usually within, between, surmounted by.<br/>
    /// <b>in the centre a cross Argent surmounted by a saltire Gules and in dexter chief a crescent surmounted by a mullet</b><br/>
    /// So in the current logic if we answer yes to the question, this is the reading<br/>
    /// [in the center] = location
    /// [a cross argent] = charge
    /// [surmounted by] = between position. Which means the current construct is a positionned charge and we need the second charge to complete it
    /// [a saltire gules] = charge, it should stop here, but since we try to read the longest possible charge, then we end up with potentially this one:
    /// [a saltire Gules and in dexter chief a crescent surmounted by a mullet], which is a valid multicharge.
    /// The way to potentially kick it out as "not a valid option" would be from the SimplePositionnedChargesParser perspective to scan the second charge
    /// and if it contains a location, or another "surmounted by" to kick this solution as null. And thus only the simple charge would remain
    /// The original implementation before refactoring was only expecting simple charge after the position.<br/>
    /// 2 solutions:<br/>
    /// * we only accept a simple charge after the "between" so that the example before works.<br/>
    /// * we add business logic and scan the potential result and declare it "invalid" if the content is incompatible. That might be the only solution if the first one is incompatible with new cases that requires complex charge after between
    /// </para>
    /// </remarks>
    internal class SimplePositionnedChargesParser : ContainerParser
    {
        public SimplePositionnedChargesParser(IParserPilot factory = null)
            : base(TokenNames.SimplePositionnedCharges, factory) { }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.Charge))
            {
                //this is likely a "position before" start, or not a positioneed charge construct at all
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.PositionBefore))
                {
                    //we try the "overall" as the last option
                    if (!TryConsumeAndAttachOne(ref origin, TokenNames.PositionOverall))
                    {

                        return null;
                    }
                    //ok this is an overall, we consume the following charge
                    if (!TryConsumeAndAttachOne(ref origin, TokenNames.SimpleCharge))
                    {
                        return null;
                    }
                }
                //position before case
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.Charge))
                {
                    return null;
                }
                TryConsumeAndAttachOne(ref origin, TokenNames.LightSeparator);
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.Charge))
                {
                    return null;
                }
            }
            else
            {
                //both alternatives (charged or position between) have a potential light separator
                TryConsumeAndAttachOne(ref origin, TokenNames.LightSeparator);
                //here we started with a simplest charge so either we are "charged" or we are "position between" or we are not in a positionned charge
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.PositionBetween))
                {
                    //maybe a "charged" situation
                    if (!TryConsumeAndAttachOne(ref origin, TokenNames.Charged))
                    {
                        return null;
                    }
                }
                //here we are in a potential position between where the only left item is a charge
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.Charge))
                {
                    return null;
                }
            }

            //RefactorTincture();

            return CurrentToken.AsTokenResult(origin);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}