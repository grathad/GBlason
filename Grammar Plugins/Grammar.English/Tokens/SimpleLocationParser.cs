using System.Threading.Tasks;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Try to parse the <see cref="ParsedKeyword"/> into a <see cref="TokenNames.Location"/>
    /// A location is an absolute definition of a placement of an object. Relative location are parsed under <see cref="TokenNames.PositionnedCharges"/>
    /// <para>
    /// <h3>Grammar:</h3> 
    /// <see cref="TokenNames.SimpleLocation"/> := <see cref="TokenNames.LocationBefore"/> <br/>
    /// (<see cref="TokenNames.Determiner"/>? <see cref="TokenNames.LocationPoint"/>. <see cref="TokenNames.Point"/>? | <br/>
    /// (<see cref="TokenNames.LocationSpecifier"/> | <see cref="TokenNames.ShieldSide"/>) <see cref="TokenNames.LocationFlank"/>)
    /// </para>
    /// </summary>
    internal class SimpleLocationParser : ContainerParser
    {
        public SimpleLocationParser(IParserPilot factory = null)
            : base(TokenNames.SimpleLocation, factory)
        {
        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.LocationBefore)) { return null; }
            //then 2 options
            //the first one potential determiner then location point then potential point
            //todo replace the determiner with something more accurate. The whole location concept might need rework when more samples are available
            var determinerResult = Parse(origin, TokenNames.Determiner);
            var pointResult = Parse(determinerResult?.Position ?? origin, TokenNames.LocationPoint);

            if (pointResult != null)
            {
                if (determinerResult != null)
                {
                    AttachChild(determinerResult.ResultToken);
                }
                AttachChild(pointResult.ResultToken);
                origin = pointResult.Position;

                TryConsumeAndAttachOne(ref origin, TokenNames.Point);
            }
            else
            {
                //not yet supported, but will need definition and potential rewrite of the grammar (and samples)
                return null;
                //not the first option, so we try the second case, we either start with the location specified or the shield side
                //if (!TryConsumeAndAttachOne(TokenNames.LocationSpecifier, parsedKeywords, cons))
                //{

                //    //trying the second option ?
                //    if (!TryConsumeAndAttachOne(TokenNames.ShieldSide, parsedKeywords, cons))
                //    {
                //        return null;
                //    }
                //}
                //if (!TryConsumeAndAttachOne(TokenNames.LocationFlank, parsedKeywords, cons))
                //{
                //    return null;
                //}
            }
            return CurrentToken.AsTokenResult(origin);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}