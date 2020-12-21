using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;

namespace Grammar.English.Test.Integration.TokenParsers
{
    public class IntegrationTest
    {
        protected List<ParserError> Errors { get; set; }


        protected IParserPilot GetPilot(string origin)
        {
            using (var source = new MemoryStream(Encoding.UTF8.GetBytes(origin)))
            {
                var testAssembly = Assembly.GetAssembly(typeof(EnglishGrammar));
                var resources = new PluginBase.Keyword.Resources(testAssembly);
                var keyWords = new Detector(Errors, resources).DetectKeywords(source).ToList();
                return new ParserPilot(new DefaultParserFactory(testAssembly), keyWords);
            }
        }

    }
}