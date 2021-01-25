using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ebnf;
using FluentAssertions;
using Xunit;

namespace Ebnf.Test
{
    public class ParserSpec
    {
        public class Parse
        {
            MemoryStream testInput { get; set; }

            StreamReader testReader { get; set; }


            [Fact]
            public void NullOrEmptyInputReturnNull()
            {
                var parser = new Parser();
                parser.Parse(null).Should().BeNull();
                parser.Parse(new MemoryStream()).Should().BeNull();
            }


            [Fact]
            public void CorrectInputReturnTree()
            {
                var content =
                    @"name01 = rule01;
name02    =  rule02 , and , (rule03 , rule04);name03=multiline, (* with comment *)
rule05
;";
                testInput = new MemoryStream(Encoding.UTF8.GetBytes(content));
                testReader = new StreamReader(testInput);

                var parser = new Parser();
                parser.Parse(testInput);
                parser.AllRules.Should().HaveCount(3);
                parser.AllRules[0].Name.Should().Be("name01 ");
                parser.AllRules[0].RulesContent.Should().Be(" rule01");
                parser.AllRules[1].Name.Should().Be(@"
name02    ");
                parser.AllRules[1].RulesContent.Should().Be("  rule02 , and , (rule03 , rule04)");
                parser.AllRules[2].Name.Should().Be("name03");
                parser.AllRules[2].RulesContent.Should().Be(@"multiline, 
rule05
");
            }
        }
    }

    public class TreeElementSpec
    {
        public class ExtractOneRule
        {
            MemoryStream testInput { get; set; }

            StreamReader testReader { get; set; }

            TreeElement testElement { get; set; }

            [Fact]
            public void NullOrEmptyStreamDoesNothing()
            {
                testElement = new TreeElement();
                testElement.ExtractOneRule(null);
                testElement.Name.Should().Be(new TreeElement().Name);
                testElement.RulesContent.Should().Be(new TreeElement().RulesContent);

                testReader = StreamReader.Null;
                testElement = new TreeElement();
                testElement.ExtractOneRule(testReader);
                testElement.Name.Should().Be(new TreeElement().Name);
                testElement.RulesContent.Should().Be(new TreeElement().RulesContent);
            }

            [Fact]
            public void ValidInputExtracted()
            {
                var name = "Name";
                var rulesContent = "Rules , Content";
                testInput = new MemoryStream(Encoding.UTF8.GetBytes($"{name}={rulesContent};"));
                testReader = new StreamReader(testInput);
                testElement = new TreeElement();
                testElement.ExtractOneRule(testReader);
                testElement.Name.Should().Be(name);
                testElement.RulesContent.Should().Be(rulesContent);
            }

            [Fact]
            public void ValidInputWithCommentsExtractedWithoutComments()
            {
                var name = "Name";
                var rulesContent = "Rules , Content";
                testInput = new MemoryStream(Encoding.UTF8.GetBytes($"{name}(* a comment here*)=(**){rulesContent}(* another here *);"));
                testReader = new StreamReader(testInput);
                testElement = new TreeElement();
                testElement.ExtractOneRule(testReader);
                testElement.Name.Should().Be(name);
                testElement.RulesContent.Should().Be(rulesContent);
            }

            [Fact]
            public void InfiniteInputThrowException()
            {
                var name = "Name";
                var rulesContent = "Rules , Content";
                testInput = new MemoryStream(Encoding.UTF8.GetBytes($"{name}={rulesContent}"));
                testReader = new StreamReader(testInput);
                testElement = new TreeElement();
                Action act = () => testElement.ExtractOneRule(testReader);
                act.Should().Throw<Exception>();
            }

            [Fact]
            public void ValidInputDoNotConsumeNextEntry()
            {
                var name = "Name";
                var rulesContent = "Rules , Content";
                var extracontent = "moreleft side = more content after;";
                testInput = new MemoryStream(Encoding.UTF8.GetBytes($"{name}={rulesContent};{extracontent}"));
                testReader = new StreamReader(testInput);
                testElement = new TreeElement();
                testElement.ExtractOneRule(testReader);
                testElement.Name.Should().Be(name);
                testElement.RulesContent.Should().Be(rulesContent);
                testReader.ReadToEnd().Should().Be(extracontent);
            }

            [Fact]
            public void NoLeftHandSideThrowException()
            {
                var name = "Name";
                var rulesContent = "Rules , Content";
                testInput = new MemoryStream(Encoding.UTF8.GetBytes($"{name}{rulesContent};"));
                testReader = new StreamReader(testInput);
                testElement = new TreeElement();
                Action act = () => testElement.ExtractOneRule(testReader);
                act.Should().Throw<Exception>();
            }

