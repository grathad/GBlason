using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Utils.LinqHelper;

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
        public const char TextStartSign = '"';
        public const char TextEndSign = '"';
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

            //First step of the parsing is to read all the declared rules and assign them as a treelement (with the rawvalues of the name and the content for now)
            using (var reader = new StreamReader(input, encoding))
            {
                //the format of a rule is: ruleName = rule ;
                while (reader.Peek() != -1)
                {
                    var nte = new TreeElement();
                    if(nte.ExtractOneRule(reader))                    
                    {
                        AllRules.Add(nte);
                        Trace.TraceInformation($"adding a rule: {nte}");
                    }                    
                }
            }

            //now we have the raw rules defined from the input.
            //we want to start parsing them a little better, with splitting their content
            //this part could be done in on pass for performance optimization, but perf is not important here
            //for the sake of simplifying the coding, and reducing the size of the steps, we only consider a treeelement with its "1st degree" blocks
            //so the next step is to detect the blocks in the rule content sequence
            //this step create the tree element children

            
            var knownRules = AllRules.ToList();
            var allRules = knownRules.ToList();
            foreach (var rule in allRules.Where(r => r != null))
            {
                var newRules = rule.ParseInternalRules(knownRules);
                Trace.TraceInformation($"internal parsing of '{rule}' with the content {rule.RulesContent}. Found {newRules?.Count ?? 0} new rule(s)");
                knownRules.AddRange(newRules);
            }
            AllRules = knownRules;

            //finally we optimize the rules
            var toRemove = new List<TreeElement>();
            //foreach (var rule in AllRules.Where(r => r != null))
            //{
            //    var optimizedRule = rule.Optimize();
            //    if (optimizedRule != null)
            //    {
            //        toRemove.Add(rule);
            //    }
            //}

            //foreach (var rule in toRemove)
            //{
            //    AllRules.Remove(rule);
            //}

            return AllRules;
        }

    }
}
