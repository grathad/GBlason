using System.Threading.Tasks;
using Format.Elements;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Represent a <see cref="Division"/> of a <see cref="Field"/>
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="Division"/> := <see cref="DivisionBy2"/> 
    /// | <see cref="DivisionBy3"/> | 
    /// <see cref="DivisionBy4"/>
    /// </para>
    /// </summary>
    internal class DivisionParser : ContainerParser
    {
        public DivisionParser(IParserPilot factory = null)
            : base(TokenNames.Division, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            var result = TryConsumeOr(ref origin,
                TokenNames.DivisionBy2,
                TokenNames.DivisionBy3,
                TokenNames.DivisionBy4);
            if (result != null)
            {
                AttachChild(result.ResultToken);
            }

            return CurrentToken.AsTokenResult(result);
        }
        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}