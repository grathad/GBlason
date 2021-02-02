using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ebnf;

namespace Grammar_UI
{
    public class EbnfParserViewModel : INotifyPropertyChanged
    {
        private Parser _ebnfParser = new Parser();

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

        private ObservableCollection<TreeElementViewModel> _parsedRules = new ObservableCollection<TreeElementViewModel>();

        public ObservableCollection<TreeElementViewModel> ParsedRules
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

        private ObservableCollection<TreeElementViewModel> _filteredRules = new ObservableCollection<TreeElementViewModel>();

        public ObservableCollection<TreeElementViewModel> FilteredRules
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

        private TreeElementViewModel _selectedItem;

        public TreeElementViewModel SelectedItem
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

        public void CreateFromSource(Parser source)
        {
            _ebnfParser = source;

            foreach (var r in _ebnfParser.AllRules)
            {
                ParsedRules.Add(new TreeElementViewModel(r));
                FilteredRules.Add(new TreeElementViewModel(r));
            }
        }

        public void CreateFromKeywords(Dictionary<string, IEnumerable<string>> keyword)
        {
            foreach (var keyWordRule in keyword)
            {
                var te = new TreeElement() { Name = keyWordRule.Key, RulesContent = keyWordRule.Value.Aggregate((a, b) => a + " " + b), IsLeaf = true };
                var tevm = new TreeElementViewModel(te);
                ParsedRules.Add(tevm);
                FilteredRules.Add(tevm);
            }
        }

        public void Filter(string text)
        {
            if (FilteredRules == null
                || ParsedRules == null)
            {
                return;
            }

            IList<TreeElementViewModel> filteredList;

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

        public event PropertyChangedEventHandler PropertyChanged;

        void NotifiyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
