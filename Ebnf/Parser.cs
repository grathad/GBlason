using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Ebnf
{
    /// <summary>
    /// Take a text stream as an input, and try to parse it into a EBNF Tree
    /// This version DOES NOT SUPPORT terminal string and keyword, which, for my only goal, are carried through a separate keywords file
    /// This means that as soon as a semicolon is met, this is the end of a grammar rule, period, no expectation to get this into a string as a regular character for now
    /// </summary>
    public class Parser
    {
        public const char EqualSign = '=';
        public const char SemiColonSign = ';';
        public const char GroupStartSign = '(';
        public const char GroupEndSign = ')';
        public const char RepeatStartSign = '{';
        public const char RepeatEndSign = '}';
        public const char OptionalStartSign = '[';
        public const char OptionalEndSign = ']';
        public const char SequenceSign = ',';
        public const char AlternSign = '|';
        public const char CommentStartSign = '(';
        public const string CommentStart = "(*";
        public const string CommentEnd = "*)";

        public List<TreeElement> AllRules { get; set; } = new List<TreeElement>();

        public List<TreeElement> Parse(Stream input, Encoding encoding = null)
        {
            if (input == null || input.Length == 0)
            {
                return null;
            }
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            using (var reader = new StreamReader(input, encoding))
            {
                //the format of a rule is: ruleName = rule ;
                while (reader.Peek() != -1)
                {
                    var nte = new TreeElement();
                    nte.ExtractOneRule(reader);
                    AllRules.Add(nte);
                }
            }
            //now we have the raw rules defined from the input.
            //we want to start parsing them a little better, with splitting their content
            //we have one child per simple named rule (sequence)
            //we have one rule per children that belong to the same alternation chain.
            //we have one child per group, optional group and repetition group
            var knownRules = AllRules.ToList();
            var allRules = knownRules.ToList();
            foreach (var rule in allRules.Where(r => r != null))
            {
                knownRules.AddRange(rule.ParseInternalRules(knownRules));
            }
            AllRules = knownRules;

            //finally we optimize the rules
            var toRemove = new List<TreeElement>();
            foreach (var rule in AllRules.Where(r => r != null))
            {
                if (rule.Optimize())
                {
                    toRemove.Add(rule);
                }
            }

            foreach (var rule in toRemove)
            {
                AllRules.Remove(rule);
            }

            return AllRules;
        }

    }
}
