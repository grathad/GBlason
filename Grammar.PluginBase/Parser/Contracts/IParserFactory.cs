using System.Reflection;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Token;

namespace Grammar.PluginBase.Parser.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IParserFactory
    {
        /// <summary>
        /// Return the type expected to generate the parser for a given <see cref="TokenNames"/>
        /// <remarks>Do not generate the instance itself, to let the caller have the option to manage its own cache to avoid duplicating the parsers</remarks>
        /// </summary>
        /// <param name="type">The type to get the parser from</param>
        /// <returns>The type for the parser or null if the type is not available</returns>
        TypeInfo GetParser(TokenNames type);

        /// <summary>
        /// Factory entry to create an instance of a parser from a type. 
        /// Independent from the <see cref="GetParser(TokenNames)"/> that return a type from a <see cref="TokenNames"/>
        /// </summary>
        /// <param name="type">The enumeration that represent the type of parser to create</param>
        /// <param name="pilot">The pilot used by the parser</param>
        /// <param name="resources">[Optional] The resources used for some parsers to detect and compare token values with keywords</param>
        /// <returns>The new instance if the input are valid, or null</returns>
        ParserBase CreateParser(TokenNames type, IParserPilot pilot, IResources resources = null);
    }
}