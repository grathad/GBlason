using System;
using System.Threading.Tasks;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Abstract class to share tools for all the logic across all the different possible division of the field (like <see cref="SimpleDivisionBy2FieldParser"/>)
    /// </summary>
    internal class SimpleDivisionParser : ContainerParser
    {
        protected SimpleDivisionParser(TokenNames token, IParserPilot factory = null)
            : base(token, factory) { }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            throw new NotSupportedException();
        }

        protected bool TinctureOrFieldVariation(ref ITokenParsingPosition origin)
        {
            if (!TryConsumeAndAttachOne(ref origin, TokenNames.Tincture))
            {
                if (!TryConsumeAndAttachOne(ref origin, TokenNames.FieldVariation))
                {
                    return false;
                }
            }
            return true;
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}