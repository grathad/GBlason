using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Represent a field attached as the first definition of a shield
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="Field"/> := <see cref="Tincture"/> | <see cref="Division"/> | <see cref="FieldVariation"/>
    /// </para>
    /// </summary>
    internal class FieldParser : ContainerParser
    {
        public FieldParser(IParserPilot factory = null) : base(TokenNames.Field, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //Generic Or grammar
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}
