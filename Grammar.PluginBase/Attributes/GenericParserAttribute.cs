using System;
using System.Reflection;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;

namespace Grammar.PluginBase.Attributes
{
    /// <summary>
    /// Represent the attributes that can be used to generate a parser for a given token (generic parsers)
    /// </summary>
    public abstract class GenericParserAttribute : Attribute
    {
        /// <summary>
        /// Return the generic parser type that will be used to represent the token
        /// </summary>
        /// <returns>The type onfo for the parser</returns>
        public abstract TypeInfo GetGenericParserType();

        /// <summary>
        /// Return the instance of a parser of the generic type defined
        /// </summary>
        /// <returns>a new instance of the required type</returns>
        public abstract ContainerParser CreateGenericParserInstance(TokenNames currentToken, IParserPilot pilot);
    }
}