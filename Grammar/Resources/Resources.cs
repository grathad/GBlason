using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using Newtonsoft.Json;
using Utils.Assembly;

namespace Grammar
{
    /// <inheritdoc />
    /// <summary>
    /// This is the base to access the resources and values for the plugins that are using the default architecture
    /// </summary>
    public class Resources : IResources
    {
        internal virtual Format Keywords { get; private set; }
        internal virtual Ebnf.Parser RootGrammar { get; private set; }

        public readonly string KeywordResourceName;
        public readonly string GrammarResourceName;
        public readonly Assembly Assembly;

        public const string DefaultKeywordsFileName = "Keywords";
        public const string DefaultGrammarFileName = "GrammarDefinition";
        public const string DefaultGrammarFileFormatExtension = "ebnf";

        /// <summary>
        /// Internal helper to load all the resources from the assembly itself
        /// </summary>
        internal virtual void LoadKeywordsFromAssembly()
        {
            using (var stream = Assembly.GetManifestResourceStream($"{KeywordResourceName}"))
            {
                if (stream == null)
                {
                    throw new MissingManifestResourceException($"{KeywordResourceName}");
                }
                LoadKeywords(stream);
            }
        }

        /// <summary>
        /// Load a collection of keywords to represent this resource.
        /// This is the only way to overwrite the assembly resources
        /// Make sure to manually call this first, if not the default getter of resources will look into the assembly
        /// </summary>
        /// <param name="source">The stream from which to read the key word resources</param>
        public void LoadKeywords(Stream source)
        {
            using (var reader = new StreamReader(source))
            {
                var serializer = new JsonSerializer();
                using (var jsonTextReader = new JsonTextReader(reader))
                {
                    Keywords = serializer.Deserialize<Format>(jsonTextReader);
                }
            }
        }

        /// <summary>
        /// Internal helper to load all the resources from the assembly itself
        /// </summary>
        internal virtual void LoadGrammarFromAssembly()
        {
            using (var stream = Assembly.GetManifestResourceStream($"{GrammarResourceName}"))
            {
                if (stream == null)
                {
                    throw new MissingManifestResourceException($"{GrammarResourceName}");
                }
                LoadGrammar(stream);
            }
        }

        /// <summary>
        /// Load a collection of keywords to represent this resource.
        /// This is the only way to overwrite the assembly resources
        /// Make sure to manually call this first, if not the default getter of resources will look into the assembly
        /// </summary>
        /// <param name="source">The stream from which to read the key word resources</param>
        public void LoadGrammar(Stream source)
        {
            RootGrammar = new Ebnf.Parser();
            RootGrammar.Parse(source);
        }

        /// <summary>
        /// Create a new instance of resources for the given resource file name, and the given assembly.
        /// The file will be loaded from the given assembly, looking for the given filename.
        /// Only one file will be loaded.
        /// The format of the file is expected to be json as defined by the <see cref="Format"/>
        /// </summary>
        /// <param name="keywordsResource">The file name to look to load the keywords from within the assembly. The resource should be formatted as Json</param>
        /// <param name="grammarResource">The file name to look to load the grammar from within the assembly. The resource should be formatted as ebnf <see cref="Ebnf.Parser"/></param>
        /// <param name="assembly">The assembly containing the file as an internal component</param>
        public Resources(Assembly assembly, string keywordsResource, string grammarResource)
        {
            KeywordResourceName = keywordsResource;
            GrammarResourceName = grammarResource;
            Assembly = assembly;
        }

