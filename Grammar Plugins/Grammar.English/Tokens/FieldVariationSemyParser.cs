using System.Threading.Tasks;
using Format.Elements;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parse a list of token and try to generate a <see cref="FieldVariationSemy"/> token
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="FieldVariationSemy"/> := 
    /// <see cref="TokenNames.SimpleTincture"/> 
    /// ((<see cref="TokenNames.SemyDeterminer"/>? <see cref="TokenNames.Semy"/>. <see cref="TokenNames.SemyCharge"/>) | <see cref="TokenNames.SemyName"/>)
    /// <see cref="TokenNames.SimpleTincture"/>?
    /// </para>
    /// </summary>
    /// <remarks>
    /// The last simple tincture is optional because some complex symbol might include the tincture within their extensive name.
    /// But then the format will fail since the last tincture need definition, the best would be to review if it is not possible to have the last tincture as part of the simple charge (which it should be)
    /// </remarks>
    internal class FieldVariationSemyParser : ContainerParser
    {
        public FieldVariationSemyParser(IParserPilot factory = null)
            : base(TokenNames.FieldVariationSemy, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.SimpleTincture)) { return null; }

            //trying the simplest, the semy name
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.SemyName))
            {
                //then it have to be the semy determiner and simple charge
                TryConsumeAndAttachOne(ref origin, TokenNames.SemyDeterminer);
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.Semy))
                {
                    //no match
                    return null;
                }
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.SemyCharge))
                {
                    //no match
                    return null;
                }
            }
            //to review likely need to be attached to the charge unless the semy name needs it
            TryConsumeAndAttachOne(ref origin, TokenNames.SimpleTincture);
            return CurrentToken.AsTokenResult(origin);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }

}