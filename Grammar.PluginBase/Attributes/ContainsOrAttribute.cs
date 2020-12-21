using System;
using System.Reflection;
using Grammar.PluginBase.Parser;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;

namespace Grammar.PluginBase.Attributes
{
    /// <summary>
    /// Represent a token that is only containing one of the list of children defined in the names
    /// </summary>
    public class ContainsOrAttribute : GenericParserAttribute
    {
        public virtual TokenNames[] Names { get; }

        /// <summary>
        /// Define the attribute. one of the elememts defined in the list of children is expected to be present
        /// </summary>
        /// <param name="names">the list of children</param>
        public ContainsOrAttribute(params TokenNames[] names)
        {
            Names = names;
        }

        public override TypeInfo GetGenericParserType()
        {
            return typeof(ContainerOrParser).GetTypeInfo();
        }

        public override ContainerParser CreateGenericParserInstance(TokenNames current, IParserPilot pilot)
        {
            return new ContainerOrParser(current, pilot, Names);
        }
    }
}