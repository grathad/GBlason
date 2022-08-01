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

        public TreeElementReference? Parent { get; set; } = null;

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

        /// <summary>
        /// Create a new <see cref="TreeElementReference"/> instance, based of the provided <paramref name="source"/>.
        /// Will either create a real instance of reference (once) or return the previously created one if already created.
        /// Only the first creation will attach the <see cref="Children"/> and the <see cref="Parent"/>
        /// as to avoid infinite cycle in the tree and to only keep one single link to the parent branch to search the path to the root
        /// </summary>
        /// <param name="source">The source tree element from which to base this reference object</param>
        /// <param name="itemsRef">The list of reference objects already created for this tree</param>
        /// <returns>A new <see cref="TreeElementReference"/> or null if the source is null</returns>
        public static TreeElementReference? CreateNew(TreeElement source, Collection<TreeElementReference>? itemsRef = null)
        {
            if (source == null) { return null; }
            if (itemsRef == null) { itemsRef = new Collection<TreeElementReference>(); }
            var alreadyIn = itemsRef.FirstOrDefault(r => string.Compare(r.RealElement.Name, source.Name) == 0);

            if (alreadyIn != null)
            {
                return alreadyIn;
            }

            var thisReference = new TreeElementReference();
            itemsRef.Add(thisReference);

            thisReference.RealElement = source;

            if (source.Children == null || !source.Children.Any())
            {
                return thisReference;
            }
            foreach (var child in source.Children)
            {
                var childRef = BuildCyclicSafeSubtree(child, itemsRef);
                if (childRef != null)
                {
                    thisReference.Children.Add(childRef);
                    //if the current children being returned is not a reference, then we attach ourselves as the parent
                    if (childRef.ReferenceToElement == null)
                    {
                        childRef.Parent = thisReference;
                    }
                    thisReference.HasChildren = true;
                }
            }
            return thisReference;
        }

        /// <summary>
        /// Create a subset of the tree as a value-copy of instances
        /// </summary>
        /// <param name="toCopy">The original reference head of the subtree to copy</param>
        /// <param name="depth">The number of children layers to copy (default to 1, which is the first generation of children from <paramref name="toCopy"/>)</param>
        /// <returns>a copy of the elements within the subtree starting at toCopy, including depth generations</returns>
        public static TreeElementReference CreateCopy(TreeElementReference toCopy, int depth = 1)
        {
            var toReturn = new TreeElementReference();
            toReturn.ElementId = toCopy.ElementId;
            toReturn.RealElement = toCopy.RealElement;
            toReturn.Reference = toCopy.Reference;
            toReturn.ReferenceToElement = toCopy.ReferenceToElement;
            toReturn.HasChildren = toCopy.HasChildren;
            if (depth > 0)
            {
                foreach (var child in toCopy.Children)
                {
                    toReturn.Children.Add(new TreeElementReference(child, depth - 1));
                }
            }
            return toReturn;
        }

        protected TreeElementReference()
        {

        }

        /// <summary>
        /// Copy constructor that only return the <paramref name="depth"/> level of children.
        /// The parameter to copy MUST have been constructed in a tree before the call as no tree creation are attempted in this copy ctor
        /// </summary>
        /// <param name="toCopy">The element to copy</param>
        /// <param name="depth">The number of generation of children to attach to the copy</param>
        protected TreeElementReference(TreeElementReference toCopy, int depth = 1)
        {
            ElementId = toCopy.ElementId;
            RealElement = toCopy.RealElement;
            Reference = toCopy.Reference;
            HasChildren = toCopy.HasChildren;
            ReferenceToElement = toCopy.ReferenceToElement;
            if (depth > 0)
            {
                foreach (var child in toCopy.Children)
                {
                    Children.Add(new TreeElementReference(child, depth - 1));
                }
            }
        }

        /// <summary>
        /// Build a cyclicly safe tree. Recursively going through the whole tree, and only building one branch per node, every copy of the same node that have already be given a branch
        /// is treated as a "reference" i.e. a link to the real node that was given children, and no children or parent of its own.
        /// </summary>
        /// <param name="root">The root of the tree from where to build the safe tree</param>
        /// <param name="itemRefs">The list of all the real nodes that have been created so far</param>
        /// <returns>The new root of the safe tree</returns>
        public static TreeElementReference? BuildCyclicSafeSubtree(TreeElement root, Collection<TreeElementReference> itemRefs)
        {
            if (root == null) { return null; }

            TreeElementReference? refElement;
            //here we only continue to build the tree IF the current element is NOT in the references
            var alreadyR = itemRefs.FirstOrDefault(r => string.Compare(r.RealElement.Name, root.Name) == 0);
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
                //we return an element with a reference NO parent and NO children
                return refElement;
            }
            //here this is the first time we cross path with that element
            //adding this element to the list for later potential redetection
            refElement = CreateNew(root, itemRefs);
            return refElement;
        }

        /// <summary>
        /// Go through the tree parent by parent until the root or a cyclic reference is found
        /// </summary>
        /// <param name="leaf">The start of the search</param>
        /// <param name="cycle">The potential cyclic reference</param>
        /// <param name="root">The root</param>
        /// <returns>the node found, either the root or the cyclic reference</returns>
        public void FindParent(ref Collection<TreeElementReference> cycle, TreeElementReference root)
        {
            //since we are using references, and not the real treenode, we should only have one parent
            if (this == null || root == null)
            {
                return;
            }

            if (cycle.Any(c => c.ElementId == ElementId))
            {
                //we are in a cyclic branch, returning
                return;
            }
            cycle.Insert(0, this);
            if (this == root)
            {
                //we reached the root, we are done
                return;
            }

            if (Parent == null)
            {
                //no way to continue
                return;
            }

            //we continue the search from the parent subtree
            Parent.FindParent(ref cycle, root);
        }

        public override string ToString()
        {
            return this?.RealElement?.Name ?? "Null";
        }
    }
}
