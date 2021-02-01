using System;
using System.Linq;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;

namespace Grammar.English.Tokens
{
    /// <summary>
    /// Represent a token that contains all the <see cref="ParsedKeyword"/> that does not match any resource and are marked as <see cref="ParsedKeyword.NoKeyword"/>
    /// <para>
    /// <h3>Grammar:</h3>
    /// <see cref="TokenNames.SymbolName"/> := <see cref="ParsedKeyword.NoKeyword"/> (<see cref="TokenNames.SymbolNameExtension"/>? <see cref="ParsedKeyword.NoKeyword"/>)*
    /// </para>
    /// </summary>
    internal class SymbolNameParser : LeafParser
    {
        public SymbolNameParser(IParserPilot factory = null, PluginBase.Keyword.IResources resources = null)
            : base(TokenNames.SymbolName, factory, resources)
        {
        }

        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            if (origin == null)
            {
                throw new ArgumentNullException(nameof(origin));
            }
            //since the "CreateLeaf" helper change the origin value (by updating the position)
            //and we need to update it ourselves to progress through the keywords, we will use a local variable internally and update the origin on return
            var localOrigin = origin.Copy();
            //we try to read as many no keywords as we can
            var parsedKeywords = ParserPilot.GetRemainingKeywords(localOrigin.Start);
            var foundNames = parsedKeywords
                .TakeWhile(parsedKeyword => parsedKeyword.Key == ParsedKeyword.NoKeyword).ToList();
            if (!foundNames.Any())
            {
                //no result found
                return null;
            }
            localOrigin.Start += foundNames.Count;

            //then the loop between potential symbol name extensions and the following keyword
            while (localOrigin.Start < ParserPilot.LastPosition)
            {
                //getting the potential name extension
                var extension = Parse(localOrigin, TokenNames.SymbolNameExtension);
                if (extension == null)
                {
                    break;
                }
                localOrigin = extension.Position;
                //trying to get the other no key words
                var otherKeywords = ParserPilot.GetRemainingKeywords(localOrigin.Start)?
                    .TakeWhile(pK => pK.Key == ParsedKeyword.NoKeyword).ToList();
                if (!otherKeywords?.Any() ?? true)
                {
                    break;
                }
                //we consume the extension and the keywords
                foundNames.AddRange((extension.ResultToken as LeafToken)?.OriginalKw);
                foundNames.AddRange(otherKeywords);
                localOrigin.Start += otherKeywords.Count;
            }

            return CreateLeaf(origin, foundNames.ToArray());
        }
    }
}