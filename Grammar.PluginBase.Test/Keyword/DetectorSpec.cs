using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using Grammar.PluginBase.Keyword;
using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Grammar.PluginBase.Test.Keyword
{
    public class DetectorSpec
    {
        public class Constructor
        {
            [Fact]
            public void NullInput_Initialize()
            {
                var test = new Detector(null, null);
                test.Errors.Should().BeNull();
                test.AllKeywords.Should().BeNull();
                test.AllMergeableWords.Should().BeNull();
                test.Resources.Should().BeNull();
            }

            [Fact]
            public void Initialize()
            {
                var i1 = new List<ParserError>();
                var i2 = new Mock<IResources>();
                var i3 = new List<string>();
                var i4 = new Dictionary<string, IEnumerable<string>>();
                i2.Setup(m => m.GetMergeableWords()).Returns(i3);
                i2.Setup(m => m.GetKeywords()).Returns(i4);

                var test = new Detector(i1, i2.Object);
                test.Errors.Should().BeSameAs(i1);
                test.Resources.Should().BeSameAs(i2.Object);
                test.AllMergeableWords.Should().BeSameAs(i3);
                test.AllKeywords.Should().BeSameAs(i4);
            }
        }

        public class DetectKeywords
        {
            [Fact]
            public void BlazonNull_Throw()
            {
                Action t = () => new Detector(null, null).DetectKeywords(null);

                t.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("blazon");
            }

            [Fact]
            public void AllKeywordsNull_Throw()
            {
                Action t = () => new Detector(null, null).DetectKeywords(new MemoryStream());

                t.Should().Throw<ArgumentException>().Which.ParamName.Should().Be(nameof(Detector.AllKeywords));
            }


            [Fact]
            public void EmptyInput_EmptyResult()
            {
                var resources = new Mock<IResources>();
                resources.Setup(r => r.GetKeywords()).Returns(
                    new Dictionary<string, IEnumerable<string>>
                    {
                        {"key", new[] {"value"}}
                    });
                new Detector(null, resources.Object).DetectKeywords(new MemoryStream()).Should().BeEmpty();
            }

            [Fact]
            public void NoMatching_EmptyResult()
            {
                //redundant with no input ... to rewrite
                var resources = new Mock<IResources>();
                resources.Setup(r => r.GetKeywords()).Returns(
                    new Dictionary<string, IEnumerable<string>>
                    {
                        {"key", new[] {"value"}}
                    });
                //var matches = new KeyWordMatch("key", "value", "raw");
                var detectorMock = new Mock<Detector>(null, resources.Object) { CallBase = true };
                detectorMock.Protected()
                    .SetupSequence<KeyWordMatch>("TryGetKvp", ItExpr.IsAny<string>(), ItExpr.IsAny<string>())
                    .Returns(default(KeyWordMatch))
                    .Returns(default(KeyWordMatch));

                var test = detectorMock.Object.DetectKeywords(new MemoryStream())?.ToList();
                test.Should().BeEmpty();
            }

            [Fact]
            public void MultipleMatchingInput_MultiplegResults()
            {
                //need to loop twice and provide the best result
                var resources = new Mock<IResources>();
                resources.Setup(r => r.GetKeywords()).Returns(
                    new Dictionary<string, IEnumerable<string>>
                    {
                        {"key", new[] {"value"}},
                        { "other", new[]{"test"} }
                    });
                //var matches = new KeyWordMatch("key", "value", "raw");
                var detectorMock = new Mock<Detector>(null, resources.Object) { CallBase = true };
                detectorMock.Protected()
                    .SetupSequence<KeyWordMatch>("TryGetKvp", ItExpr.IsAny<string>(), ItExpr.IsAny<string>())
                    .Returns(new KeyWordMatch("key", "value", "rawvalue"))
                    .Returns(new KeyWordMatch("other", "test", "rawtest"));

                var test = detectorMock.Object.DetectKeywords(new MemoryStream(Encoding.UTF8.GetBytes("va")))?.ToList();
                test.Should().HaveCount(1);
                test.First().Key.Should().Be("key");
                test.First().Value.Should().Be("value");
            }
        }

        public class TryGetKvp
        {
            [Fact]
            public void NullWord_ReturnNull()
            {
                new Detector(null, null).TryGetKvp(null, " ").Should().BeNull();
            }

            [Fact]
            public void NullList_ReturnNull()
            {
                new Detector(null, null).TryGetKvp(" ", " ").Should().BeNull();
            }


            [Fact]
            public void EmptyList_ReturnNull()
            {
                var resMock = new Mock<IResources>();
                resMock.Setup(m => m.GetKeywords())
                    .Returns(new Dictionary<string, IEnumerable<string>>());
                new Detector(null, resMock.Object).TryGetKvp(null, " ").Should().BeNull();
            }

            [Fact]
            public void NoMatch_ReturnNull()
            {
                var resMock = new Mock<IResources>();
                resMock.Setup(m => m.GetKeywords())
                    .Returns(new Dictionary<string, IEnumerable<string>>());
                var test = new Detector(null, resMock.Object);
                test.TryGetKvp(" ", " ").Should().BeNull();
            }

            [Fact]
            public void OneMatch_ReturnMatch()
            {
                var resMock = new Mock<IResources>();
                resMock.Setup(m => m.GetKeywords())
                    .Returns(new Dictionary<string, IEnumerable<string>>
                    {
                        {"key", new []{"value"} }
                    });
                var test = new Detector(null, resMock.Object);
                var kvp = test.TryGetKvp("value", "raw");
                kvp.Value.Should().Be("value");
                kvp.Key.Should().Be("key");
                kvp.Raw.Should().Be("raw");
            }

            [Fact]
            public void MultipleMatch_ReturnFirstMatch()
            {
                var resMock = new Mock<IResources>();
                resMock.Setup(m => m.GetKeywords())
                    .Returns(new Dictionary<string, IEnumerable<string>>
                    {
                        {"wrongKey", new []{"wrong value"} },
                        {"last key", new []{"another", "value"} },
                        {"key", new []{"value"} }
                    });
                var test = new Detector(null, resMock.Object);
                var kvp = test.TryGetKvp("value", "raw");
                kvp.Value.Should().Be("value");
                kvp.Key.Should().Be("last key");
                kvp.Raw.Should().Be("raw");
            }

            [Fact]
            public void MultipleWordMatch_ReturnFirstMatch()
            {
                var resMock = new Mock<IResources>();
                resMock.Setup(m => m.GetKeywords())
                    .Returns(new Dictionary<string, IEnumerable<string>>
                    {
                        {"wrongKey", new []{"wrong value", "another wrong one"} },
                        {"key", new []{"value another"} },
                        {"last key", new []{"value", "another" } }
                    });
                var test = new Detector(null, resMock.Object);
                var kvp = test.TryGetKvp("value another", "raw");
                kvp.Value.Should().Be("value another");
                kvp.Key.Should().Be("key");
                kvp.Raw.Should().Be("raw");
            }

            [Fact]
            public void MultipleWordMatch_ReturnNoMatch()
            {
                var resMock = new Mock<IResources>();
                resMock.Setup(m => m.GetKeywords())
                    .Returns(new Dictionary<string, IEnumerable<string>>
                    {
                        {"wrongKey", new []{"wrong value", "another wrong one"} },
                        {"key", new []{"value", "another"} },
                        {"last key", new []{"another", "value"} }
                    });
                var test = new Detector(null, resMock.Object);
                var kvp = test.TryGetKvp("value another, next", "raw");
                kvp.Should().Be(null);
            }
        }

        public class GetBestMatch
        {
            [Fact]
            public void OriginalTextNullOrEmpty_Throw()
            {
                var detector = new Detector(null, null);
                Action t = () => detector.GetBestMatch(null, new List<KeyWordMatch>(), new List<string>(), out var _);
                t.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("originalText");
                t = () => detector.GetBestMatch("", new List<KeyWordMatch>(), new List<string>(), out var _);
                t.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("originalText");
            }

            [Fact]
            public void NullCurrentMatches_Throw()
            {
                var detector = new Detector(null, null);
                Action t = () => detector.GetBestMatch(" ", null, null, out var _).Should().BeNull();
                t.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("currentMatches");
            }

            [Fact]
            public void EmptyCurrentMatches_ReturnNull()
            {
                var detector = new Detector(null, null);
                string raw = null;
                detector.GetBestMatch(" ", new List<KeyWordMatch>(), null, out raw).Should().BeNull();
                raw.Should().BeEmpty();
            }

            [Fact]
            public void NoValidMatches_ReturnWildCard()
            {
                var detector = new Detector(null, null);
                var source = "  x";
                var wildCard = detector.GetBestMatch(
                    source,
                    Array.Empty<KeyWordMatch>(),
                    null,
                    out var raw);
                wildCard.Key.Should().Be(ParsedKeyword.NoKeyword);
                wildCard.StartLocation.Should().Be(2);
                wildCard.Value.Should().Be("x");
                raw.Should().Be(source);
            }

            [Fact]
            public void HappyPathMultipleValidMatch_ReturnBest()
            {
                var detector = new Detector(null, null);
                var wildCard = detector.GetBestMatch(
                    "short", //only used for separator analysis, no need to mean anything, the length need to be under or equal the value though
                    new[]
                    {
                        new KeyWordMatch("key", "value", "raw"),
                        new KeyWordMatch("key2", "value2", "raw2")
                    },
                    null,
                    out var raw);
                wildCard.Key.Should().Be("key2");
                wildCard.StartLocation.Should().Be(-1);
                wildCard.Value.Should().Be("value2");
                raw.Should().Be("raw2");
            }
        }
    }
}
