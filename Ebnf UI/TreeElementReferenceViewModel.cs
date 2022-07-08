using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Ebnf;

namespace Ebnf_UI
{
    /// <summary>
    /// A version of the tree element VM that avoid infinite cyclic references by implementing a link with tree item that are already part of a subtree (for display and debug purposes)
    /// </summary>
    public class TreeElementReferenceViewModel : INotifyPropertyChanged
    {
        public TreeElementReferenceViewModel(TreeElement source, Collection<TreeElementReferenceViewModel> itemsRef = null)
        {
            RealElement = source;
            if (RealElement == null) { return; }
            if (itemsRef == null) { itemsRef = new Collection<TreeElementReferenceViewModel>(); }
            var alreadyIn = itemsRef.Any(t => t.RealElement == source);
            if (!alreadyIn) { itemsRef.Add(this); }
            if (source.Children == null || !source.Children.Any())
            {
                return;
            }
            foreach (var child in RealElement.Children)
            {
                Children.Add(BuildCyclicSafeSubtree(child, itemsRef));
            }
        }

        public TreeElementReferenceViewModel()
        {

        }

        public TreeElementReferenceViewModel FirstOrDefault(Func<TreeElementReferenceViewModel, bool> filter)
        {
            if (filter.Invoke(this)) { return this; }
            //moving along the tree
            if (Children == null || !Children.Any())
            {
                return null;
            }
            foreach(var child in Children)
            {
                var finding = child.FirstOrDefault(filter);
                if(finding != null)
                {
                    return finding;
                }
            }
            return null;
        }

        private TreeElement _realElement;

        public TreeElement RealElement
        {
            get { return _realElement; }
            set
            {
                _realElement = value;
                NotifiyPropertyChanged(nameof(RealElement));
            }
        }

        private TreeElementReferenceViewModel _reference;

        public TreeElementReferenceViewModel Reference
        {
            get { return _reference; }
            set
            {
                _reference = value;
                NotifiyPropertyChanged(nameof(Reference));
            }
        }

        private ObservableCollection<TreeElementReferenceViewModel> _children = new ObservableCollection<TreeElementReferenceViewModel>();

        public ObservableCollection<TreeElementReferenceViewModel> Children
        {
            get
            {
                return _children;
            }
            set
            {
                _children = value;
                NotifiyPropertyChanged(nameof(Children));
            }
        }

        public static TreeElementReferenceViewModel BuildCyclicSafeSubtree(TreeElement root, Collection<TreeElementReferenceViewModel> itemRefs)
        {
            if (root == null) { return null; }

            TreeElementReferenceViewModel refElement;
            //here we only continue to build the tree IF the current element is NOT in the references
            var alreadyR = itemRefs.FirstOrDefault(r => r.RealElement == root);
            if (alreadyR != null)
            {
                //we already have the root in the tree, let's use a reference to it instead (not including the children)
                refElement = new TreeElementReferenceViewModel
                {
                    Reference = alreadyR,
                    RealElement = root
                };
                //we return an element with a reference and NO (direct) children
                return refElement;
            }
            //here this is the first time we cross path with that element
            //adding this element to the list for later potential redetection
            refElement = new TreeElementReferenceViewModel(root, itemRefs);
            return refElement;
        }

        public string DisplayName
        {
            get
            {
                var sourceElement = RealElement ?? Reference?.RealElement ?? null;
                if (sourceElement == null) { return "Error - Empty"; }

                var text = "";

                //if special group
                if (sourceElement.IsGroup)
                {
                    text += "Group ";
                }
                if (sourceElement.IsOptional)
                {
                    text += "[optional] ";
                }
                if (sourceElement.IsRepetition)
                {
                    text += "[repetable] ";
                }
                if (sourceElement.IsAlternation)
                {
                    text += "Chose one of";
                }
                if (!string.IsNullOrEmpty(text))
                {
                    return text;
                }

                return sourceElement.Name;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void NotifiyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
