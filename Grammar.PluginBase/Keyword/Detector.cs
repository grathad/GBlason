using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Utils.LinqHelper;

namespace Grammar.PluginBase.Keyword
{
    /// <inheritdoc />
    /// <summary>
    /// This is a helper to detect all the keywords from the format.
    /// This, in the default architecture, is considered the first step of the full format construction process.
    /// <para>The scanning of all the words in the original input, into related keywords</para>
    /// </summary>
    public class Detector : IDetector, IBenchmarker
    {

        //private static readonly char[] KeywordSeparators = { ',', ';', '.', ':' };
        private static readonly char[] Separators = { ' ', '-', '\t', '\n', '\r', ',', ';', '.', ':' };
        private static readonly char[] WordNeutral = { ' ', '-', '\t', '\n', '\r' };

        internal List<ParserError> Errors { get; }
        public Stopwatch BenchmarkingWatch { get; set; } = new Stopwatch();

        internal readonly IResources Resources;

        internal readonly List<string> AllMergeableWords;
        internal readonly Dictionary<string, IEnumerable<string>> AllKeywords;

        /// <summary>
        /// Create a new instance of detector that will be used to run the initial step of parsing (extract every word as a keyword
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="languageResource"></param>
        public Detector(List<ParserError> errors, IResources languageResource)
        {
            Errors = errors;
            Resources = languageResource;
            //initialization, flattening the resources for reading purpose
            AllMergeableWords = Resources?.GetMergeableWords();
            //loading the resources, since the plugin is strongly cultured, we can pick up a specific resource
            AllKeywords = Resources?.GetKeywords();
        }

        /// <inheritdoc cref="IDetector.DetectKeywords"/>
        public IEnumerable<ParsedKeyword> DetectKeywords(Stream blazon, Encoding encoding = null)
        {
            BenchmarkingWatch.Start();
            if (blazon == null)
            {
                BenchmarkingWatch.Stop();
                throw new ArgumentNullException(nameof(blazon));
            }
            if (!(AllKeywords?.Any() ?? false))
            {
                BenchmarkingWatch.Stop();
                throw new ArgumentException("No key word to compare to the input", nameof(AllKeywords));
            }
            if (blazon.Length < 1)
            {
                BenchmarkingWatch.Stop();
                return Array.Empty<ParsedKeyword>();
            }
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }


            //the list of candidates that have been found so far
            var currentMatches = new List<KeyWordMatch>();
            //all the terms flattened
            var allTerms = AllKeywords.Values.SelectMany(v => v).ToList();
            //there might be reason to not ignore upper vs lower case, but by experience the input is not forced either way
            //var hashSet = new HashSet<string>(allTerms, StringComparer.OrdinalIgnoreCase);
            //the longest term for optimization (we dopn't want to consider key longer than that)
            var longestTerm = allTerms.Max(s => s.Length);


            //the position logic management in byte number (some characters need more than a byte)
            var lastBytePosition = 0;
            //the position management in character numbers
            var lastCharacterPosition = 0;
            //the final results of this parsing
            var results = new List<ParsedKeyword>();

            //the source of the text to parse
            Stream sourceCopy = new MemoryStream();
            blazon.CopyTo(sourceCopy);
            sourceCopy.Position = 0;

            using (var reader = new StreamReader(sourceCopy, encoding))
            {
                var rawWord = string.Empty;
                //consuming the words
                while (true)
                {
                    //rather than trying to read byte by byte, we rely on a stream reader with the appropriate encoding
                    rawWord += (char)reader.Read();

                    //checking if we match one keyword term
                    //we try to consume the longest possible option, so to know if we found something, we need to continue until the max length
                    var currentWord = rawWord;
                    //var currentWord = rawWord.Trim();
                    //we do 2 checks, one with a trim before for separators, and one without, the priority is for the raw (without trim)
                    var match = TryGetKvp(currentWord, rawWord);
                    if (match != null)
                    {
                        currentMatches.Add(match);
                    }
                    //trimming
                    else
                    {
                        //we don't want to consume actual potential keywords like , ; and :
                        currentWord = currentWord.TrimStart(WordNeutral);
                        match = TryGetKvp(currentWord, rawWord);
                        if (match != null)
                        {
                            currentMatches.Add(match);
                        }
                    }

                    //we stop if we reached the maximum allowed length, or the end of the stream
                    if (rawWord.Length >= longestTerm || reader.EndOfStream)
                    {
                        //if we have any match we try to find the best (the logic is smart enough to know if it is a wildcard or an actual result)
                        //this is a split the other function share the same fields ...
                        //if there is a best match, we completed and we have ourselves a find ! that we can add to the results
                        var bestMatch = GetBestMatch(rawWord, currentMatches, AllMergeableWords, out var raw);

                        //reinitializing all the counters
                        currentMatches.Clear();
                        rawWord = "";

                        if (bestMatch != null)
                        {
                            //we need to update the position (since the current one is based relatively on the word, and not on the full stream)
                            bestMatch.StartLocation += lastCharacterPosition;
                            lastCharacterPosition = bestMatch.StartLocation + (bestMatch.Value?.Length ?? 0);
                            //we move back the position to the end of the finding
                            //calculate the position !!! > We need to include potential starting space character(s)
                            //Which are not part of the best match since it is trimed ...
                            var position = lastBytePosition + encoding.GetByteCount(raw ?? string.Empty);
                            sourceCopy.Position = position;
                            reader.DiscardBufferedData();
                            results.Add(bestMatch);

                            lastBytePosition = (int)sourceCopy.Position;
                        }

                        if (sourceCopy.Position >= sourceCopy.Length)
                        {
                            break;
                        }
                    }
                }
            }
            BenchmarkingWatch.Stop();
            //we need to assign the keys now
            return results;
        }

