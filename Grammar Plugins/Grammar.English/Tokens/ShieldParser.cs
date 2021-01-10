using System.Threading.Tasks;
using Format.Elements;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// This represent the definition of a shield.
    /// This is one of the rare case when a container is clearly defined and the rules are respected
    /// <para>
    /// <h3>Grammar: </h3>
    /// <see cref="Shield"/> := <see cref="Field"/>. |
    /// <see cref="Field"/>. <see cref="TokenNames.FieldSeparator"/>! (<see cref="TokenNames.Charge"/>)+
    /// <see cref="TokenNames.AllCounterChanged"/>? Cadency?
    /// </para>
    /// </summary>
    /// <remarks>
    /// The light separator after the field is not expected if the field is the only element in the shield, but should be present if there are any object following.
    /// <br/>
    /// The counterchanged is just a tincture applied to everything might be changed with a refactored tincture of sort for parsing purpose
    /// </remarks>
    internal class ShieldParser : ContainerParser
    {
        public ShieldParser(IParserPilot pilot = null)
            : base(TokenNames.Shield, pilot)
        { }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //mandatory field
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.Field))
            {
                ErrorMandatoryTokenMissing(TokenNames.Field, origin.Start);
                return null;
            }

            //expected separator, but ok - ish if not present
            var separatorPresent = TryConsumeAndAttachOne(ref origin, TokenNames.FieldSeparator);

            //then there are optional charges on the field
            //if there is more than one charge, this will be handled in the grammar for complex charges (like list or positionned ones)
            if (TryConsumeAndAttachOne(ref origin, TokenNames.Charge))
            {
                if (!separatorPresent)
                {
                    //we did not found any separator even if there were a valid charge after the field
                    ErrorOptionalTokenMissing(TokenNames.FieldSeparator, origin.Start);
                }
            }
            TryConsumeAndAttachOne(ref origin, TokenNames.AllCounterChanged);
            return CurrentToken.AsTokenResult(origin);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}
