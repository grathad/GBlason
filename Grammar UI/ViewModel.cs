using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Grammar;

namespace Grammar_UI
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

        private Resources _resources;

        public Resources Resources
        {
            get
            {
                return _resources;
            }
            set
            {
                _resources = value;

                NotifiyPropertyChanged(nameof(Resources));
            }
        }

        private string _assemblyName;

        public string AssemblyName
        {
            get
            {
                return _assemblyName;
            }
            set
            {
                _assemblyName = value;

                NotifiyPropertyChanged(nameof(AssemblyName));
            }
        }

        private string _keywordResourceName;

        public string KeywordResourceName
        {
            get
            {
                return _keywordResourceName;
            }
            set
            {
                _keywordResourceName = value;

                NotifiyPropertyChanged(nameof(KeywordResourceName));
            }
        }

        private string _grammarResourceName;

        public string GrammarResourceName
        {
            get
            {
                return _grammarResourceName;
            }
            set
            {
                _grammarResourceName = value;

                NotifiyPropertyChanged(nameof(GrammarResourceName));
            }
        }

        public void LoadFromFile(string fileName)
        {
            try
            {
                var assembly = Assembly.LoadFrom(fileName);
                Resources = new Resources(assembly);
                AssemblyName = Resources.Assembly.GetName()?.Name ?? "Error while attempting to get the assembly name";
                KeywordResourceName = Resources.KeywordResourceName;
                GrammarResourceName = Resources.GrammarResourceName;

            }
            catch (Exception e)
            {

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void NotifiyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
