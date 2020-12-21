using System.Collections.Generic;
using Grammar.PluginBase.Keyword;
using Xunit;

namespace Grammar.PluginBase.Test.Keyword
{
    public class FoundExpectedSpecs
    {
        public class Constructor
        {
            [Fact]
            public void OneWord()
            {
                //happy path one word
                var result = new FoundExpected((ParsedKeyword) null, "one word");
                Assert.Equal("one word", result.Expected);
            }

            [Fact]
            public void MultipleWords()
            {
                //happy path multiple words
                var result = new FoundExpected((ParsedKeyword) null, new List<IEnumerable<string>>
                {
                    new[] {"a"},
                    new[] {"list", "of"},
                    new[] {"words"}
                });
                Assert.Equal("a, list of or words", result.Expected);
            }

            [Fact]
            public void NoWord()
            {

                //error path no words
                var result = new FoundExpected((ParsedKeyword) null,
                    new List<IEnumerable<string>>());
                Assert.Null(result.Expected);
            }

            [Fact]
            public void NullInput()
            {

                //error path null
                var result = new FoundExpected((ParsedKeyword) null,
                    (List<IEnumerable<string>>) null);
                Assert.Null(result.Expected);
            }
        }
    }
}
