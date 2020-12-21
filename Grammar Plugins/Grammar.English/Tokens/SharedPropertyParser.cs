using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// This represent a single property that can be applied to an element (usually a charge, but not only)
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.SharedProperty"/> := 
    /// <see cref="TokenNames.Direction"/> | 
    /// <see cref="TokenNames.Location"/>
    /// </para>
    /// </summary>
    /// <example>
    /// <h4>Location</h4>
    /// two cinquefoils <b>in chief</b>
    /// <h4>Direction</h4>
    /// azure three mullets or, <b>in pale</b>
    /// Fusilly <b>bendwise</b> argent and azure.
    /// a broken spear <b>bendways</b> between two mullets Azure
    /// </example>
    internal class SharedPropertyParser : ContainerParser
    {
        public SharedPropertyParser(IParserPilot factory = null) 
            : base(TokenNames.SharedProperty, factory) { }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            var result = TryConsumeOr(ref origin,
                TokenNames.Direction,
                TokenNames.Location);

            if (result == null)
            {
                return null;
            }
            AttachChild(result.ResultToken);
            return CurrentToken.AsTokenResult(result);
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}