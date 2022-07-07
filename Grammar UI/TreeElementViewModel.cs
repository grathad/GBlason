using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Ebnf;
using Utils.LinqHelper;

namespace Grammar_UI
{

    public class TreeElementViewModel : INotifyPropertyChanged
    {

        public TreeElementViewModel(TreeElement element)
        {
            RealElement = element;
        }

        public void BuildLinks(IEnumerable<TreeElementViewModel> allElements)
        {
            foreach (var realParent in RealElement.Parents)
            {
                var vmParent = allElements.FirstOrDefault(vme => string.Equals(vme.Name, realParent.Name));
                if (vmParent != null)
                {
                    Parents.Add(vmParent);
                }
            }
            //we think before adding the child references
            //UI purpose grammar operators (not clean but will do for now)
            if (RealElement.IsOptional)
            {
                Children.Add(new TreeElementViewModel(null) { Name = Parser.OptionalStartSign.ToString() });
            }
            if (RealElement.IsRepetition)
            {
                Children.Add(new TreeElementViewModel(null) { Name = Parser.RepeatStartSign.ToString() });
            }
            if (RealElement.IsGroup)
            {
                Children.Add(new TreeElementViewModel(null) { Name = Parser.GroupStartSign.ToString() });
            }

            //if the child has ALREADY been added, we only add a reference to it
            for (var i = 0; i < RealElement.Children.Count; i++)
            {
                var realChild = RealElement.Children[i];
                var vmChild = allElements.FirstOrDefault(vme => string.Equals(vme.Name, realChild.Name));
                if (vmChild != null)
                {
                    Children.Add(vmChild);
                }
                else
                {
                    //we have an error, the child should be in the list of all the elements
                    throw new Exception($"Unexpected situation the child {realChild?.Name} is not part of the list of all elements");
                }
                if (i < RealElement.Children.Count - 1)
                {
                    if (RealElement.IsAlternation)
                    {
                        Children.Add(new TreeElementViewModel(null) { Name = Parser.AlternSign.ToString() });
                    }
                    else
                    {
                        Children.Add(new TreeElementViewModel(null) { Name = Parser.SequenceSign.ToString() });
                    }
                }
            }

            if (RealElement.IsGroup)
            {
                Children.Add(new TreeElementViewModel(null) { Name = Parser.GroupEndSign.ToString() });
            }
            if (RealElement.IsRepetition)
            {
                Children.Add(new TreeElementViewModel(null) { Name = Parser.RepeatEndSign.ToString() });
            }
            if (RealElement.IsOptional)
            {
                Children.Add(new TreeElementViewModel(null) { Name = Parser.OptionalEndSign.ToString() });
            }
        }

        public TreeElement RealElement { get; init; }

        private string _name = null;

        public string Name
        {
            get
            {
                return _name ?? RealElement?.Name;
            }
            set
            {
                _name = value;
                NotifiyPropertyChanged(nameof(Name));
            }
        }

        private ObservableCollection<TreeElementViewModel> _children = new ObservableCollection<TreeElementViewModel>();

        public ObservableCollection<TreeElementViewModel> Children
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

        private ObservableCollection<TreeElementViewModel> _parents = new ObservableCollection<TreeElementViewModel>();

        public override string ToString()
        {
            return Name;
        }

        public ObservableCollection<TreeElementViewModel> Parents
        {
            get
            {
                return _parents;
            }
            set
            {
                _parents = value;
                NotifiyPropertyChanged(nameof(Parents));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void NotifiyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
