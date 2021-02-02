using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ebnf;
using Grammar;

namespace Grammar_UI
{
    public class ResourcesViewModel : INotifyPropertyChanged
    {
        private Resources _resources;

        private EbnfParserViewModel _ebnfParser;

        public EbnfParserViewModel EbnfParser
        {
            get
            {
                return _ebnfParser;
            }
            set
            {
                _ebnfParser = value;
                NotifiyPropertyChanged(nameof(EbnfParser));
            }
        }


        private Dictionary<string, IEnumerable<IEnumerable<string>>> _finalTokens;

        public Dictionary<string, IEnumerable<IEnumerable<string>>> FinalTokens
        {
            get
            {
                return _finalTokens;
            }
            set
            {
                _finalTokens = value;
                NotifiyPropertyChanged(nameof(FinalTokens));
            }
        }

        //still do not know the format of the custom rule objects, so for now they will be objects
        private IEnumerable<object> _customRules;

        public IEnumerable<object> CustomRules
        {
            get
            {
                return _customRules;
            }
            set
            {
                _customRules = value;
                NotifiyPropertyChanged(nameof(CustomRules));
            }
        }

        public ResourcesViewModel(Resources resources)
        {
            _resources = resources;
        }

        public void LoadRoot(string rootFileName)
        {
            using (var st = File.OpenRead(rootFileName))
            {
                _resources.LoadGrammar(st);
                EbnfParser = new EbnfParserViewModel();
                EbnfParser.CreateFromSource(_resources.RootGrammar);
            }
        }

        public void LoadKeywords(string kwFileName)
        {
            using (var st = File.OpenRead(kwFileName))
            {
                _resources.LoadKeywords(st);
                FinalTokens = _resources.GetTokens();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void NotifiyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
