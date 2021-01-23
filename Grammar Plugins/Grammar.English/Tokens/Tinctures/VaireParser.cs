using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parse tokens that represent a vaire (a vair definition where the tincture are defined)
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.Vaire"/> := 
    /// <see cref="TokenNames.Counter"/>? <see cref="TokenNames.VaireName"/> (<see cref="TokenNames.SymbolStateDeterminer"/>? <see cref="TokenNames.FurOrientationName"/>)?
    /// <see cref="TokenNames.SimpleTincture"/> <see cref="TokenNames.And"/> <see cref="TokenNames.SimpleTincture"/> | <br/>
    /// <see cref="TokenNames.SimpleTincture"/> <see cref="TokenNames.VaireBetweenName"/> <see cref="TokenNames.SimpleTincture"/>
    /// </para>
    /// </summary>
    internal class VaireParser : ContainerParser
    {
        public VaireParser(IParserPilot factory = null)
            : base(TokenNames.Vaire, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //first case: tincture and vaire name for between assignment
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.SimpleTincture))
            {
                //we are either in the second case, or not in a vaire
                TryConsumeAndAttachOne(ref origin, TokenNames.Counter);
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.VaireName))
                {
                    return null;
                }
                var resultDeterminer = Parse(origin, TokenNames.SymbolStateDeterminer);
                var resultOrientation = Parse(resultDeterminer?.Position ?? origin, TokenNames.FurOrientationName);
                if (resultOrientation != null)
                {
                    if (resultDeterminer != null)
                    {
                        AttachChild(resultDeterminer.ResultToken);
                    }
                    AttachChild(resultOrientation.ResultToken);
                    origin = resultOrientation.Position;
                }
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.SimpleTincture)) { return null; }
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.And)) { return null; }
            }
            else
            {
                //we are in the first case, now consuming the name between, or not in a vaire
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.VaireBetweenName)) { return null; }
            }
            //shared with both cases
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.SimpleTincture)) { return null; }
            
            return CurrentToken.AsTokenResult(origin);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}