            [Fact]
            public void InfiniteCommentThrowException()
            {
                testInput = new MemoryStream(Encoding.UTF8.GetBytes($"(* a comment that does not end"));
                testReader = new StreamReader(testInput);
                testElement = new TreeElement();
                Action act = () => testElement.ExtractOneRule(testReader);
                act.Should().Throw<Exception>();
            }
        }

        public class ParseInternalRule
        {
            [Fact]
            public void NullOrEmptyContentReturnEmptyNoChild()
            {
                var testTe = new TreeElement { RulesContent = string.Empty };
                testTe.ParseInternalRules(null).Should().BeEmpty();
                testTe.Children.Should().BeEmpty();
                testTe = new TreeElement { RulesContent = null };
                testTe.ParseInternalRules(null).Should().BeEmpty();
                testTe.Children.Should().BeEmpty();
            }

            [Fact]
            public void SimplestChildExistReturnNothing()
            {
                var testVal = new TreeElement { Name = "one" };
                var testTe = new TreeElement { RulesContent = "one" };
                testTe.ParseInternalRules(new List<TreeElement> { testVal }).Should().BeEmpty();
                testTe.Children.Should().HaveCount(1);
                testTe.Children[0].Should().Be(testVal);
            }

            [Fact]
            public void ChildNotExistReturned()
            {
                var child0 = new TreeElement { Name = "zero" };
                var child1 = new TreeElement { Name = "one" };
                var child2 = new TreeElement { Name = "two" };
                var child3 = new TreeElement { Name = "casesensitive" };
                var availableRules = new List<TreeElement> { child0, child1, child2, child3 };

                var value = "one, three, two  , zero, caSesensitive";
                var testTe = new TreeElement { RulesContent = value };
                testTe.ParseInternalRules(availableRules)
                    .Should().HaveCount(2)
                    .And.Subject.Should().Contain(te => te.Name == "three")
                    .And.Subject.Should().Contain(te => te.Name == "caSesensitive");

                testTe.Children.Should().HaveCount(5);
                testTe.Children.Should().ContainInOrder(child1, child2, child0);
            }

            [Fact]
            public void GroupCreateOneChildGroup()
            {
                var firstTest = "one , two ";
                var secondTest = "one, two  , ThRee";
                var thirdTest = "one , (two, three), four ";

                var testTe = new TreeElement { RulesContent = $"({firstTest})" };
                testTe.ParseInternalRules(null).Should().HaveCount(2);
                testTe.Children.Should().HaveCount(1);
                testTe.Children[0].RulesContent.Should().Be(firstTest);

                testTe = new TreeElement { RulesContent = $"{{{secondTest}}}" };
                testTe.ParseInternalRules(null).Should().HaveCount(3);
                testTe.Children.Should().HaveCount(1);
                testTe.Children[0].RulesContent.Should().Be(secondTest);

                testTe = new TreeElement { RulesContent = $"[{thirdTest}]" };
                testTe.ParseInternalRules(null).Should().HaveCount(4);
                testTe.Children.Should().HaveCount(1)
                    .And.Subject.Should().Contain(te => te.IsOptional)
                    .And.Subject.Should().ContainSingle(thirdTest);
            }

            [Fact]
            public void TermBeforeGroupShouldNotAddSpaceInGroup()
            {
                var testTe = new TreeElement { RulesContent = $"zero , (first)" };
                testTe.ParseInternalRules(null).Should().HaveCount(2);
                testTe.Children.Should().HaveCount(2)
                    .And.Subject.Should().Contain(te => te.RulesContent == "first"); //and not " first"
            }

            [Fact]
            public void GroupThenRuleCreate2Children()
            {
                var testTe = new TreeElement { RulesContent = $"(first) , second" };
                testTe.ParseInternalRules(null).Should().HaveCount(2);
                testTe.Children.Should().HaveCount(2)
                    .And.Subject.Should().Contain(te => te.RulesContent == "second")
                    .And.Subject.Should().Contain(te => te.RulesContent == "first");
            }

