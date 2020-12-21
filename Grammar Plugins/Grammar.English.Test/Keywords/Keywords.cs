using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Grammar.English.Test.Integration.Resources;
using Grammar.PluginBase.Keyword;
using Xunit;

namespace Grammar.English.Test.Integration.Keywords
{
    /// <summary>
    /// Complex unit tests based on examples
    /// Those are not really unit test and are using real examples to check the coverage of the code in term of success (a layer on top of the unit test)
    /// The real unit test are done in the other classes in the same namespace
    /// This also contain a check to make sure the vocabulary (list of keywords) is not redundant (duplicates)
    /// </summary>
    public class Keywords
    {
        /// <summary>
        /// Targeted per failing cases or per names to cover the <see cref="Detector.DetectKeywords(Stream)"/>
        /// </summary>
        [Fact]
        public void DetectKeywordsTargeted()
        {
            //simple quarterly tincture colour and tincture metal and basic refactoring (and kw)
            var parser = new Detector(new List<ParserError>(), new PluginBase.Keyword.Resources("en-GB", Assembly.GetAssembly(typeof(Keywords))));
            var results = parser.DetectKeywords(new MemoryStream(Encoding.UTF8.GetBytes((string)BlazonRepository.DivisionQuarterlyNoNumber)));
            Assert.Collection(results,
                keyword => Assert.Equal("quarterly", keyword.Value),
                keyword => Assert.Equal("argent", keyword.Value),
                keyword => Assert.Equal("and", keyword.Value),
                keyword => Assert.Equal("gules", keyword.Value));

            //number of charges sub charges listing (with comma, and)
            results = parser.DetectKeywords(new MemoryStream(Encoding.UTF8.GetBytes((string)BlazonRepository.ChargeHallwin)));
            Assert.Collection(results,
                keyword => Assert.Equal("Argent", keyword.Value),
                keyword => Assert.Equal("three", keyword.Value),
                keyword => Assert.Equal("lions", keyword.Value),
                keyword => Assert.Equal("sable", keyword.Value),
                keyword => Assert.Equal("armed", keyword.Value),
                keyword => Assert.Equal(",", keyword.Value),
                keyword => Assert.Equal("langued", keyword.Value),
                keyword => Assert.Equal("and", keyword.Value),
                keyword => Assert.Equal("crowned", keyword.Value),
                keyword => Assert.Equal("or", keyword.Value));

            //multi words charge as wildcards, ordinary (saltire)
            results = parser.DetectKeywords(new MemoryStream(Encoding.UTF8.GetBytes((string)BlazonRepository.GarthshoreofthatIlk)));
            Assert.Collection(results,
                keyword => Assert.Equal("Argent", keyword.Value),
                keyword => Assert.Equal(",", keyword.Value),
                keyword => Assert.Equal("a", keyword.Value),
                keyword => Assert.Equal("saltire", keyword.Value),
                keyword => Assert.Equal("between", keyword.Value),
                keyword => Assert.Equal("four", keyword.Value),
                keyword => Assert.Equal("holly", keyword.Value),
                keyword => Assert.Equal("laves", keyword.Value),
                keyword => Assert.Equal("Vert", keyword.Value));

            //wildcards, with complex charge with an ordinary with ordinary properties, and "debruised"
            results = parser.DetectKeywords(new MemoryStream(Encoding.UTF8.GetBytes((string)BlazonRepository.OrdinaryBatonSinister)));
            Assert.Collection(results,
                keyword => Assert.Equal("Argent", keyword.Value),
                keyword => Assert.Equal(",", keyword.Value),
                keyword => Assert.Equal("a", keyword.Value),
                keyword => Assert.Equal("galley", keyword.Value),
                keyword => Assert.Equal("sable", keyword.Value),
                keyword => Assert.Equal(",", keyword.Value),
                keyword => Assert.Equal("sails", keyword.Value),
                keyword => Assert.Equal("furled", keyword.Value),
                keyword => Assert.Equal(",", keyword.Value),
                keyword => Assert.Equal("flags", keyword.Value),
                keyword => Assert.Equal("and", keyword.Value),
                keyword => Assert.Equal("pinnets", keyword.Value),
                keyword => Assert.Equal("flying", keyword.Value),
                keyword => Assert.Equal("and", keyword.Value),
                keyword => Assert.Equal("oars", keyword.Value),
                keyword => Assert.Equal("in", keyword.Value),
                keyword => Assert.Equal("action", keyword.Value),
                keyword => Assert.Equal(",", keyword.Value),
                keyword => Assert.Equal("debruised", keyword.Value),
                keyword => Assert.Equal("with", keyword.Value),
                keyword => Assert.Equal("a", keyword.Value),
                keyword => Assert.Equal("baton sinister", keyword.Value),
                keyword => Assert.Equal("couped", keyword.Value),
                keyword => Assert.Equal("gules", keyword.Value));

            //charges within charges
            results = parser.DetectKeywords(new MemoryStream(Encoding.UTF8.GetBytes((string)BlazonRepository.OrdinaryRibbon)));
            Assert.Collection(results,
                keyword => Assert.Equal("Or", keyword.Value),
                keyword => Assert.Equal(",", keyword.Value),
                keyword => Assert.Equal("lion", keyword.Value),
                keyword => Assert.Equal("rampant", keyword.Value),
                keyword => Assert.Equal("gules", keyword.Value),
                keyword => Assert.Equal(",", keyword.Value),
                keyword => Assert.Equal("surmounted", keyword.Value),
                keyword => Assert.Equal("of", keyword.Value),
                keyword => Assert.Equal("a", keyword.Value),
                keyword => Assert.Equal("ribbon", keyword.Value),
                keyword => Assert.Equal("sable", keyword.Value),
                keyword => Assert.Equal(";", keyword.Value),
                keyword => Assert.Equal("within", keyword.Value),
                keyword => Assert.Equal("a", keyword.Value),
                keyword => Assert.Equal("bordure", keyword.Value),
                keyword => Assert.Equal("azure", keyword.Value),
                keyword => Assert.Equal("charged", keyword.Value),
                keyword => Assert.Equal("with", keyword.Value),
                keyword => Assert.Equal("three", keyword.Value),
                keyword => Assert.Equal("boars", keyword.Value),
                keyword => Assert.Equal("heads", keyword.Value),
                keyword => Assert.Equal("erased", keyword.Value),
                keyword => Assert.Equal(",", keyword.Value),
                keyword => Assert.Equal("or", keyword.Value));

            //ordinary within ordinary, fur
            results = parser.DetectKeywords(new MemoryStream(Encoding.UTF8.GetBytes((string)BlazonRepository.OrdinaryElliotScotland)));
            Assert.Collection(results,
                keyword => Assert.Equal("Gules", keyword.Value),
                keyword => Assert.Equal(";", keyword.Value),
                keyword => Assert.Equal("on", keyword.Value),
                keyword => Assert.Equal("a", keyword.Value),
                keyword => Assert.Equal("bend", keyword.Value),
                keyword => Assert.Equal("engrailed", keyword.Value),
                keyword => Assert.Equal("or", keyword.Value),
                keyword => Assert.Equal("a", keyword.Value),
                keyword => Assert.Equal("baton", keyword.Value),
                keyword => Assert.Equal("azure", keyword.Value),
                keyword => Assert.Equal(";", keyword.Value),
                keyword => Assert.Equal("within", keyword.Value),
                keyword => Assert.Equal("a", keyword.Value),
                keyword => Assert.Equal("bordure", keyword.Value),
                keyword => Assert.Equal("vair", keyword.Value));

            //overall
            results = parser.DetectKeywords(new MemoryStream(Encoding.UTF8.GetBytes((string)BlazonRepository.OrdinaryRethelMazarin)));
            Assert.Collection(results,
                keyword => Assert.Equal("Azure", keyword.Value),
                keyword => Assert.Equal(",", keyword.Value),
                keyword => Assert.Equal("a", keyword.Value),
                keyword => Assert.Equal("fasces", keyword.Value),
                keyword => Assert.Equal("or", keyword.Value),
                keyword => Assert.Equal(",", keyword.Value),
                keyword => Assert.Equal("bound", keyword.Value),
                keyword => Assert.Equal("argent", keyword.Value),
                keyword => Assert.Equal(",", keyword.Value),
                keyword => Assert.Equal("the", keyword.Value),
                keyword => Assert.Equal("axe", keyword.Value),
                keyword => Assert.Equal("of", keyword.Value),
                keyword => Assert.Equal("the", keyword.Value),
                keyword => Assert.Equal("same", keyword.Value),
                keyword => Assert.Equal(",", keyword.Value),
                keyword => Assert.Equal("a", keyword.Value),
                keyword => Assert.Equal("fess", keyword.Value),
                keyword => Assert.Equal("gules", keyword.Value),
                keyword => Assert.Equal(",", keyword.Value),
                keyword => Assert.Equal("brochant", keyword.Value),
                keyword => Assert.Equal("overall", keyword.Value),
                keyword => Assert.Equal("and", keyword.Value),
                keyword => Assert.Equal("ch.", keyword.Value),
                keyword => Assert.Equal("three", keyword.Value),
                keyword => Assert.Equal("mullets", keyword.Value),
                keyword => Assert.Equal("or", keyword.Value));

            //quarters in quarters, charge orientation (property - in pale)
            results = parser.DetectKeywords(new MemoryStream(Encoding.UTF8.GetBytes((string)BlazonRepository.DivisionQuarterInQuarter)));
            Assert.Collection(results,
                keyword => Assert.Equal("Quarterly", keyword.Value),
                keyword => Assert.Equal(",", keyword.Value),
                keyword => Assert.Equal("I", keyword.Value),
                keyword => Assert.Equal("and", keyword.Value),
                keyword => Assert.Equal("IV", keyword.Value),
                keyword => Assert.Equal(",", keyword.Value),
                keyword => Assert.Equal("quarterly", keyword.Value),
                keyword => Assert.Equal("1st", keyword.Value),
                keyword => Assert.Equal("and", keyword.Value),
                keyword => Assert.Equal("4th", keyword.Value),
                keyword => Assert.Equal("Azure", keyword.Value),
                keyword => Assert.Equal("three", keyword.Value),
                keyword => Assert.Equal("fleurs", keyword.Value),
                keyword => Assert.Equal("de", keyword.Value),
                keyword => Assert.Equal("lys", keyword.Value),
                keyword => Assert.Equal("Or", keyword.Value),
                keyword => Assert.Equal(",", keyword.Value),
                keyword => Assert.Equal("2nd", keyword.Value),
                keyword => Assert.Equal("and", keyword.Value),
                keyword => Assert.Equal("3rd", keyword.Value),
                keyword => Assert.Equal("Gules", keyword.Value),
                keyword => Assert.Equal("three", keyword.Value),
                keyword => Assert.Equal("lions", keyword.Value),
                keyword => Assert.Equal("passant", keyword.Value),
                keyword => Assert.Equal("guardant", keyword.Value),
                keyword => Assert.Equal("in", keyword.Value),
                keyword => Assert.Equal("pale", keyword.Value),
                keyword => Assert.Equal("Or", keyword.Value),
                keyword => Assert.Equal(";", keyword.Value),
                keyword => Assert.Equal("II", keyword.Value),
                keyword => Assert.Equal("Or", keyword.Value),
                keyword => Assert.Equal("a", keyword.Value),
                keyword => Assert.Equal("lion", keyword.Value),
                keyword => Assert.Equal("rampant", keyword.Value),
                keyword => Assert.Equal("within", keyword.Value),
                keyword => Assert.Equal("a", keyword.Value),
                keyword => Assert.Equal("tressure", keyword.Value),
                keyword => Assert.Equal("flory", keyword.Value),
                keyword => Assert.Equal("counter", keyword.Value),
                keyword => Assert.Equal("flory", keyword.Value),
                keyword => Assert.Equal("Gules", keyword.Value),
                keyword => Assert.Equal(";", keyword.Value),
                keyword => Assert.Equal("III", keyword.Value),
                keyword => Assert.Equal("Azure", keyword.Value),
                keyword => Assert.Equal("a", keyword.Value),
                keyword => Assert.Equal("harp", keyword.Value),
                keyword => Assert.Equal("Or", keyword.Value),
                keyword => Assert.Equal("stringed", keyword.Value),
                keyword => Assert.Equal("Argent", keyword.Value),
                keyword => Assert.Equal(".", keyword.Value));


            //division (bend sinister)
            results = parser.DetectKeywords(new MemoryStream(Encoding.UTF8.GetBytes((string)BlazonRepository.DivisionBendSinister)));
            Assert.Collection(results,
                keyword => Assert.Equal("per", keyword.Value),
                keyword => Assert.Equal("bend sinister", keyword.Value),
                keyword => Assert.Equal("Argent", keyword.Value),
                keyword => Assert.Equal("and", keyword.Value),
                keyword => Assert.Equal("gules", keyword.Value));

            //relative position (on object an object)
            //relative positions (between 2 objects in position and an object in position)
            //cross with position (issuing out of)
            //refactoring of properties (both property)
            results = parser.DetectKeywords(new MemoryStream(Encoding.UTF8.GetBytes((string)BlazonRepository.Bisket)));
            Assert.Collection(results,
                keyword => Assert.Equal("Argent", keyword.Value),
                keyword => Assert.Equal(",", keyword.Value),
                keyword => Assert.Equal("on", keyword.Value),
                keyword => Assert.Equal("a", keyword.Value),
                keyword => Assert.Equal("chevron", keyword.Value),
                keyword => Assert.Equal("engrailed", keyword.Value),
                keyword => Assert.Equal("sable", keyword.Value),
                keyword => Assert.Equal("between", keyword.Value),
                keyword => Assert.Equal("two", keyword.Value),
                keyword => Assert.Equal("cinquefoils", keyword.Value),
                keyword => Assert.Equal("Gules", keyword.Value),
                keyword => Assert.Equal("in", keyword.Value),
                keyword => Assert.Equal("chief", keyword.Value),
                keyword => Assert.Equal("and", keyword.Value),
                keyword => Assert.Equal("a", keyword.Value),
                keyword => Assert.Equal("mullet", keyword.Value),
                keyword => Assert.Equal("Azure", keyword.Value),
                keyword => Assert.Equal("in", keyword.Value),
                keyword => Assert.Equal("base", keyword.Value),
                keyword => Assert.Equal("a", keyword.Value),
                keyword => Assert.Equal("cross", keyword.Value),
                keyword => Assert.Equal("crosslet", keyword.Value),
                keyword => Assert.Equal("fitchy", keyword.Value),
                keyword => Assert.Equal("issuing", keyword.Value),
                keyword => Assert.Equal("out", keyword.Value),
                keyword => Assert.Equal("of", keyword.Value),
                keyword => Assert.Equal("a", keyword.Value),
                keyword => Assert.Equal("crescent", keyword.Value),
                keyword => Assert.Equal("both", keyword.Value),
                keyword => Assert.Equal("Argent", keyword.Value));

        }

        [Fact]
        public void TestFormatResourceKeywords()
        {
            //temporary test to validate the resources to prepare for the keywords reading
            var kw = new PluginBase.Keyword.Resources("en-GB", Assembly.GetAssembly(typeof(Keywords)));
            var all = kw.GetKeywords();
            //need to check that there is no redundancies in the values (2 keywords cover the same values)
            foreach (var kvp in all)
            {
                foreach (var value in kvp.Value)
                {
                    Trace.TraceInformation(value);
                    Assert.False(ExistSomewhereElse(kvp.Key, value, all));
                }
            }
        }

        private bool ExistSomewhereElse(string key, string value, Dictionary<string, IEnumerable<string>> source)
        {
            return source.Where(entry => entry.Key != key).Any(kvp => kvp.Value.Any(v => v == value));
        }
    }
}
