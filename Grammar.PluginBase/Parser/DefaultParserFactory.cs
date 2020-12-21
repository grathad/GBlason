using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Grammar.PluginBase.Attributes;
using Grammar.PluginBase.Keyword;
using Grammar.PluginBase.Parser.Contracts;
using Grammar.PluginBase.Token;
using Utils.Enum;
using Utils.TypeHelper;

namespace Grammar.PluginBase.Parser
{
    /// <summary>
    /// Updated in Issue 25: https://gitlab.com/gblason/webclient/issues/25
    /// This is the entry point that manage the tree of calls.
    /// And optimize the build up of token by memorizing parser instances
    /// <see cref="IParserPilot"/>
    /// </summary>
    public class DefaultParserFactory : IParserFactory
    {
        internal virtual List<TypeInfo> TypeCache { get; } = new List<TypeInfo>();

        internal const string ParserName = "Parser";
        internal virtual Assembly Assembly { get; }
        internal virtual IResources Resources { get; }

        /// <summary>
        /// Create a default parser factory for the given assembly (we can have one factory per plugin)
        /// </summary>
        /// <param name="assembly">The assembly from which to initialize the parser, if null, will initialize from the current assembly (PluginBase) it needs to have a <see cref="NeutralResourcesLanguageAttribute"/> to build the corresponding resources</param>
        /// <param name="resources">The resource to use in the factory if the assembly default resource is not usable</param>
        public DefaultParserFactory(Assembly assembly = null, IResources resources = null)
        {
            assembly = assembly ?? typeof(DefaultParserFactory).GetTypeInfo().Assembly;
            Resources = resources ?? new Resources(assembly);
            Assembly = assembly;
        }

        /// <inheritdoc />
        /// <summary>
        /// Return the type for the given type. By order will return
        /// <list type="bullet">
        /// <element>
        /// Any custom parser which names in the given assembly match the <paramref name="type"/> followed by <see cref="ParserName"/>
        /// </element>
        /// <element>
        /// Any generic parser defined by a <see cref="GenericParserAttribute"/> on the <paramref name="type"/>
        /// </element>
        /// <element>
        /// Last resort if no better items were found would return a <see cref="LeafParser"/>
        /// </element>
        /// </list> 
        /// </summary>
        /// <returns><inheritdoc /></returns>
        public virtual TypeInfo GetParser(TokenNames type)
        {
            if (!TypeCache.Any())
            {
                //initializing the type cache if it was not already done
                TypeCache.AddRange(
                    Assembly.GetTypes()
                    .Where(t => t.Name.EndsWith(ParserName))
                    .Select(t => t.Type().GetTypeInfo()));
            }

            var typeName = $"{type}{ParserName}";
            //trying to find a custom parser
            var customParserType = TypeCache.FirstOrDefault(t => t.Name == typeName);
            if (customParserType != null)
            {
                return customParserType;
            }

            //trying to find a generic parser
            var genericParserType = type.Attributes().GetAttribute<GenericParserAttribute>();

            return genericParserType?.GetGenericParserType()
                ?? typeof(LeafParser).Type().GetTypeInfo();
        }

        /// <summary>
        /// <inheritdoc cref="CreateParser(TokenNames,IParserPilot,IResources)"/>.
        /// <para>
        /// Heavily relies on reflection to create parsers based on a type and on a type behavior definition (simple parsers are generic)
        /// </para>
        /// </summary>
        /// <param name="type"><inheritdoc cref="CreateParser(TokenNames,IParserPilot,IResources)"/></param>
        /// <param name="pilot"><inheritdoc cref="CreateParser(TokenNames,IParserPilot,IResources)"/></param>
        /// <param name="resources"><inheritdoc cref="CreateParser(TokenNames,IParserPilot,IResources)"/></param>
        /// <returns><inheritdoc cref="CreateParser(TokenNames,IParserPilot,IResources)"/></returns>
        /// <exception cref="Exception"><see cref="LeafParser"/></exception>
        /// <exception cref="Exception"><see cref="Activator.CreateInstance(Type)"/></exception>
        /// <exception cref="NotSupportedException">If the type requested is not a child of <see cref="ParserBase"/></exception>
        /// <exception cref="AggregateException">
        /// If none the public constructors for the type requested.
        /// Have a list of parameters that are either
        /// <list type="bullet">
        /// <item>
        /// Defined with a default value
        /// </item>
        /// <item>
        /// Of a compatible type with the one we can handle
        /// </item>
        /// </list> 
        /// </exception>
        public ParserBase CreateParser(
            TokenNames type,
            IParserPilot pilot,
            IResources resources = null)
        {
            //there are 3 types of parsers possible
            //customs, generic and if not specified, leaves
            var parserType = GetParser(type);

            if (!parserType.IsSubclassOf(typeof(ParserBase)))
            {
                throw new NotSupportedException($"The requested type {parserType.Name} from the {type} is not compatible with {nameof(ParserBase)}");
            }

            //we only need to do constructor reflection if we are not in the case of a generic parser
            var generic = type.Attributes().GetAttribute<GenericParserAttribute>();
            if (generic != null)
            {
                return generic.CreateGenericParserInstance(type, pilot);
            }

            //for the other cases, we need to
            //analyze what kind of constructor we need to create
            //we have the following information available
            //the parser factory expected type of input IParserFactory
            //the token name expected type of input TokenNames
            //the resources expected type of input IResources
            resources = resources ?? Resources;

            var supportedTypesAndValues = new Dictionary<Type, object>
            {
                {typeof(IParserFactory), this},
                {typeof(TokenNames), type},
                {typeof(IParserPilot), pilot},
                {typeof(IResources), resources}
            };

            var constructors = parserType.GetConstructors().Where(c => c.IsPublic);

            ConstructorInfo selectedConstructor = null;
            var selectedConstructorParameters = new List<object>();
            var errors = new List<NotSupportedException>();

            //we take the constructors that match our capacity of input.
            //With the most parameters possible
            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();
                if (selectedConstructor == null
                    || selectedConstructor.GetParameters().Length < parameters.Length)
                {
                    var tempParams = new List<object>();
                    var canUse = true;
                    foreach (var parameter in parameters)
                    {
                        if (!supportedTypesAndValues.ContainsKey(parameter.ParameterType))
                        {
                            errors.Add(new NotSupportedException(
                                $"Impossible to use the constructor with {parameters.Length} the parameter {parameter.Name} of type {parameter.ParameterType} is incompatible with the data available"));
                            canUse = false;
                            break;
                        }
                        tempParams.Add(supportedTypesAndValues[parameter.ParameterType]);
                    }
                    if (canUse)
                    {
                        selectedConstructor = constructor;
                        selectedConstructorParameters = new List<object>(tempParams);
                    }
                }
            }
            if (selectedConstructor == null)
            {
                throw new AggregateException(errors);
            }

            return (ParserBase)selectedConstructor.Invoke(selectedConstructorParameters.ToArray());
        }
    }
}