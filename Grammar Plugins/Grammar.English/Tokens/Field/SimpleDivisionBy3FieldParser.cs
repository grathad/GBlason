using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parse tokens that represent a <see cref="TokenNames.SimpleDivisionBy3Field"/>.
    /// <para>
    /// A simple division by 2 fields, is the representation of most (if not all) of division by 2 where the 2 elements define the actual subfields, and not shields
    /// Like: Argent and Sable, A charge. Where the charge is not part of the field, and thus not part of the division
    /// </para>
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.SimpleDivisionBy3Field"/> := 
    /// (<see cref="TokenNames.Tincture"/> | <see cref="TokenNames.FieldVariation"/>)
    /// <see cref="TokenNames.LightSeparator"/>.
    /// (<see cref="TokenNames.Tincture"/> | <see cref="TokenNames.FieldVariation"/>)
    /// <see cref="TokenNames.And"/>.
    /// (<see cref="TokenNames.Tincture"/> | (<see cref="TokenNames.FieldVariation"/>)
    /// </para>
    /// </summary>
    internal class SimpleDivisionBy3FieldParser : SimpleDivisionParser
    {
        public SimpleDivisionBy3FieldParser(IParserPilot factory = null)
            : base(TokenNames.SimpleDivisionBy2Field, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            if (!TinctureOrFieldVariation(ref origin)) { return null; }
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.LightSeparator)) { return null; }
            if (!TinctureOrFieldVariation(ref origin)) { return null; }
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.And)) { return null; }
            if (!TinctureOrFieldVariation(ref origin)) { return null; }

            return CurrentToken.AsTokenResult(origin); ;
        }
    }
}