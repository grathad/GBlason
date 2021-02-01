using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ebnf;

namespace Ebnf_UI
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string _filePath;

        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
                NotifiyPropertyChanged(nameof(FilePath));
            }
        }

        private EbnfParserViewModel _ebnfParser = new EbnfParserViewModel();

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

        public event PropertyChangedEventHandler PropertyChanged;

        public FileStream FileStream { get; set; }

        void NotifiyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
