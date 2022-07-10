using Ebnf;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace GBlasonWebAPI.Models
{
    /// <summary>
    /// Wrapper around a tree element that enable the reference through link to another element and ensure only one full instance of each unique id is present in the tree (finite tree)
    /// </summary>
    public class TreeElementReference
    {
        /// <summary>
        /// Real numeric identity of the current node
        /// </summary>
        public Guid ElementId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// If this node is a real element (excluding the children) when the property is assigned, should not be serialized
        /// </summary>
        public TreeElementBase RealElement { get; set; } = new();

        /// <summary>
        /// The Children of the reference
        /// </summary>
        public Collection<TreeElementReference> Children { get; set; } = new Collection<TreeElementReference>();

        public bool HasChildren { get; set; } = false;

        /// <summary>
        /// If this node is a reference to a real element (element already in the tree) we only keep a link to the GUID of the real element (and no child)
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Guid? ReferenceToElement { get; set; } = null;

        /// <summary>
        /// This property is only used when building the tree, for simplicity sake.
        /// Not needed at all in the final object (in the contrary, this include cyclic references of the tree)
        /// Removing it from the serialization (alternatively could be set to null before serialization, but this would be slower)
        /// </summary>
        [JsonIgnore]
        public TreeElementReference? Reference { get; set; }

        public TreeElementReference()
        {

        }

        /// <summary>
        /// Copy constructor that only return the <paramref name="depth"/> level of children.
        /// The parameter to copy MUST have been constructed in a tree before the call as no tree creation are attempted in this copy ctor
        /// </summary>
        /// <param name="toCopy">The element to copy</param>
        /// <param name="depth">The number of generation of children to attach to the copy</param>
        public TreeElementReference(TreeElementReference toCopy, int depth = 1)
        {
            ElementId = toCopy.ElementId;
            RealElement = toCopy.RealElement;
            Reference = toCopy.Reference;
            HasChildren = toCopy.HasChildren;
            if(depth > 0)
            {
                foreach(var child in toCopy.Children)
                {
                    Children.Add(new TreeElementReference(child, depth - 1));
                }
            }
        }

        /// <summary>
        /// Constructor of the wrapper that is used to factorially generate the children wrapper as well
        /// </summary>
        /// <param name="source">the original real parent in the infinite tree</param>
        /// <param name="itemsRef">the list of all the reference created so far (flatten)</param>
        public TreeElementReference(TreeElement source, Collection<TreeElementReference> itemsRef)
        {
            RealElement = source;
            if (RealElement == null) { return; }
            if (itemsRef == null) { itemsRef = new Collection<TreeElementReference>(); }
            var alreadyIn = itemsRef.Any(t => t.RealElement == source);
            if (!alreadyIn) { itemsRef.Add(this); }
            if (source.Children == null || !source.Children.Any())
            {
                return;
            }
            foreach (var child in source.Children)
            {
                var childRef = BuildCyclicSafeSubtree(child, itemsRef);
                if (childRef != null)
                {
                    Children.Add(childRef);
                    HasChildren = true;
                }
            }
        }

        public static TreeElementReference? BuildCyclicSafeSubtree(TreeElement root, Collection<TreeElementReference> itemRefs)
        {
            if (root == null) { return null; }

            TreeElementReference refElement;
            //here we only continue to build the tree IF the current element is NOT in the references
            var alreadyR = itemRefs.FirstOrDefault(r => r.RealElement == root);
            if (alreadyR != null)
            {
                //we already have the root in the tree, let's use a reference to it instead (not including the children)
                refElement = new TreeElementReference
                {
                    Reference = alreadyR,
                    ReferenceToElement = alreadyR.ElementId,
                    RealElement = root,
                    HasChildren = alreadyR.HasChildren
                };
                //we return an element with a reference and NO (direct) children
                return refElement;
            }
            //here this is the first time we cross path with that element
            //adding this element to the list for later potential redetection
            refElement = new TreeElementReference(root, itemRefs);
            return refElement;
        }
    }
}
