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

        private static Stream? _rawEbnf;
        private string rawEbnfLoadingError = string.Empty;

        private const string EBNF_FILE_NAME = "GBlasonWebAPI.Resources.GrammarDefinition.ebnf";

        private readonly ILogger<EbnfController> _logger;

        private static readonly Collection<TreeElementReference> _memoryTree = new Collection<TreeElementReference>();

        public virtual Collection<TreeElementReference> MemoryTree
        {
            get { return _memoryTree; }
        }

        public virtual Stream? RawEbnf { get => _rawEbnf; set => _rawEbnf = value; }

        public EbnfController(ILogger<EbnfController> logger)
        {
            _logger = logger;
            GetTreeStream();
        }

        [HttpGet("raw")]
        public string Raw()
        {
            if (RawEbnf == null)
            {
                return $"Error the EBNF Grammar can't be loaded {rawEbnfLoadingError}";
            }
            else
            {
                if (!RawEbnf.CanRead)
                {
                    GetTreeStream();
                }
                using (StreamReader reader = new StreamReader(RawEbnf))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        [HttpGet("tree")]
        public string Tree(Guid? head = null)
        {
            var init = InitMemory();
            if (!string.IsNullOrEmpty(init))
            {
                return init;
            }

            if (MemoryTree == null || !MemoryTree.Any())
            {
                return "A problem occured with the creation of the tree, flat list of real element is empty";
            }

            //finding the nodes that we need to produce and return
            TreeElementReference? headElement = null;

            if (head != null)
            {
                foreach (var elem in MemoryTree)
                {
                    if (elem.ElementId == head)
                    {
                        headElement = elem;
                        break;
                    }
                }
                if (headElement == null)
                {
                    return $"Impossible to find the element {head} in the tree";
                }
            }

            else
            {
                headElement = MemoryTree.FirstOrDefault();

                if (headElement == null)
                {
                    return $"Could not find the root element of the tree";
                }
            }

            //we only return the direct children of the current headElement
            var subtree = TreeElementReference.CreateCopy(headElement);

            //transform it to json

            string jsonString = JsonSerializer.Serialize(subtree);

            //return it to the caller

            return jsonString;
        }

        /// <summary>
        /// Return the branch from the leaf all the way to the root, containing only the branch with the main parent of all the nodes required to move to the root
        /// </summary>
        /// <param name="leaf">The leaf from which the branch is returned</param>
        /// <returns>The branch all the way to the root or null if the leaf does not exist or have no link to the root</returns>
        [HttpGet("branch")]
        public string Branch(Guid leaf)
        {
            var init = InitMemory();
            if (!string.IsNullOrEmpty(init))
            {
                return init;
            }
            var leafnode = MemoryTree.FirstOrDefault(tn => tn.ElementId == leaf);
            if (leafnode == null)
            {
                return $"Could not find the leaf with the ID {leaf}";
            }
            var root = MemoryTree.FirstOrDefault();
            if (root == null)
            {
                return "The tree is empty, can't return the path";
            }
            if (root == leafnode)
            {
                var subtree = TreeElementReference.CreateCopy(leafnode, 0);
                return JsonSerializer.Serialize(subtree);
            }

            //trying to reach the root from the leaf
            var branch = new Collection<TreeElementReference>();
            leafnode.FindParent(ref branch, root);

            var trimmedBranch = new Collection<TreeElementReference>();
            //we create the copies to avoid cyclic references for all the level of the branch (but only against the nodes we want)
            foreach(var node in branch)
            {
                trimmedBranch.Add(TreeElementReference.CreateCopy(node));
            }

            return JsonSerializer.Serialize(trimmedBranch);
        }

        /// <summary>
        /// Initialize the tree in memory and return the potential error
        /// </summary>
        /// <returns>An error if the tree can't be build, an empty string in case of success</returns>
        protected string InitMemory()
        {
            if (MemoryTree == null || !MemoryTree.Any())
            {
                var error = BuildSafeTree();
                if (!string.IsNullOrEmpty(error))
                {
                    return error;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// The entry point for the controller to get the stream representing the tree (later to be replaced with other resource management)
        /// </summary>
        protected void GetTreeStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            try
            {
                RawEbnf = assembly.GetManifestResourceStream(EBNF_FILE_NAME);
            }
            catch (Exception e)
            {
                rawEbnfLoadingError = e.Message;
            }
        }

        /// <summary>
        /// Clean up of the tree, including the pruning of the cyclic references (to avoid infinite loops)
        /// This edit the <see cref="_memoryTree"/> content, by adding and removing optimized nodes
        /// </summary>
        /// <returns>Any potential errors that may have occured during the execution</returns>
        protected string BuildSafeTree()
        {
            if (RawEbnf == null)
            {
                return JsonSerializer.Serialize(new Exception("The EBNF file was not found, unable to build the tree"));
            }
            try
            {
                //build up the normal infinity cycling tree of nodes for the grammar
                if (!RawEbnf.CanRead)
                {
                    GetTreeStream();
                }
                var elements = parsedEbnf.Parse(RawEbnf);
                if (elements == null)
                {
                    return JsonSerializer.Serialize(new Exception("The EBNF file parsing failed, unable to return a valid tree"));
                }

                var optimizedList = new Collection<TreeElement>();
                foreach (var elem in elements)
                {
                    var optimized = elem.Optimize();
                    if (optimized != null)
                    {
                        optimizedList.Add(elem);
                    }
                }

                //Build up the tree with reference (so that the json format of the cyclic tree object is finite)
                //we replace the original with the optimized list
                MemoryTree.Clear();
                var flatRealElements = new Collection<TreeElementReference>();
                foreach (var elem in optimizedList)
                {
                    var treeElem = TreeElementReference.CreateNew(elem, flatRealElements);
                    if (treeElem != null)
                    {
                        MemoryTree.Add(treeElem);
                    }
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