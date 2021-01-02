using System.Collections.Generic;
using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parser for tokens that represent a <see cref="TokenNames.SymbolSubPartGroup"/>.
    /// This grammar represent the subpart(s) of a symbol, that are autonomous, meaning that they are completed with at least the simple tincture necessary, or even other shared properties.
    /// At the smallest a group is only built from one part definition and one simple tincture
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.SymbolSubPartGroup"/> :=  
    /// <see cref="TokenNames.SubPartNameList"/>. <see cref="TokenNames.SimpleTincture"/>
    /// </para>
    /// </summary>
    /// <remarks>The tincture is potentially the responsibility of the parent, since multiple subpart can share the same, but of course can belong to the children if they have different ones</remarks>
    internal class SymbolSubPartGroupParser : ContainerParser
    {
        public SymbolSubPartGroupParser(IParserPilot factory = null)
            : base(TokenNames.SymbolSubPartGroup, factory)
        {
        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            var list = Parse(origin.Start, TokenNames.SubPartNameList);
            if (list == null) { return null; }

            var tincture = Parse(list.Position.Start, TokenNames.SimpleTincture);
            if (tincture == null) { return null; }

            AttachChildren(new List<IToken>() { list.ResultToken, tincture.ResultToken });
            origin = tincture.Position;

            return CurrentToken.AsTokenResult(origin);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}