            [Fact]
            public void UnclosedGroupThrowException()
            {
                // ( start
                Action test = () => new TreeElement { RulesContent = $"(" }.ParseInternalRules(null);
                test.Should().Throw<Exception>();


                test = () => new TreeElement { RulesContent = $"( start" }.ParseInternalRules(null);
                test.Should().Throw<Exception>();


                test = () => new TreeElement { RulesContent = $" before too (" }.ParseInternalRules(null);
                test.Should().Throw<Exception>();

                test = () => new TreeElement { RulesContent = $" before too )" }.ParseInternalRules(null);
                test.Should().Throw<Exception>();

                test = () => new TreeElement { RulesContent = $") start" }.ParseInternalRules(null);
                test.Should().Throw<Exception>();
            }

            [Fact]
            public void SubGroupOverParentGroupThrowException()
            {
                // ( start { continue ) what are you doing }
                Action test = () => new TreeElement { RulesContent = $"( start [ continue ) what are you doing ]" }.ParseInternalRules(null);
                test.Should().Throw<Exception>();
            }

            [Fact]
            public void AlternativeSequenceInOneChild()
            {
                // 1 | (2) | 3
                // 1 | 2 | 3 | 4
                var testTe = new TreeElement { RulesContent = $"1 | (2) | 3" };
                testTe.ParseInternalRules(null).Should().HaveCount(3);
                testTe.Children.Should().HaveCount(1)
                    .And.Subject.Should().Contain(te => te.Children.Count == 3);
                testTe.Children[0].Children.Should().Contain(te => te.Name == "1");
                testTe.Children[0].Children.Should().Contain(te => te.RulesContent == "2");
                testTe.Children[0].Children.Should().Contain(te => te.Name == "3");


                testTe = new TreeElement { RulesContent = $"1 | 2 | 3 | 4" };
                testTe.ParseInternalRules(null).Should().HaveCount(4);
                testTe.Children.Should().HaveCount(1)
                    .And.Subject.Should().Contain(te => te.Children.Count == 4);
            }

            [Fact]
            public void AlternativeSequenceMultipleChildren()
            {
                // (1) | 2 | 3 | [4]

                var testTe = new TreeElement { RulesContent = $"(1) | 2 | 3 | [4]" };
                testTe.ParseInternalRules(null).Should().HaveCount(4);
                testTe.Children.Should().HaveCount(1)
                    .And.Subject.Should().Contain(te => te.Children.Count == 4);
            }

            [Fact]
            public void AlternativeSequenceWithInternalGroup()
            {
                // 1 | (2 | 3) | (4)
                var testTe = new TreeElement { RulesContent = $" 1 | (2 | 3) | (4)" };
                testTe.ParseInternalRules(null).Should().HaveCount(4);
                testTe.Children.Should().HaveCount(1)
                    .And.Subject.Should().Contain(te => te.Children.Count == 3);
            }

            [Fact]
            public void MultipleAlternativeSequenceSeparated()
            {
                // 1 | (2) | 3 , (4) , 5 , 6 | 7 , 8
                var testTe = new TreeElement { RulesContent = $"1 | (2) | 3 , (4) , 5 , 6 | 7 , 8" };
                testTe.ParseInternalRules(null).Should().HaveCount(8);
                testTe.Children.Should().HaveCount(5);
                testTe.Children[0].Children.Should().HaveCount(3);
                testTe.Children[3].Children.Should().HaveCount(2);
            }

            [Fact]
            public void AlternativeSequenceMixed()
            {
                // 1 | 2 , 3 | 4
                // 1 , 2 | 3 , 4
                var testTe = new TreeElement { RulesContent = $" 1 | 2 , 3 | 4" };
                testTe.ParseInternalRules(null).Should().HaveCount(4);
                testTe.Children.Should().HaveCount(2);
                testTe.Children[0].Children.Should().HaveCount(2);
                testTe.Children[1].Children.Should().HaveCount(2);

                testTe = new TreeElement { RulesContent = $" 1 , 2 | 3 , 4" };
                testTe.ParseInternalRules(null).Should().HaveCount(4);
                testTe.Children.Should().HaveCount(3);
                testTe.Children[0].Children.Should().BeEmpty();
                testTe.Children[1].Children.Should().HaveCount(2);
                testTe.Children[2].Children.Should().BeEmpty();
            }


            [Fact]
            public void OperatorWithoutermThrow()
            {
                // , 1  (* Starting with an operator *)
                // | 1

                Action act = () => new TreeElement { RulesContent = $", 1 " }.ParseInternalRules(null);
                act.Should().Throw<Exception>();

                act = () => new TreeElement { RulesContent = $"| 1 " }.ParseInternalRules(null);
                act.Should().Throw<Exception>();
            }
        }
    }
}
