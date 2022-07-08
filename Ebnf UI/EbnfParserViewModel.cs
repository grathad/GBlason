using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ebnf;

namespace Ebnf_UI
{
    public class EbnfParserViewModel : INotifyPropertyChanged
    {
        private readonly Parser _ebnfParser = new();

        #region VM Properties

        private bool _parsingNotInProgress = true;

        public bool ParsingNotInProgress
        {
            get
            {
                return _parsingNotInProgress;
            }
            set
            {
                _parsingNotInProgress = value;
                NotifiyPropertyChanged(nameof(ParsingNotInProgress));
            }
        }

        private ObservableCollection<TreeElementReferenceViewModel> _parsedRules = new ObservableCollection<TreeElementReferenceViewModel>();

        /// <summary>
        /// The total flatten list of all the rules parsed, not in a tree
        /// </summary>
        public ObservableCollection<TreeElementReferenceViewModel> ParsedRules
        {
            get
            {
                return _parsedRules;
            }
            set
            {
                _parsedRules = value;
                NotifiyPropertyChanged(nameof(ParsedRules));
            }
        }

        private ObservableCollection<TreeElementReferenceViewModel> _filteredRules = new ObservableCollection<TreeElementReferenceViewModel>();

        /// <summary>
        /// The sublist of rules available flattened as used by the list view
        /// </summary>
        public ObservableCollection<TreeElementReferenceViewModel> FilteredRules
        {
            get
            {
                return _filteredRules;
            }
            set
            {
                _filteredRules = value;
                NotifiyPropertyChanged(nameof(FilteredRules));
            }
        }

        private TreeElementReferenceViewModel _selectedItem;

        /// <summary>
        /// The currently selected item in either the list or the filtered list
        /// </summary>
        public TreeElementReferenceViewModel SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                NotifiyPropertyChanged(nameof(SelectedItem));
            }
        }

        #endregion

        #region Operations

        /// <summary>
        /// Filter the flat list to only show the corresponding elements
        /// </summary>
        /// <param name="text"></param>
        public void Filter(string text)
        {
            if (FilteredRules == null
                || ParsedRules == null)
            {
                return;
            }

            IList<TreeElementReferenceViewModel> filteredList;

            if (string.IsNullOrEmpty(text))
            {
                filteredList = ParsedRules.ToList();
            }
            else
            {
                filteredList = ParsedRules.Where(r => r.RealElement.Name.Contains(text, StringComparison.OrdinalIgnoreCase) || text.Contains(r.RealElement.Name)).ToList();
            }

            FilteredRules.Clear();
            foreach (var rule in filteredList)
            {
                FilteredRules.Add(rule);
            }
        }

        /// <summary>
        /// Creating all the rules from the input file and creating the corresponding viewmodels out of the parsing for debugging purpose
        /// </summary>
        /// <param name="input"></param>
        public void Parse(FileStream input)
        {
            try
            {
                ParsingNotInProgress = false;
                _ebnfParser.Parse(input);

                foreach (var rootRule in _ebnfParser.AllRules.Where(r => !r.Parents.Any()))
                {
                    var itemsRef = new Collection<TreeElementReferenceViewModel>();
                    FilteredRules.Add(new TreeElementReferenceViewModel(rootRule, itemsRef));
                    foreach (var r in itemsRef)
                    {
                        if (ParsedRules.All(rul => rul != r))
                        {
                            ParsedRules.Add(r);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //we could display the exception later
                var breakPoint = e;
            }
            finally
            {
                ParsingNotInProgress = true;
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        void NotifiyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
