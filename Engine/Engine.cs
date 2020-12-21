using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Reflection;
using Grammar;
using Utils.Composition.Contracts;
using Utils.Composition.Implementations;
using Utils.IO;
using Utils.Runtime;
using Utils.TypeHelper;

namespace Engine
{
    /// <summary>
    /// This is the engine that is used to execute "parsing" on a blazon string or stream.
    /// The role of the engine is to use a defined avaiable plugin, and execute it
    /// </summary>
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class Engine
    {
        /// <summary>
        /// Default constructor that initialize the engine with ALL the compatible plugins available
        /// </summary>
        public Engine(
            bool initialize = true,
            IContainerConfiguration config = null,
            IAssemblyLoadContext loadContext = null)
        {
            ContainerConfig = config ?? new ContainerConfigurationWrapper();
            LoadContext = loadContext ?? new AssemblyLoadContextWrapper();
            if (initialize)
            {
                Initialize();
            }
        }

        /// <summary>
        /// Default auto loading of compatible plugins for the engine
        /// </summary>
        private void Initialize()
        {
            LoadPlugin(CompatiblePlugins());
        }

        /// <summary>
        /// 
        /// </summary>
        protected IContainerConfiguration ContainerConfig { get; }
        /// <summary>
        /// 
        /// </summary>
        protected IAssemblyLoadContext LoadContext { get; }

        /// <summary>
        /// Contains the name of the folder that should contains the plugins
        /// </summary>
        public const string PluginDirectory = "Plugins";

        /// <summary>
        /// Load or Reload the plugins available for parsing in the engine
        /// </summary>
        /// <param name="assemblies">The assemblies to load in the engine</param>
        public virtual void LoadPlugin(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }
            var configuration = ContainerConfig.WithAssemblies(assemblies);
            //    .WithAssembly(assemblies.FirstOrDefault(a => a.FullName == name));
            using (var container = configuration.CreateContainer())
            {
                Parsers = container.GetExports<IGrammarParser>();
            }
        }

        /// <summary>
        /// List all the plugins in the expected location that are compatible with the current version of the engine
        /// </summary>
        /// <returns>A list of all the plugins that contains the implementation of at least one of the contract used in this engine</returns>
        public virtual IEnumerable<Assembly> CompatiblePlugins()
        {
            var executableLocation = AppContext.BaseDirectory;
            var path = Path.Combine(Path.GetDirectoryName(executableLocation), PluginDirectory);
            if (!Directory.Exists(path))
            {
                return null;
            }
            var assemblies = Directory
                .GetFiles(path, "*.dll", System.IO.SearchOption.AllDirectories)
                .Select(LoadContext.LoadFromAssemblyPath)
                .ToList();
            //typeof(MyType).GetInterfaces().Contains(typeof(IMyInterface))
            return assemblies.Where(a => a.ExportedTypes.Any(t => t.Type().ContainsInterface<IGrammarParser>()));
        }

        /// <summary>
        /// Those are the parser contract implementations (plugins) that can potentially be used for parsing
        /// </summary>
        [ImportMany]
        public IEnumerable<IGrammarParser> Parsers { get; set; }

        /// <summary>
        /// Parse a blazon and return the result of the parsing. <see cref="IGrammarParser"/>
        /// </summary>
        /// <param name="blazon">The string to parse and turn into a format</param>
        /// <param name="pluginToUse">The plugin to use for parsing, if not defined then the method uses the first one of <see cref="Parsers"/> defined</param>
        /// <returns>A parsing result as defined in <see cref="ParsingResult"/></returns>
        /// <exception cref="ArgumentNullException">
        /// if the <paramref name="blazon"/> is null or empty, 
        /// or if the <paramref name="pluginToUse"/> is null AND the <see cref="Parsers"/> is empty
        /// </exception>
        public virtual ParsingResult Parse(string blazon, IGrammarParser pluginToUse = null)
        {
            if (string.IsNullOrEmpty(blazon))
            {
                throw new ArgumentNullException(nameof(blazon));
            }
            if (pluginToUse == null)
            {
                pluginToUse = Parsers?.FirstOrDefault();
            }
            if (pluginToUse == null)
            {
                throw new ArgumentNullException(nameof(pluginToUse));
            }
            return pluginToUse.Parse(blazon);
        }
    }
}