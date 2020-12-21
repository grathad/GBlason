

using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// This represent a charge that is composed by multiple ordinaries (usually sharing properties) it can be a list of names of ordinaries
    /// </summary>
    internal class MultiOrdinaryParser : ContainerParser
    {
        public MultiOrdinaryParser(IParserPilot factory = null) 
            : base(TokenNames.MultiOrdinary, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //throw new NotImplementedException();
            return null;
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}