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
        #region fileNames
        private string _rootGrammarFilePath;

        public string RootGrammarFilePath
        {
            get
            {
                return _rootGrammarFilePath;
            }
            set
            {
                _rootGrammarFilePath = value;

                NotifiyPropertyChanged(nameof(RootGrammarFilePath));
            }
        }

        private string _keywordsFilePath;

        public string KeywordsFilePath
        {
            get
            {
                return _keywordsFilePath;
            }
            set
            {
                _keywordsFilePath = value;

                NotifiyPropertyChanged(nameof(KeywordsFilePath));
            }
        }
        private string _customGrammarFilePath;

        public string CustomGrammarFilePath
        {
            get
            {
                return _customGrammarFilePath;
            }
            set
            {
                _customGrammarFilePath = value;

                NotifiyPropertyChanged(nameof(CustomGrammarFilePath));
            }
        }
        #endregion

        private ResourcesViewModel _resources = new ResourcesViewModel(new Resources());

        public ResourcesViewModel Resources
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

        public void LoadFromFile(string fileName)
        {
            try
            {
                //var assembly = Assembly.LoadFrom(fileName);
                //Resources = new Resources(assembly);
                //AssemblyName = Resources.Assembly.GetName()?.Name ?? "Error while attempting to get the assembly name";
                //KeywordResourceName = Resources.KeywordResourceName;
                //GrammarResourceName = Resources.GrammarResourceName;

                //EbnfParser.CreateFromSource(Resources.GetGrammar());
                //EbnfParser.CreateFromKeywords(Resources.GetKeywords());
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
