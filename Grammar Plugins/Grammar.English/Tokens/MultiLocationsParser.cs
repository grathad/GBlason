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
    /// (In | On) The? (locationpoint | locationflank) And In? (locationpoint | locationflank)	
    /// </para>
    /// </summary>
    internal class MultiLocationsParser : ContainerParser
    {
        public MultiLocationsParser(IParserPilot factory = null)
            : base(TokenNames.MultiLocations, factory)
        {
        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //not implemented
            return null;
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}