        /// <summary>
        /// Create a new instance of resources for the given assembly using the following default expectations:
        /// * The keyword resource, is accessible as the loaded assembly resource with the corresponding default name:
        /// <see cref="DefaultKeywordsFileName"/>.<see cref="NeutralResourcesLanguageAttribute"/> (of the assembly)
        /// * A file in the assembly is present and have the name defined in <see cref="DefaultGrammarFileName"/>
        /// </summary>
        /// <example>
        /// MyAssembly.Name.Keywords.en-GB
        /// </example>
        /// <param name="assembly">The assembly from which to initialize the resource</param>
        /// <exception cref="NullReferenceException">If the calling assembly does not have a defined <see cref="NeutralResourcesLanguageAttribute"/></exception>
        public Resources(Assembly assembly)
        {
            NeutralResourcesLanguageAttribute currentCulture;
            try
            {
                currentCulture = assembly.Assembly().GetCustomAttributes<NeutralResourcesLanguageAttribute>().FirstOrDefault();
                if (currentCulture == null)
                {
                    throw new NullReferenceException($"The assembly needs to contain a {nameof(NeutralResourcesLanguageAttribute)}");
                }
            }
            catch (InvalidCastException ice)
            {
                //we are passing an assembly without NeutralResourcesLanguageAttribute thus the get custom attributes fails. same as having a currentculture as null
                throw new Exception($"The assembly needs to contain a {nameof(NeutralResourcesLanguageAttribute)}", ice);
            }
            Assembly = assembly;
            var assemblyName = Assembly.Assembly().GetName().Name;
            KeywordResourceName = $"{assemblyName}.{DefaultKeywordsFileName}.{currentCulture.CultureName}";
            GrammarResourceName = $"{assemblyName}.{DefaultGrammarFileName}.{currentCulture.CultureName}.{DefaultGrammarFileFormatExtension}";
        }

        /// <summary>
        /// Create a new instance of resources for the calling assembly
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// If the calling assembly does not have a defined <see cref="NeutralResourcesLanguageAttribute"/>
        /// </exception>
        public Resources()
        {
            Assembly = Assembly.GetCallingAssembly();
            var currentCulture = Assembly.Assembly().GetCustomAttributes<NeutralResourcesLanguageAttribute>().FirstOrDefault();
            if (currentCulture == null)
            {
                throw new NullReferenceException("The assembly needs to contain a NeutralResourcesLanguageAttribute");
            }
            var assemblyName = Assembly.Assembly().GetName().Name;
            KeywordResourceName = $"{assemblyName}.{DefaultKeywordsFileName}.{currentCulture.CultureName}";
            GrammarResourceName = $"{assemblyName}.{DefaultGrammarFileName}.{currentCulture.CultureName}.{DefaultGrammarFileFormatExtension}";
        }

        /// <inheritdoc />
        /// <summary>
        /// Return all the keywords and their related identification values
        /// </summary>
        /// <returns>A dictionary where the key is the keyword identifier and the value is the list of potential matches</returns>
        public Dictionary<string, IEnumerable<string>> GetKeywords()
        {
            if (Keywords == null)
            {
                LoadKeywordsFromAssembly();
            }
            return Keywords.Keywords;
        }

        /// <inheritdoc />
        /// <summary>
        /// Return all the keywords and their related identification values
        /// </summary>
        /// <returns>A dictionary where the key is the keyword identifier and the value is the list of potential matches</returns>
        public virtual Dictionary<string, IEnumerable<IEnumerable<string>>> GetTokens()
        {
            if (Keywords == null)
            {
                LoadKeywordsFromAssembly();
            }
            return Keywords.Tokens;
        }

        /// <inheritdoc />
        /// <summary>
        /// Return the tokens for an entry in the resource matching the given name (matrix is returned)
        /// </summary>
        /// <param name="name">The name of the entry in the resource to get the token for</param>
        /// <returns>The matric of key words related to the token name if it exists in the resource, other wise, null</returns>
        public IEnumerable<IEnumerable<string>> GetTokens(string name)
        {
            var tokens = GetTokens();

            return !tokens.ContainsKey(name.ToString())
                ? null
                : tokens[name.ToString()];
        }

        /// <inheritdoc />
        /// <summary>
        /// Return all the mergeable words
        /// </summary>
        /// <returns>a list of all merge able words</returns>
        public List<string> GetMergeableWords()
        {
            if (Keywords == null)
            {
                LoadKeywordsFromAssembly();
            }
            return Keywords.MergeableWords;
        }

        public Ebnf.Parser GetGrammar()
        {
            if (RootGrammar == null)
            {
                LoadGrammarFromAssembly();
            }
            return RootGrammar;
        }
    }
}
