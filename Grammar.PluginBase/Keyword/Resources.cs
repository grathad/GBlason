using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using Grammar.PluginBase.Token;
using Newtonsoft.Json;
using Utils.Assembly;

namespace Grammar.PluginBase.Keyword
{
    /// <inheritdoc />
    /// <summary>
    /// This is the base to access the resources and values for the plugins that are using the default architecture
    /// </summary>
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class Resources : IResources
    {
        internal virtual Format Root { get; private set; }

        internal readonly string ResourceName;
        internal readonly Assembly Assembly;

        public const string DefaultResourceFileName = "Keywords";

        internal virtual void LoadResourceInMemory()
        {
            using (var stream = Assembly.GetManifestResourceStream($"{ResourceName}"))
            {
                if (stream == null)
                {
                    throw new MissingManifestResourceException($"{ResourceName}");
                }
                using (var reader = new StreamReader(stream))
                {
                    var serializer = new JsonSerializer();
                    using (var jsonTextReader = new JsonTextReader(reader))
                    {
                        Root = serializer.Deserialize<Format>(jsonTextReader);
                    }
                }
            }
        }

        /// <summary>
        /// Create a new instance of resources for the given resource file name, and the given assembly.
        /// The file will be loaded from the given assembly, looking for the given filename.
        /// Only one file will be loaded.
        /// The format of the file is expected to be json as defined by the <see cref="Format"/>
        /// </summary>
        /// <param name="resourceName">The file name to look to load within the assembly. The file name format is <b>AssemblyName.FileName</b></param>
        /// <param name="assembly">The assembly containing the file as an internal component</param>
        public Resources(string resourceName, Assembly assembly)
        {
            ResourceName = resourceName;
            Assembly = assembly;
        }

        /// <summary>
        /// Create a new instance of resources for the given assembly using the following default expectations:
        /// * A file in the assembly is present and have the name defined in <see cref="DefaultResourceFileName"/>
        /// * The file extension is the same as the first one defined in the <see cref="NeutralResourcesLanguageAttribute"/> for the passed assembly
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
            ResourceName = $"{Assembly.Assembly().GetName().Name}.{DefaultResourceFileName}.{currentCulture.CultureName}";
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
            ResourceName = $"{Assembly.Assembly().GetName().Name}.{DefaultResourceFileName}.{currentCulture.CultureName}";
        }

        /// <inheritdoc />
        /// <summary>
        /// Return all the keywords and their related identification values
        /// </summary>
        /// <returns>A dictionary where the key is the keyword identifier and the value is the list of potential matches</returns>
        public Dictionary<string, IEnumerable<string>> GetKeywords()
        {
            if (Root == null)
            {
                LoadResourceInMemory();
            }
            return Root.Keywords;
        }

        /// <inheritdoc />
        /// <summary>
        /// Return all the keywords and their related identification values
        /// </summary>
        /// <returns>A dictionary where the key is the keyword identifier and the value is the list of potential matches</returns>
        public virtual Dictionary<string, IEnumerable<IEnumerable<string>>> GetTokens()
        {
            if (Root == null)
            {
                LoadResourceInMemory();
            }
            return Root.Tokens;
        }

        /// <inheritdoc />
        /// <summary>
        /// Return the tokens for an entry in the resource matching the given name (matrix is returned)
        /// </summary>
        /// <param name="name">The name of the entry in the resource to get the token for</param>
        /// <returns>The matric of key words related to the token name if it exists in the resource, other wise, null</returns>
        public IEnumerable<IEnumerable<string>> GetTokens(TokenNames name)
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
            if (Root == null)
            {
                LoadResourceInMemory();
            }
            return Root.MergeableWords;
        }
    }
}
