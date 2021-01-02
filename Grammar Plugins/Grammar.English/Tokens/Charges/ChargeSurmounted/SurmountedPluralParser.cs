using System;
using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// This represent multiple charges that are listed together, and optionally share a property
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.SurmountedPlural"/> := 
    /// SURMOUNTEDPOSSIBLEFIRSTPLURALGROUP EACHKEYWORD SUMOUNTEDKEYWORD SURMOUNTEDPOSSIBLESECONDGROUP
    /// <see cref="TokenNames.SurmountedPossibleFirstPluralGroup"/> <see cref="TokenNames.Each"/> <see cref="TokenNames.Surmounted"/> <see cref="TokenNames.SurmountedPossibleSecondGroup"/>
    /// </para>
    /// </summary>
    /// <example>
    /// <i>a chevron between </i>two cinquefoils in chief <b>and</b> a hunting-horn in base Or
    /// </example>
    internal class SurmountedPluralParser : ContainerParser
    {
        public SurmountedPluralParser(IParserPilot factory = null)
            : base(TokenNames.SurmountedPlural, factory)
        {

        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            throw new NotImplementedException();
        }


        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}