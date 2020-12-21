using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parse tokens that represent a <see cref="TokenNames.SecondAndThirdDivisionNumber"/>
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.SecondAndThirdDivisionNumber"/> := 
    /// <see cref="TokenNames.SecondDivisionNumber"/>. <see cref="TokenNames.And"/>. <see cref="TokenNames.ThirdDivisionNumber"/>
    /// </para>
    /// </summary>
    internal class SecondAndThirdDivisionNumberParser : ContainerParser
    {
        public SecondAndThirdDivisionNumberParser(IParserPilot factory = null)
            : base(TokenNames.FirstAndFourthDivisionNumber, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.SecondDivisionNumber)) { return null; }
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.And)) { return null; }
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.ThirdDivisionNumber)) { return null; }
            return CurrentToken.AsTokenResult(origin); ;
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}