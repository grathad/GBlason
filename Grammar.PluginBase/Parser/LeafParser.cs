using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Grammar.PluginBase.Token.Contracts;
using Utils.LinqHelper;

namespace Grammar.PluginBase.Parser
{
    /// <summary>
    /// 
    /// </summary>
    public class LeafParser : ParserBase, ILeafParser
    {
        /// <summary>
        /// 
        /// </summary>
        public LeafToken CurrentToken { get; }

        /// <summary>
        /// 
        /// </summary>
        public IResources Resources { get; }

        /// <summary>
        /// Create a new leaf parser, used to extract a token as a final leaf in our tree of tokens.
        /// A leaf token does not contains token children but a list of parsed keywords that are used to create the leaf
        /// </summary>
        /// <param name="type">The type of token that this leaf represent</param>
        /// <param name="factory">[Optional] The injected factory used to pilot and optimize the parsing <see cref="ParserPilot"/></param>
        /// <param name="resources">[Optional] The resource contract to get the information on the key words and term to use in parsing the leaf. Default to <see cref="Keyword.Resources"/> when not specified</param>
        public LeafParser(TokenNames type, IParserPilot factory = null, IResources resources = null) :
            base(type, factory)
        {
            CurrentToken = new LeafToken { Type = type };
            Resources = resources;
        }

        /// <summary>
        /// Generic way of creating a leaf token while reading a leaf from the keywords
        /// </summary>
        /// <param name="original"></param>
        /// <param name="keywordsUsed"></param>
        /// <returns></returns>
        protected ITokenResult CreateLeaf(ITokenParsingPosition original, params ParsedKeyword[] keywordsUsed)
        {
            if (keywordsUsed == null || !keywordsUsed.Any())
            {
                return CurrentToken.AsTokenResult(original);
            }
            var newPosition = original.Copy();
            foreach (var parsedKeyword in keywordsUsed.Where(kw => kw != null))
            {
                CurrentToken.OriginalKw.Add(parsedKeyword);
                newPosition.Start++;
            }
            
            return CurrentToken.AsTokenResult(newPosition);
        }

        /// <summary>
        /// Attempt to consume a leaf, by comparing the keyword of the current position defined in the <paramref name="origin"/> against the list of available keywords defined in <see cref="Resources"/>
        /// </summary>
        /// <param name="origin">The information about the current location we are trying to parse</param>
        /// <returns>A task holding the token result with either a succesful consumption (that moved the position) or a failed one (with the explanation of the failure)</returns>
        public override async Task<ITokenResult> TryConsumeAsync(ITokenParsingPosition origin)
        {
            List<IEnumerable<string>> allKeyWords;
            try
            {
                allKeyWords = Resources.GetTokens(CurrentToken.Type).ToList();
            }
            catch (Exception)
            {
                ErrorNoTokenKeywords(origin.Start);
                return null;
            }

            var consumed = await Task.Run(() => FindMatchingKeywords(origin.Start, allKeyWords));

            return CreateLeaf(origin, consumed.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override ITokenResult TryConsume(ref ITokenParsingPosition origin)
        {
            var allKeyWords = GetAllRemainingKeywords(origin.Start);
            if(allKeyWords == null || !allKeyWords.Any())
            {
                return null;
            }

            var consumed = FindMatchingKeywords(origin.Start, allKeyWords);
            if (consumed == null)
            {
                //since we don't do "is" anymore we don't trigger grammar errors The logic for errors will need to change as well
                //ErrorWrongKeyword(new FoundExpected(kw.Take(maxLength), allKeyWords));
                ErrorNoOptionFound(origin.Start);
                return null;
            }

            var leaf = CreateLeaf(origin, consumed.ToArray());
            origin = leaf.Position;
            return leaf;
        }

        protected List<IEnumerable<string>> GetAllRemainingKeywords(int origin)
        {
            if(origin < 0 || CurrentToken == null)
            {
                return null;
            }
            List<IEnumerable<string>> allKeyWords;
            try
            {
                allKeyWords = Resources.GetTokens(CurrentToken.Type).ToList();
            }
            catch (Exception)
            {
                Trace.TraceInformation($"{origin} Impossible to get tokens for the type {CurrentToken.Type}");
                ErrorNoTokenKeywords(origin);
                return null;
            }
            return allKeyWords;
        }

        protected List<ParsedKeyword> FindMatchingKeywords(int origin, List<IEnumerable<string>> allKeyWords)
        {
            //Figure out if we have a match (we only take the potential match that have less or as many words as the rest of the blazon to parse)
            var kw = ParserPilot.GetRemainingKeywords(origin);
            var maxLength = kw.Count;
            var potentialMatches = new List<List<ParsedKeyword>>();
            foreach (var keyword in allKeyWords.Where(k => k.Count() <= maxLength))
            {
                var potentialMatch = keyword as IList<string> ?? keyword.ToList();
                var wordToCheck = new Stack<ParsedKeyword>(kw.Take(potentialMatch.Count).Reverse().ToList());
                var wordToReturn = new List<ParsedKeyword>();

                //we check if the words in the reality match the order in the potential match
                wordToReturn.AddRange(potentialMatch
                    .TakeWhile(word => string.Equals(word, wordToCheck.Peek().Key, StringComparison.OrdinalIgnoreCase))
                    .Select(word => wordToCheck.Pop()));
                if (wordToReturn.Count == potentialMatch.Count)
                {
                    potentialMatches.Add(wordToReturn);
                }
            }
            if (!potentialMatches.Any())
            {
                //we don't have any match
                return null;
            }
            var consumed = potentialMatches.GetMaxElement(list => list.Count);
            return consumed;
        }

        public bool Exist(int position)
        {
            var allKeyWords = GetAllRemainingKeywords(position);
            if (allKeyWords == null || !allKeyWords.Any())
            {
                return false;
            }
            return FindMatchingKeywords(position, allKeyWords) != null;
        }

        protected override ITokenResult End()
        {
            throw new NotImplementedException();
        }
    }
}