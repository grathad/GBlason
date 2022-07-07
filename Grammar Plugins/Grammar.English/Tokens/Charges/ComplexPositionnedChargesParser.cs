using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Complex positionned charges are list of charges that need to be separated with a <see cref="TokenNames.ChargeSeparator"/> because the start is a complex list of charge itself
    /// <para>
    /// <h3>Grammar:</h3> 
    /// <see cref="TokenNames.ComplexPositionnedCharges"/> := 
    /// (<see cref="TokenNames.SimplePositionnedCharges"/> | <see cref="TokenNames.MultiCharges"/>)
    /// <see cref="TokenNames.ComplexPositionnedChargeSeparator"/>. <see cref="TokenNames.PositionBetween"/>. <see cref="TokenNames.Charge"/>. |<br/>
    /// <see cref="TokenNames.PositionBefore"/>. (<see cref="TokenNames.SimplePositionnedCharges"/> | <see cref="TokenNames.MultiCharges"/>). <see cref="TokenNames.ChargeSeparator"/>? <see cref="TokenNames.Charge"/>. | <br/>
    /// <see cref="TokenNames.PositionOverall"/>. (<see cref="TokenNames.SimplePositionnedCharges"/> | <see cref="TokenNames.MultiCharges"/>)
    /// </para>
    /// </summary>
    internal class ComplexPositionnedChargesParser : ContainerParser
    {
        public ComplexPositionnedChargesParser(IParserPilot factory = null)
            : base(TokenNames.ComplexPositionnedCharges, factory) { }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //todo separate the logic between different types of positions
            //position between, position "on", and other keywords should have their own definition, they are specific and not necessarily compatible
            var positionBeforeCase = false;
            var positionOverallCase = false;

            //the compatible cases:
            //first case, we start with the position before
            if (TryConsumeAndAttachOne(ref origin, TokenNames.PositionBefore))
            {
                //I am going to rewrite the complex positionned charge grammar anyway. BUT the problem here, is that I consume and attach an item
                //if the parsing end up in failure, the item is still attached to the complex positionned charge parent
                //it's usually not a big deal UNLESS the item is an already created and valid child that used to be consumed and attached to another parent.
                //in this case, the old valid parent will still have the correct child, but the child will point to the wrong parent
                //we should try consume, and move the position (new concept) and only ATTACH if we do not return null ...
                //The bug can be reproduced using ARGENT, ON A BEND SABLE, THREE OWLS OF THE FIRST
                //on a bend is a simple positionned charge, but will be assigned to this specific complex positionned charge parent even if it fails this overall parse (I should consider attaching only at the end, when the parse is succesful for ALL parser)
                positionBeforeCase = true;
            }
            //other alternative start, we start with the position overall
            if (TryConsumeAndAttachOne(ref origin, TokenNames.PositionOverall))
            {
                positionOverallCase = true;
            }

            //third case, second case and first case second input, simple position or multi charges
            var result = TryConsumeOr(ref origin,
                TokenNames.SimplePositionnedCharges,
                TokenNames.MultiCharges);
            if (result == null)
            {
                return null;
            }

            AttachChild(result.ResultToken);

            if (positionOverallCase)
            {
                //we stop there
                return new TokenResult(CurrentToken, origin);
            }
            //more to come
            if (positionBeforeCase)
            {
                //we have an optional semicolon
                TryConsumeAndAttachOne(ref origin, TokenNames.DivisionSeparator);
            }
            else
            {
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.ComplexPositionnedChargeSeparator))
                {
                    return null;
                }
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.PositionBetween))
                {
                    return null;
                }
            }
            //the final charge
            return !TryConsumeAndAttachOne(ref origin, TokenNames.Charge)
                ? null
                : new TokenResult(CurrentToken, origin);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}