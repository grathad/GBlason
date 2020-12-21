using System.Threading.Tasks;
using Format.Elements;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parse tokens that represent a <see cref="DivisionBy2"/>
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="DivisionBy2"/> := 
    /// <see cref="TokenNames.DivisionBy2Name"/>. <see cref="TokenNames.LightSeparator"/>? <see cref="TokenNames.LineVariationDefinition"/>?
    /// (<see cref="TokenNames.SimpleDivisionBy2Field"/> | <see cref="TokenNames.SimpleDivisionShield"/> | <see cref="TokenNames.PositionnedHalves"/>)
    /// </para>
    /// </summary>
    internal class DivisionBy2Parser : ContainerParser
    {
        public DivisionBy2Parser(IParserPilot factory = null)
            : base(TokenNames.DivisionBy2, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.DivisionBy2Name)) { return null; }
            TryConsumeAndAttachOne(ref origin, TokenNames.LightSeparator);
            TryConsumeAndAttachOne(ref origin, TokenNames.LineVariationDefinition);
            //either a simple quarter or a positionned halves. This is not a regular OR this is priority based.
            //Because the simple division by 2 will always consume more than the simple division by 2 fields (a shield is always a field + something)
            //but if we match a simple division by 2 we pick it as priority

            if (!TryConsumeAndAttachOne(ref origin, TokenNames.SimpleDivisionBy2Field))
            {
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.SimpleDivisionShield))
                {
                    //have to be a positionned halves or not a valid division by 2
                    //though with the mandatory name, it is likely an error
                    if (!TryConsumeAndAttachOne(ref origin, TokenNames.PositionnedHalves))
                    {
                        //ErrorWrongKeyword(new FoundExpected(origin.ToList(), "a simple quarter or 2 positionned halves"));
                        return null;
                    }
                }
            }
            return new TokenResult(CurrentToken, origin);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}