using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Grammar
{
    /// <summary>
    /// The Grammar Parser is the entry point of the Grammar to turn a blazon into its formated version
    /// The grammar to be operative need as an entry point a EBNF parsed schema <see cref="Ebnf.Parser"/>
    /// From there all the EBNF rules that are final (no children) will be associated with 2 other potential input:
    /// 1. The custom parser (plugin for custom rules) which means that the rule is executed on the code rather than the grammar
    /// 2. The list of keywords (or text based final leaves and their parent rules) this is due to historic and optimization reasons
    /// </summary>
    public class GrammarParser
    {

        public IResources Resources { get; private set; }

        public GrammarParser()
        {
        }

        public void LoadResourcesFromAssembly(Assembly assembly)
        {
            Resources = new Resources(assembly);
        }
    }
}
