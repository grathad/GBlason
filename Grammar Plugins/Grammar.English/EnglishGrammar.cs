using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;

[assembly: InternalsVisibleTo("Grammar.English.Test.Unit")]
[assembly: InternalsVisibleTo("Grammar.English.Test.Integration.TokenParsers")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Grammar.English
{
    /// <inheritdoc />
    /// <summary>
    /// First version of a english grammar parser, implementing the <see cref="T:Grammar.IGrammarParser" /> and using the architecture defined along the
    /// Grammar parser helper namespace
    /// </summary>
    [Export(typeof(IGrammarParser))]
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class EnglishGrammar : IGrammarParser
    {
        /// <summary>
        /// The detector used for the grammar parser
        /// </summary>
        public virtual PluginBase.Keyword.IDetector Detector { get; }

        /// <summary>
        /// Default constructor initializing the <see cref="Detector"/> with a new empty <see cref="PluginBase.Keyword.Detector"/>
        /// </summary>
        public EnglishGrammar()
        {
            Detector = new PluginBase.Keyword.Detector(new List<ParserError>(), new PluginBase.Keyword.Resources());
        }

        /// <summary>
        /// Constructor that initialize the <see cref="Detector"/> wih the givent parameter
        /// </summary>
        /// <param name="detector">the detector to use when parsing</param>
        public EnglishGrammar(PluginBase.Keyword.IDetector detector)
        {
            Detector = detector;
        }

        internal virtual IParserPilot CreateParser(IParserFactory factory, IList<PluginBase.Keyword.ParsedKeyword> keywords)
        {
            return new ParserPilot(factory, keywords);
        }

        /// <inheritdoc />
        /// <summary>
        /// <see cref="IGrammarParser.Parse(string)"/>
        /// </summary>
        /// <param name="blazon"><see cref="IGrammarParser.Parse(string)"/></param>
        /// <returns><see cref="IGrammarParser.Parse(string)"/></returns>
        public virtual ParsingResult Parse(string blazon)
        {
            return Parse(new MemoryStream(Encoding.UTF8.GetBytes(blazon)));
        }

        /// <inheritdoc />
        /// <remarks>The default encoding used if non are provided is <see cref="Encoding.UTF8"/></remarks>
        public virtual ParsingResult Parse(Stream blazon)
        {
            return Parse(blazon, Encoding.UTF8);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">If either of the passed parameters are null</exception>
        /// <exception cref="NullReferenceException">If the detector is null</exception>
        public virtual ParsingResult Parse(Stream blazon, Encoding encoding)
        {
            if (blazon == null)
            {
                throw new ArgumentNullException(nameof(blazon));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }
            if (Detector == null)
            {
                throw new NullReferenceException(nameof(Detector));
            }

            var keyWords = Detector.DetectKeywords(blazon, encoding)?.ToList();

            if (!(keyWords?.Any() ?? false))
            {
                return null;
            }

            var pilot = CreateParser(new DefaultParserFactory(), keyWords);

            if (pilot == null)
            {
                throw new NullReferenceException(nameof(pilot));
            }

            var position = TokenParsingPosition.DefaultStartingPosition;
            var parseResult = pilot.Parse(position);
            var result = parseResult?.ResultToken as ContainerToken;
            
            //todo implement the parsing result and format support from the base result token
            return null;
        }

        /// <inheritdoc />
        /// <summary>
        /// <see cref="IGrammarParser.ParseAsync(string)"/>
        /// </summary>
        /// <param name="blazon"><see cref="IGrammarParser.ParseAsync(string)"/></param>
        /// <returns><see cref="IGrammarParser.ParseAsync(string)"/></returns>
        public virtual Task<ParsingResult> ParseAsync(string blazon)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        /// <summary>
        /// <see cref="IGrammarParser.ParseAsync(Stream)"/>
        /// </summary>
        /// <param name="blazon"><see cref="IGrammarParser.ParseAsync(Stream)"/></param>
        /// <returns><see cref="IGrammarParser.ParseAsync(Stream)"/></returns>
        public virtual Task<ParsingResult> ParseAsync(Stream blazon)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        /// <summary>
        /// <see cref="IGrammarParser.ParseAsync(Stream, Encoding)"/>
        /// </summary>
        /// <param name="blazon"><see cref="IGrammarParser.ParseAsync(Stream, Encoding)"/></param>
        /// <param name="encoding"><see cref="IGrammarParser.ParseAsync(Stream, Encoding)"/></param>
        /// <returns><see cref="IGrammarParser.ParseAsync(Stream, Encoding)"/></returns>
        public virtual Task<ParsingResult> ParseAsync(Stream blazon, Encoding encoding)
        {
            throw new NotImplementedException();
        }
    }
}
