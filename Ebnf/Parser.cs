using System;
using System.Collections.Generic;
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

    public class TreeElement
    {
        public string Name { get; set; }

        public bool IsOptional { get; set; } = false;

        public bool IsRepetition { get; set; } = false;

        public bool IsAlternation { get; set; } = false;

        public string RulesContent { get; set; }

        public IList<TreeElement> Children { get; set; } = new List<TreeElement>();

        public IList<TreeElement> Parents { get; set; } = new List<TreeElement>();

        /// <summary>
        /// Cut the input into a rule that will be represented by the current instance
        /// </summary>
        /// <param name="reader"></param>
        public void ExtractOneRule(StreamReader reader)
        {
            if (reader == null || reader.EndOfStream)
            {
                return;
            }
            int lastCode;
            char lastChar;
            var leftSideDone = false;
            var rightSideDone = false;
            var startedComment = false;

            //the format of a rule is: ruleName = rule ;
            while (reader.Peek() != -1)
            {
                lastCode = reader.Read();
                if (lastCode == -1)
                {
                    break;
                }
                lastChar = (char)lastCode;

                if (lastChar == Parser.CommentStartSign && Parser.CommentStart == string.Concat(lastChar, (char)reader.Peek()))
                {
                    reader.Read();
                    startedComment = true;
                    continue;
                }

                if (startedComment)
                {
                    string endSearch = string.Concat(lastChar, (char)reader.Peek());
                    if (endSearch == Parser.CommentEnd)
                    {
                        reader.Read();
                        startedComment = false;
                    }
                    continue;
                }

                if (!leftSideDone)
                {
                    if (lastChar == Parser.EqualSign)
                    {
                        leftSideDone = true;
                    }
                    else
                    {
                        if (lastChar == Parser.SemiColonSign)
                        {
                            throw new Exception($"Found a end of rule character '{Parser.SemiColonSign}' before finding the rule left and right side separation '{Parser.EqualSign}' in the text {Name}");
                        }
                        Name += (char)lastCode;
                    }
                }
                else
                {
                    if (lastChar == Parser.SemiColonSign)
                    {
                        //the rawelement is done, we move to the next one
                        rightSideDone = true;
                        break;
                    }
                    RulesContent += lastChar;
                }
            }
            //if we did not get to the end, this is an error
            if (!rightSideDone)
            {
                throw new Exception($"Reached the end of the input without proper grammar rule ending '{Parser.SemiColonSign}'");
            }
        }

        private TreeElement AddSingleRule(ref string currentRuleName, List<TreeElement> availableRules, List<TreeElement> newRules)
        {
            TreeElement toReturn = null;
            var cleanName = currentRuleName.Trim();
            if (string.IsNullOrEmpty(cleanName))
            {
                return toReturn;
            }
            var available = availableRules.FirstOrDefault(te => string.Equals(te.Name, cleanName));
            if (available == null)
            {
                available = new TreeElement { Name = cleanName, RulesContent = cleanName };
                newRules.Add(available);
            }
            currentRuleName = string.Empty;
            Children.Add(available);
            available.Parents.AddDistinct(this);
            return available;
        }

        /// <summary>
        /// Read the left hand side of the raw rule, and split them into children
        /// For final rule names if existing in the <paramref name="availableRules"/> the corresponding TreeElement will be attached as a child
        /// If not available, a new TreeElement is created and added as a child, the newly created children are sent back
        /// The groups (neutral, optional or repetitives) are all in a non returned new child element, that use the content of the group as a nominal representation
        /// </summary>
        /// <param name="availableRules">The list of available rules that can be attached as child to this rule</param>
        /// <returns>All the new rules (not the groups, only unique child) as a list, or an empty list if no new rule</returns>
        public List<TreeElement> ParseInternalRules(List<TreeElement> availableRules)
        {
            if (string.IsNullOrEmpty(RulesContent))
            {
                return new List<TreeElement>();
            }
            if (availableRules == null)
            {
                availableRules = new List<TreeElement>();
            }

            //there are multiple separator, in order of priority: (again the string and final text ARE NOT in scope of this implementation
            //( ... ) neutral group
            //{ ... } repetition group
            //[ ... ] optional group
            //, sequence
            //| alternation

            var newRules = new List<TreeElement>();
            using (var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(RulesContent))))
            {
                var currentRuleName = string.Empty;
                TreeElement currentRule = null;
                TreeElement lastAddedRule = null;
                var alternationGroups = new List<List<TreeElement>>();

                var groupVal = 0;
                char groupOpening = char.MinValue;

                while (reader.Peek() != -1)
                {
                    var nextChar = (char)reader.Read();
                    switch (nextChar)
                    {
                        case Parser.GroupStartSign:
                        case Parser.RepeatStartSign:
                        case Parser.OptionalStartSign:
                            //this is the start of a group (only option) if the content is only one single group or element, this can be optimized away
                            //we send all the text until the next closing sign 
                            if (groupOpening == char.MinValue)
                            {
                                if (!string.IsNullOrEmpty(currentRuleName.Trim()))
                                {
                                    //syntax error
                                    throw new Exception($"a new group start at the character '{nextChar}' without a proper closure of the previous content: {currentRuleName}");
                                }
                                currentRuleName = string.Empty;
                                groupOpening = nextChar;
                                groupVal = 1;
                            }
                            else if (groupOpening == nextChar)
                            {
                                groupVal++;
                            }
                            else
                            {
                                currentRuleName += nextChar;
                            }
                            break;
                        case Parser.GroupEndSign when groupOpening == Parser.GroupStartSign:
                        case Parser.RepeatEndSign when groupOpening == Parser.RepeatStartSign:
                        case Parser.OptionalEndSign when groupOpening == Parser.OptionalStartSign:
                            groupVal--;
                            if (groupVal == 0)
                            {
                                //everything before this is the subgroup we create a child
                                currentRule = new TreeElement { Name = $"Group.{currentRuleName.GetHashCode()}", RulesContent = currentRuleName };
                                currentRule.IsRepetition = groupOpening == Parser.RepeatStartSign;
                                currentRule.IsOptional = groupOpening == Parser.OptionalStartSign;
                                lastAddedRule = null;
                                newRules.AddRange(currentRule.ParseInternalRules(newRules.Concat(availableRules).ToList()));
                                currentRuleName = string.Empty;
                                groupOpening = char.MinValue;
                            }
                            break;
                        case Parser.GroupEndSign when groupOpening == char.MinValue:
                        case Parser.RepeatEndSign when groupOpening == char.MinValue:
                        case Parser.OptionalEndSign when groupOpening == char.MinValue:
                            throw new Exception($"Found a group closing character '{nextChar}' without the corresponding opener in : {RulesContent}");

                        case Parser.SequenceSign when groupOpening == char.MinValue:
                            //all the word before (should be only one) is the closure of the rule, unless we are in a block, in this case, we ignore
                            //also true if the content is empty (the case if the previous block before that coma was a group)
                            lastAddedRule = AddSingleRule(ref currentRuleName, availableRules, newRules);
                            if (lastAddedRule == null)
                            {
                                if (currentRule != null)
                                {
                                    //we are dealing with a closed group that is being closed by our current sign
                                    Children.Add(currentRule);
                                    currentRule.Parents.AddDistinct(this);
                                    lastAddedRule = currentRule;
                                    currentRule = null;
                                }
                                else
                                {
                                    throw new Exception($"Encountered a sequence operator without left hand term in {RulesContent}");
                                }
                            }
                            break;
                        case Parser.AlternSign when groupOpening == char.MinValue:
                            //check the unit test for all the scenario, its pretty complex
                            //the logic is to save, in another separate list the elements that we added that are related using the |
                            //so at the end, the one closely grouped by | are removed in their own alternate group
                            var previousAlternateGroup = alternationGroups.LastOrDefault();
                            bool isPreviousAlternateGroupCurrent = false;
                            if ((previousAlternateGroup?.Any() != null) && Children.Any())
                            {
                                //2 scenario we are at a | which previous last added rule is a one to add now (with the reading of the |)
                                //or we are at a | which previous last added rule have already been added (which is possible if it was a group), it's not a real parser ... it's a hack to progress with my grammar definition struggles
                                //to solve this, I will make sure that a group closing itself, also take care of adding itself to a previous alternate group if any (only true for right term of a |)
                                isPreviousAlternateGroupCurrent = previousAlternateGroup.LastOrDefault() == Children.LastOrDefault();
                            }
                            //we need to understand what is left of the |
                            if (!string.IsNullOrEmpty(currentRuleName.Trim()))
                            {
                                //we are replacing the coma and enter the left side as the new rule.
                                lastAddedRule = AddSingleRule(ref currentRuleName, availableRules, newRules);
                            }
                            if (lastAddedRule == null)
                            {
                                if (currentRule != null)
                                {
                                    Children.Add(currentRule);
                                    currentRule.Parents.AddDistinct(this);
                                    lastAddedRule = currentRule;
                                    currentRule = null;
                                }
                                else
                                {
                                    throw new Exception($"Encountered an alternative operator without left hand term in {RulesContent}");
                                }
                            }
                            //we add the last added rule to either a new alternation group, or a past one
                            if (isPreviousAlternateGroupCurrent)
                            {
                                previousAlternateGroup.Add(lastAddedRule);
                            }
                            else
                            {
                                //this is a new alternation group
                                var newGroup = new List<TreeElement> { lastAddedRule };
                                alternationGroups.Add(newGroup);
                            }
                            break;
                        default:
                            currentRuleName += nextChar;
                            break;
                    }
                }
                if (groupVal > 0)
                {
                    throw new Exception($"One group starting with {groupOpening}{currentRuleName} was not properly closed in the content: {RulesContent}");
                }
                if (groupVal < 0)
                {
                    throw new Exception($"One group starting with {groupOpening}{currentRuleName} was not properly closed or mixed with another group in the content: {RulesContent}");
                }
                if (!string.IsNullOrEmpty(currentRuleName))
                {
                    //last simple term
                    AddSingleRule(ref currentRuleName, availableRules, newRules);
                }
                else if (currentRule != null)
                {
                    //last group
                    Children.Add(currentRule);
                    currentRule.Parents.AddDistinct(this);
                }

                //cleaning up all the alternation groups
                var groupNumber = 0;
                if (alternationGroups != null && alternationGroups.Any())
                {
                    foreach (var agroup in alternationGroups)
                    {
                        //we remote the children as direct child, and attach them to the new tree element
                        if (agroup != null && agroup.Any())
                        {
                            var alternChild = new TreeElement { Name = $"Group.Altern.{groupNumber}" };
                            var idx = Children.IndexOf(agroup.First());
                            Children.Insert(idx, alternChild);
                            //now moving the children from the parent to the alternative child
                            //because we only store the children BEFORE the | we also need to recover the one after
                            var lastidx = Children.IndexOf(agroup.Last());
                            //if we do not have the element after, we should throw it means we had a | without a term after it
                            agroup.Add(Children.ElementAt(lastidx + 1));
                            foreach (var c in agroup)
                            {
                                Children.Remove(c);
                                c.Parents.Remove(this);
                                alternChild.Children.Add(c);
                                c.Parents.AddDistinct(alternChild);
                            }
                        }
                        groupNumber++;
                    }
                }

                return newRules;
            }

        }

        /// <summary>
        /// Optimize (mostly removing group with only one element) the tree, a rule can only optimize itself away, you need to call optimize manually on the children to continue the process
        /// </summary>
        /// <returns>The list of rules that has been deleted</returns>
        public bool Optimize()
        {
            //if the current rule has one child, and only one child (no matter if the child has children on its own)
            //Then the child should inherit the properties (boolean) of the parent, and be attached to the parent's parent.
            if (Children.Count != 1) { return false; }

            var onlyChild = Children.FirstOrDefault();

            onlyChild.IsAlternation |= IsAlternation;
            onlyChild.IsOptional |= IsOptional;
            onlyChild.IsRepetition |= IsRepetition;
            onlyChild.Parents.Remove(this);
            foreach (var parent in Parents)
            {
                var myIndex = parent.Children.IndexOf(this);
                parent.Children.Insert(myIndex, onlyChild);
                parent.Children.Remove(this);
                onlyChild.Parents.AddDistinct(parent);                
            }

            return true;
        }
    }
}
