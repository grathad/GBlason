using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parse tokens that represent a division by 4 with only 2 shields definition
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.SimpleDivisionShield"/> := 
    /// <see cref="TokenNames.Shield"/>. 
    /// <see cref="TokenNames.LightSeparator"/>?
    /// <see cref="TokenNames.And"/>. 
    /// <see cref="TokenNames.Shield"/>. 
    /// </para>
    /// </summary>
    internal class SimpleDivisionShieldParser : ContainerParser
    {
        public SimpleDivisionShieldParser(IParserPilot factory = null)
            : base(TokenNames.SimpleDivisionShield, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.Shield)) { return null; }
            TryConsumeAndAttachOne(ref origin, TokenNames.LightSeparator);
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.And)) { return null; }

            //we have a very special case, that need documentation:
            //in the second shield IF we have more than one field, and we are in a chain that have a division (which should be true)
            //then, we should only attach the field out of the second shield, IF IT HAVE A SEPARATOR after it...
            //because then the separator is for the whole "division" shield and not the field of the second shield
            //example:
            //Parted per pale Or and Argent, an eagle displayed Azure armed and beaked Gules
            // FIELD (Division)            ^ The object on top of the overall shield NOT the object of the second field (starting with argent)
            //Parted per pale Or and Argent an eagle displayed Azure armed and beaked Gules
            // FIELD (division by 2), the object belong to the second shield in the division

            //now that I am thinking about it
            //we need to separate the division of the field, with the division of the charges
            //the division of the field, will only try to read the "second" shield in the simple quarter
            //as a shield and shield or as field and field
            //the problem with this is that, we actually do not necessarily pick up the smallest one, we need to be smart about the one we select
            //and for charge division I think we only support field and field ?
            //need more research
            //var lastShield = GetParser(TokenNames.Shield).TryConsume(parsedKeywords, out var sc);
            //if (lastShield == null || !lastShield.Any())
            //{
            //    return null;
            //}
            //var explorer = lastShield.First() as ContainerToken;
            ////if we contain more than a field, then we check for a separator
            //if (explorer.Children.Count() > 1 && explorer.Children.Any(c =>
            //        c.Type == TokenNames.ChargeSeparator || c.Type == TokenNames.LightSeparator))
            //{
            //    //we need to consider the field as being the whole shield.
            //}

            if (!TryConsumeAndAttachOne(ref origin, TokenNames.Shield)) { return null; }

            //if (!TryConsumeAndAttachOne(TokenNames.ChargeSeparator, parsedKeywords, cons))
            //{
            //    if (!TryConsumeAndAttachOne(TokenNames.LightSeparator, parsedKeywords, cons))
            //    {
            //        ErrorEndToSoon("separator");
            //        return null;
            //    }
            //}
            return CurrentToken.AsTokenResult(origin);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}