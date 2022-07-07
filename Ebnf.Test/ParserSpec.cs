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
                parser.AllRules.Should().HaveCount(11);
                parser.AllRules[0].Name.Should().Be("name01");
                parser.AllRules[0].RulesContent.Should().Be(" rule01");
                parser.AllRules[1].Name.Should().Be(@"name02");
                parser.AllRules[1].RulesContent.Should().Be("  rule02 , and , (rule03 , rule04)");
                parser.AllRules[2].Name.Should().Be("name03");
                parser.AllRules[2].RulesContent.Should().Be(@"multiline, 
rule05
");
            }
        }
    }
}
