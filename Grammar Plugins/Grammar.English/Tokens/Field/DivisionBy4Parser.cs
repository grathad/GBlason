using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parse tokens that represent a <see cref="DivisionBy4"/>
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="DivisionBy4"/> := 
    /// <see cref="TokenNames.DivisionBy4Name"/>. <see cref="TokenNames.DivisionBy4Separator"/>? <see cref="TokenNames.LineVariationDefinition"/>?
    /// (<see cref="TokenNames.SimpleDivisionBy2Field"/> | <see cref="TokenNames.SimpleDivisionShield"/> | <see cref="TokenNames.PositionnedQuarters"/>)
    /// </para>
    /// </summary>
    internal class DivisionBy4Parser : ContainerParser
    {
        public DivisionBy4Parser(IParserPilot factory = null)
            : base(TokenNames.DivisionBy4, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.DivisionBy4Name)) { return null; }
            TryConsumeAndAttachOne(ref origin, TokenNames.DivisionBy4Separator);
            TryConsumeAndAttachOne(ref origin, TokenNames.LineVariationDefinition);
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.SimpleDivisionBy2Field))
            {
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.SimpleDivisionShield))
                {
                    if (!TryConsumeAndAttachOne(ref origin, TokenNames.PositionnedQuarters))
                    {
                        return null;
                    }
                }
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