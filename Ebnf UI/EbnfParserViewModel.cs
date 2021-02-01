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

        public void Parse(FileStream input)
        {
            try
            {
                ParsingNotInProgress = false;
                _ebnfParser.Parse(input);
                foreach (var rule in _ebnfParser.AllRules)
                {
                    ParsedRules.Add(new TreeElementViewModel(rule));
                }
                //creating tree for all the children for all the rules                
                foreach (var rule in ParsedRules)
                {
                    rule.BuildLinks(ParsedRules);
                    FilteredRules.Add(rule);
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

        public event PropertyChangedEventHandler PropertyChanged;

        void NotifiyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
