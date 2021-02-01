using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Represent a field variation using 2 tinctures to fill the field
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="FieldVariation2Tinctures"/> := <see cref="TokenNames.FieldVariationName"/>. <see cref="TokenNames.Orientation"/>? (<see cref="TokenNames.Of"/>. <see cref="TokenNames.FieldVariationNumber"/>)?
    /// <see cref="TokenNames.Tincture"/>. <see cref="TokenNames.And"/>. <see cref="TokenNames.Tincture"/>. | <br/>
    /// <see cref="TokenNames.Tincture"/>. <see cref="TokenNames.FieldVariationName"/>. <see cref="TokenNames.Orientation"/>? <see cref="TokenNames.Tincture"/>.
    /// </para>
    /// </summary>
    internal class FieldVariation2TincturesParser : ContainerParser
    {
        public FieldVariation2TincturesParser(IParserPilot factory = null)
            : base(TokenNames.FieldVariation2Tinctures, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //2 constructs
            var result = Parse(origin, TokenNames.FieldVariationName);
            if (result == null)
            {
                //second grammar, the field variation name is after the first tincture
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.Tincture)) { return null; }
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.FieldVariationName)) { return null; }
                TryConsumeAndAttachOne(ref origin, TokenNames.Orientation);
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.Tincture)) { return null; }
            }
            else
            {
                origin = result.Position;
                //first grammar
                AttachChild(result.ResultToken);

                //potential orientation
                TryConsumeAndAttachOne(ref origin, TokenNames.Orientation);
                //potential field variation number
                if (TryConsumeAndAttachOne(ref origin, TokenNames.Of))
                {
                    TryConsumeAndAttachOne(ref origin, TokenNames.FieldVariationNumber);
                }
                //first tincture and second tincture
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.Tincture)) { return null; }
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.And)) { return null; }
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.Tincture)) { return null; }
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