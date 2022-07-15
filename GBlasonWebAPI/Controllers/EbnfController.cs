using Ebnf;
using GBlasonWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.Json;

namespace GBlasonWebAPI.Controllers
{
    [ApiController]
    [Route("api/ebnf")]
    public class EbnfController : ControllerBase
    {
        private static readonly Parser parsedEbnf = new Parser();

        private static Stream? rawEbnf;
        private string rawEbnfLoadingError = string.Empty;

        private const string EBNF_FILE_NAME = "GBlasonWebAPI.Resources.GrammarDefinition.ebnf";

        private readonly ILogger<EbnfController> _logger;

        private static readonly Collection<TreeElementReference> memoryTree = new Collection<TreeElementReference>();

        public EbnfController(ILogger<EbnfController> logger)
        {
            _logger = logger;
            BuildTreeStream();
        }

        private void BuildTreeStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            try
            {
                rawEbnf = assembly.GetManifestResourceStream(EBNF_FILE_NAME);
            }
            catch (Exception e)
            {
                rawEbnfLoadingError = e.Message;
            }
        }

        [HttpGet("raw")]
        public string Raw()
        {
            if (rawEbnf == null)
            {
                return $"Error the EBNF Grammar can't be loaded {rawEbnfLoadingError}";
            }
            else
            {
                if (!rawEbnf.CanRead)
                {
                    BuildTreeStream();
                }
                using (StreamReader reader = new StreamReader(rawEbnf))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        [HttpGet("tree")]
        public string Tree(Guid? head = null)
        {
            if (memoryTree == null || !memoryTree.Any())
            {
                var error = BuildSafeTree();
                if (!string.IsNullOrEmpty(error))
                {
                    return error;
                }
            }

            if (memoryTree == null || !memoryTree.Any())
            {
                return "A problem occured with the creation of the tree, flat list of real element is empty";
            }

            //finding the nodes that we need to produce and return
            var headElement = memoryTree.FirstOrDefault();
            if (head != null)
            {
                foreach (var elem in memoryTree)
                {
                    if (elem.ElementId == head)
                    {
                        headElement = elem;
                        break;
                    }
                }
            }

            if (headElement == null)
            {
                return $"Could not find the element requested {head}";
            }

            //we only return the direct children of the current headElement
            var subtree = new TreeElementReference(headElement);

            //transform it to json

            string jsonString = JsonSerializer.Serialize(subtree);

            //return it to the caller

            return jsonString;
        }

        private string BuildSafeTree()
        {
            if (rawEbnf == null)
            {
                return JsonSerializer.Serialize(new Exception("The EBNF file was not found, unable to build the tree"));
            }
            try
            {
                //build up the normal infinity cycling tree of nodes for the grammar
                if (!rawEbnf.CanRead)
                {
                    BuildTreeStream();
                }
                var elements = parsedEbnf.Parse(rawEbnf);
                if (elements == null)
                {
                    return JsonSerializer.Serialize(new Exception("The EBNF file parsing failed, unable to return a valid tree"));
                }

                var optimizedList = new Collection<TreeElement>();

                foreach (var elem in elements)
                {
                    optimizedList.Add(elem.Optimize());
                }

                //Build up the tree with reference (so that the json format of the cyclic tree object is finite)

                var flatRealElements = new Collection<TreeElementReference>();
                foreach (var elem in optimizedList)
                {
                    memoryTree.Add(new TreeElementReference(elem, flatRealElements));
                }

                return string.Empty;
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Exception($"The following error happened during the tree creation: {e.Message}"));
            }
        }
    }
}