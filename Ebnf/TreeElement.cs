using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utils.LinqHelper;

namespace Ebnf
{
    /// <summary>
    /// Represent one of the node of the grammar, either as a parent of subnode, or as a final one with a simple rule
    /// </summary>
    public class TreeElement
    {
        /// <summary>
        /// The representation of the rule, either the given name in the grammar or a constructed one for groups
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If the rules included in this node are optional
        /// </summary>
        public bool IsOptional { get; set; } = false;

        /// <summary>
        /// If the rules included in this node are meant to be repeated more than once (compatible with optional)
        /// </summary>
        public bool IsRepetition { get; set; } = false;

        /// <summary>
        /// If only one of the rules included in this node is meant to be resolved
        /// </summary>
        public bool IsAlternation { get; set; } = false;

        public bool IsGroup { get; set; } = false;

        public bool IsLeaf { get; set; } = false;

        /// <summary>
        /// The raw content of the rule (text representation from the grammar)
        /// </summary>
        public string RulesContent { get; set; }

        /// <summary>
        /// The actual rules that are to be applied when encountering that node
        /// </summary>
        public IList<TreeElement> Children { get; set; } = new List<TreeElement>();

        /// <summary>
        /// The parents to which this rule is attached, as a rule can belong to multiple other rules. Used for optimization and to find the roots
        /// </summary>
        public IList<TreeElement> Parents { get; set; } = new List<TreeElement>();

        /// <summary>
        /// Cut the input into a rule that will be represented by the current instance.
        /// If succesful assign the <see cref="Name"/> from the left side of the input and the <see cref="RulesContent"/> from the right side
        /// </summary>
        /// <param name="reader">The reader containing the input to process</param>
        public bool ExtractOneRule(StreamReader reader)
        {
            if (reader == null || reader.EndOfStream)
            {
                return false;
            }
            int lastCode;
            char lastChar;
            var leftSideDone = false;
            var rightSideDone = false;
            var startedComment = false;
            var started = false;

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

                started = started || !char.IsControl(lastChar) || !char.IsWhiteSpace(lastChar);

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
            Name = Name.Trim().Trim(Environment.NewLine.ToCharArray());
            if (string.IsNullOrEmpty(Name))
            {
                return false;
            }
            if(!Name.All(c => char.IsLetterOrDigit(c)))
            {
                throw new Exception($"The rule named {Name} contains invalid characters, make sure the name is a single word");
            }
            //if we did not get to the end, this is an error
            if (started && !rightSideDone)
            {
                throw new Exception($"Reached the end of the input without proper grammar rule ending '{Parser.SemiColonSign}'");
            }
            return true;
        }

        /// <summary>
        /// Helper to include a single end node as a Children of the current rule
        /// </summary>
        /// <param name="currentRuleName">The current rule content to be used for the creation</param>
        /// <param name="availableRules">The list of already available rules to avoid creating one already existing</param>
        /// <param name="newRules">The list of new rules created by this node, to which we can add the new rule that we will create if not already existing</param>
        /// <returns>The newly added or retrieved element that became our child</returns>
        private TreeElement AddSingleRule(ref string currentRuleName, List<TreeElement> availableRules, List<TreeElement> newRules)
        {
            TreeElement toReturn = null;
            var cleanName = currentRuleName.Trim().Trim(Environment.NewLine.ToCharArray());
            if (string.IsNullOrEmpty(cleanName))
            {
                return toReturn;
            }
            var available = availableRules.FirstOrDefault(te => string.Equals(te.Name, cleanName));
            if (available == null)
            {
                available = newRules.FirstOrDefault(te => string.Equals(te.Name, cleanName));
                if (available == null)
                {
                    available = new TreeElement { Name = cleanName, RulesContent = cleanName };
                    newRules.Add(available);
                }
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

            //We read in order, every 1st degree term is a "child"
            //if the child name was alread defined, then 
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
                        case Parser.TextStartSign:
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
                                currentRule = new TreeElement { Name = $"Group#{Math.Abs(currentRuleName.GetHashCode())}", RulesContent = currentRuleName };
                                currentRule.IsRepetition = groupOpening == Parser.RepeatStartSign;
                                currentRule.IsOptional = groupOpening == Parser.OptionalStartSign;
                                currentRule.IsGroup = groupOpening == Parser.GroupStartSign;
                                lastAddedRule = null;
                                newRules.Add(currentRule);
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
                if (currentRule != null)
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
                            var alternChild = new TreeElement { Name = $"Group.Altern.{groupNumber}", IsAlternation = true };
                            alternChild.Parents.Add(this);
                            var idx = Children.IndexOf(agroup.First());
                            newRules.Add(alternChild);
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
                                if (!string.IsNullOrEmpty(alternChild.RulesContent))
                                {
                                    alternChild.RulesContent += $" {Parser.AlternSign} ";
                                }
                                alternChild.RulesContent += c.Name;
                                c.Parents.AddDistinct(alternChild);
                            }
                            alternChild.Name += $"#{Math.Abs(alternChild.GetHashCode())}";
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
        public TreeElement Optimize()
        {
            if (Children.Count != 1 && Parents != null && Parents.Any(p => p.Children.Count > 1)) { return null; }

            var onlyChild = Children.FirstOrDefault();

            onlyChild.IsAlternation |= IsAlternation;
            onlyChild.IsOptional |= IsOptional;
            onlyChild.IsRepetition |= IsRepetition;
            onlyChild.Name = Name;
            onlyChild.Parents.Remove(this);
            foreach (var parent in Parents)
            {
                var myIndex = parent.Children.IndexOf(this);
                parent.Children.Insert(myIndex, onlyChild);
                parent.Children.Remove(this);
                onlyChild.Parents.AddDistinct(parent);
            }

            return onlyChild;
        }

        /// <summary>
        /// Override for debugging purpose using the name of the rule to display it
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? base.ToString() : Name;
        }
    }
}
