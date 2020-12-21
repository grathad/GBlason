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
    /// <see cref="TokenNames.Location"/> := <see cref="TokenNames.MultiLocations"/> | <see cref="TokenNames.SimpleLocation"/>
    /// </para>
    /// </summary>
    internal class LocationParser : ContainerParser
    {
        public LocationParser(IParserPilot factory = null) 
            : base(TokenNames.Location, factory)
        {
        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            var result = TryConsumeOr(ref origin,
                TokenNames.MultiLocations,
                TokenNames.SimpleLocation);
            if (result == null)
            {
                return null;
            }
            AttachChild(result.ResultToken);
            return CurrentToken.AsTokenResult(result);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}