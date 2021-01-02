using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Parser for tokens that represent a <see cref="TokenNames.SubPartName"/>
    /// <para>
    /// The current definition for a subpartname is a non keyword word, that is finishing by "ed" (in the english grammar)
    /// </para>
    /// </summary>
    internal class SubPartNameParser : LeafParser
    {
        public SubPartNameParser(IParserPilot factory = null)
            : base(TokenNames.SubPartName, factory)
        {
        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            //we check if the keyword at the position is of the correct type (no key word) as well as finishing by "ed"
            var nextkw = ParserPilot.GetRemainingKeywords(origin.Start).FirstOrDefault();
            if (nextkw != null
                && nextkw.Key == ParsedKeyword.NoKeyword
                && nextkw.Value.TrimEnd().ToUpperInvariant().EndsWith("ED"))
            {
                //we found a match
                var leaf = CreateLeaf(origin, new[] { nextkw });
                origin = leaf.Position;
                return leaf;
            }
            return null;
        }

        /// <inheritdoc/>
        public override Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            throw new System.NotImplementedException();
        }
    }
}