        /// <summary>
        /// Try to find a valid key value pair from all the key words (<see cref="AllKeywords"/>) which value is equal to the given <paramref name="word"/>
        /// </summary>
        /// <param name="word">The word to look for in the list of key value pair</param>
        /// <param name="raw">The actual raw binary input for memory purpose</param>
        /// <returns>The found keyword match if something is found otherwise null</returns>
        internal virtual KeyWordMatch TryGetKvp(string word, string raw)
        {
            if (string.IsNullOrEmpty(word))
            {
                return null;
            }
            if (!(AllKeywords?.Any() ?? false))
            {
                return null;
            }
            return
                AllKeywords.Keys
                    .Where(k => AllKeywords[k]
                        .Any(value => string.Equals(value, word, StringComparison.OrdinalIgnoreCase)))
                    .Select(s => new KeyWordMatch(s, word, raw))
                    .FirstOrDefault();
        }

        /// <summary>
        /// For the list of given choices, find the best match
        /// <remarks>
        /// this is very important as this logic decide when a word is actually a known keyword or when this is a "wildcard" 
        /// or a word that can be anything and need proper interpretation later down the parsing
        /// </remarks>
        /// </summary>
        /// <param name="originalText">The characters that are used to find all the matches</param>
        /// <param name="currentMatches">The current matches to find the best match from</param>
        /// <param name="allMergeableWords">The list of all mergeable words available in the resources</param>
        /// <param name="raw">The text without any trim used to find the keyword</param>
        /// <returns>The keyword that is the best match, or null if none are available</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="originalText"/> is null</exception>
        internal virtual ParsedKeyword GetBestMatch(
            string originalText,
            IEnumerable<KeyWordMatch> currentMatches,
            ICollection<string> allMergeableWords,
            out string raw)
        {
            //3
            if (string.IsNullOrEmpty(originalText))
            {
                throw new ArgumentNullException(nameof(originalText));
            }
            if (currentMatches == null)
            {
                throw new ArgumentNullException(nameof(currentMatches));
            }

            raw = string.Empty;
            var enumeratedMatches = currentMatches.ToList();


            //so there are 3 possible output here
            //1. The best actual match when a keyword is valid and thus returned (top priority)
            //2. A wildcard (the first word in the current reading) if there is no valid match
            //3. Null if there is nothing in the current reading buffer - which is handled as the input validation level
            if (enumeratedMatches.Any())
            {

                var validMatches = new List<KeyWordMatch>();
                var startPosition = 0;

                //1. is there any valid key words in the matches ?
                foreach (var match in enumeratedMatches)
                {
                    //so we check if the match is a full word (a separator after it)
                    //because we trim start the start of the word is not always 0
                    startPosition = originalText.IndexOf(match.Value, StringComparison.OrdinalIgnoreCase);
                    var nextCharPosition = startPosition + match.Value.Length;
                    //this is a valid match if it is at the end of the reading (no character after the word, meaning we found the longest keyword !)
                    if (nextCharPosition >= originalText.Length ||
                        //also true if the following character is part of the list of separators (this is a single 'word' in our perspective, so a valid match)
                        Separators.Any(c => c == originalText[nextCharPosition]) ||
                        //also true if the following character is not a separator, but the type of keyword is marked as a 'mergeable' key word.
                        Separators.All(c => c != originalText[nextCharPosition]) && (allMergeableWords?.Contains(match.Key) ?? false))
                    {
                        //this is a valid keyword !
                        validMatches.Add(match);
                    }
                }
                if (validMatches.Any())
                {
                    //we select the best match (the longest)
                    //var foundMatch = GetLongestValue(validMatches);
                    var foundMatch = validMatches.GetMaxElement(match => match.Value.Length);
                    raw = foundMatch.Raw;
                    return new ParsedKeyword { Key = foundMatch.Key, Value = foundMatch.Value, StartLocation = startPosition };
                }
            }

            //2. No keyword found ? then this have to be a wildcard
            //we read the first word up to the first space, or separator and mark it as a wildcard keyword
            var wildCard = originalText.Split(Separators, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

            if (string.IsNullOrEmpty(wildCard))
            {
                //3. error case - heuristically unreachable as we consider a list of separator as a valid match (and thus never reach here)
                //and the only way to have an empty wildcard here would be to have only separator characters in the originalText (space or tab...)
                return null;
            }

            //to get the raw, we need all the characters up to the found keyword
            var index = originalText.IndexOf(wildCard, StringComparison.Ordinal);
            raw = originalText.Substring(0, index + wildCard.Length);
            return new ParsedKeyword { Key = ParsedKeyword.NoKeyword, Value = wildCard, StartLocation = originalText.IndexOf(wildCard, StringComparison.OrdinalIgnoreCase) };

        }
    }
}
