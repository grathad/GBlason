using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parse a tokens that represent a division by 4 quarter that start with a number
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.PositionnedQuarters"/> := <br/>
    /// (<see cref="TokenNames.FirstDivisionNumber"/>. <see cref="TokenNames.Quarter"/>? <see cref="TokenNames.Shield"/>. <see cref="TokenNames.ChargeSeparator"/>.
    /// <see cref="TokenNames.SecondDivisionNumber"/>. <see cref="TokenNames.Quarter"/>? <see cref="TokenNames.Shield"/>. <see cref="TokenNames.ChargeSeparator"/>.
    /// <see cref="TokenNames.ThirdDivisionNumber"/>. <see cref="TokenNames.Quarter"/>? <see cref="TokenNames.Shield"/>. <see cref="TokenNames.ChargeSeparator"/>.
    /// <see cref="TokenNames.FourthDivisionNumber"/>. <see cref="TokenNames.Quarter"/>? <see cref="TokenNames.Shield"/>. <see cref="TokenNames.Separator"/>?)
    /// |<br/>
    /// (<see cref="TokenNames.FirstAndFourthDivisionNumber"/>. <see cref="TokenNames.Quarter"/>? <see cref="TokenNames.Shield"/>. <see cref="TokenNames.ChargeSeparator"/>.
    /// <see cref="TokenNames.SecondAndThirdDivisionNumber"/>. <see cref="TokenNames.Quarter"/>? <see cref="TokenNames.Shield"/>. <see cref="TokenNames.Separator"/>?)
    /// |<br/>
    /// (<see cref="TokenNames.FirstAndFourthDivisionNumber"/>. <see cref="TokenNames.Quarter"/>? <see cref="TokenNames.Shield"/>. <see cref="TokenNames.ChargeSeparator"/>.
    /// <see cref="TokenNames.SecondDivisionNumber"/>. <see cref="TokenNames.Quarter"/>? <see cref="TokenNames.Shield"/>. <see cref="TokenNames.ChargeSeparator"/>.
    /// <see cref="TokenNames.ThirdDivisionNumber"/>. <see cref="TokenNames.Quarter"/>? <see cref="TokenNames.Shield"/>. <see cref="TokenNames.Separator"/>?)
    /// |<br/>
    /// (<see cref="TokenNames.FirstDivisionNumber"/>. <see cref="TokenNames.Quarter"/>? <see cref="TokenNames.Shield"/>. <see cref="TokenNames.ChargeSeparator"/>.
    /// <see cref="TokenNames.SecondAndThirdDivisionNumber"/>. <see cref="TokenNames.Quarter"/>? <see cref="TokenNames.Shield"/>. <see cref="TokenNames.ChargeSeparator"/>.
    /// <see cref="TokenNames.FourthDivisionNumber"/>. <see cref="TokenNames.Quarter"/>? <see cref="TokenNames.Shield"/>. <see cref="TokenNames.Separator"/>?)
    /// </para>
    /// </summary>
    internal class PositionnedQuartersParser : ContainerParser
    {
        public PositionnedQuartersParser(IParserPilot factory = null)
            : base(TokenNames.PositionnedQuarters, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            var expectedRemainingPosition = 4;
            //trying to see if we start with the first division number (2 cases would be working this way)
            //ok so the "first" keyword is included in the "first and fourth" keyword, so I need to check for the most complete first, and then fallback to the smaller one
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.FirstAndFourthDivisionNumber))
            {
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.FirstDivisionNumber))
                {
                    //no good start
                    return null;
                }
                expectedRemainingPosition -= 1;
            }
            else
            {
                expectedRemainingPosition -= 2;
            }

            TryConsumeAndAttachOne(ref origin, TokenNames.Quarter);
            //ok so we have the first position, now we should get a shield
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.Shield)) { return null; }
            //and a charge separator
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.ChargeSeparator)) { return null; }

            //now it is either the second or the second and third definition
            //trying to see if we start with the first division number (2 cases would be working this way)
            //same remark than for the first and fourth, the second is included in second and third, so we start to try to read the longest option
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.SecondAndThirdDivisionNumber))
            {
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.SecondDivisionNumber))
                {
                    //no good follow up identification
                    return null;
                }
                expectedRemainingPosition -= 1;
            }
            else
            {
                expectedRemainingPosition -= 2;
            }
            TryConsumeAndAttachOne(ref origin, TokenNames.Quarter);
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.Shield)) { return null; }
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.ChargeSeparator) &&
                expectedRemainingPosition != 0) { return null; }

            if (expectedRemainingPosition == 0)
            {
                return CurrentToken.AsTokenResult(origin);
            }

            //here we expect the third or the fourth
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.ThirdDivisionNumber))
            {
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.FourthDivisionNumber))
                {
                    //no good follow up identification
                    return null;
                }
            }
            //both return -1 shield
            expectedRemainingPosition -= 1;

            TryConsumeAndAttachOne(ref origin, TokenNames.Quarter);
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.Shield)) { return null; }

            if (!TryConsumeAndAttachOne(ref origin, TokenNames.ChargeSeparator) &&
                expectedRemainingPosition != 0) { return null; }

            if (expectedRemainingPosition == 0)
            {
                return CurrentToken.AsTokenResult(origin); ;
            }
            //finally the fourth division
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.FourthDivisionNumber))
            {
                return null;
            }

            TryConsumeAndAttachOne(ref origin, TokenNames.Quarter);
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.Shield)) { return null; }

            TryConsumeAndAttachOne(ref origin, TokenNames.Separator);

            return CurrentToken.AsTokenResult(origin); ;
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}