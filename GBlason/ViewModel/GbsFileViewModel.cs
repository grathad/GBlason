using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using FormatManager.Serializer;
using GBSFormatManager;
using GBlason.Common.Converter;
using GBlason.ViewModel.Contract;

namespace GBlason.ViewModel
{
    /// <summary>
    /// Entry point of a Coat of Arm. Top of the tree. Handle the file, and the CoA root object, and the PropertiesVM, and the TreeViewVM
    /// </summary>
    public class GbsFileViewModel : INotifyPropertyChanged
    {
        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GbsFileViewModel"/> class.
        /// </summary>
        /// <remarks>Increase the number of "new document" and affect the default file title to the new document</remarks>
        public GbsFileViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GbsFileViewModel"/> class.
        /// Used when opening a document by using the "Open" command
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public GbsFileViewModel(String fileName)
        {
            FileName = fileName;
        }

        #endregion

        public CoatOfArmViewModel RootCoatOfArm { get; set; }

        #region coat of arms

        public CoatOfArmComponent CurrentlySelectedComponent
        {
            get { return _currentlySelectedComponent; }
            set
            {
                if (value == _currentlySelectedComponent) return;
                _currentlySelectedComponent = value;
                OnPropertyChanged("CurrentlySelectedComponent");
                _currentlySelectedComponent.UpdateBindingOnSelected();
            }
        }
        private CoatOfArmComponent _currentlySelectedComponent;
        #endregion

        #region Tools and "from repo" building functions

        /// <summary>
        /// Centralized method to handle the opening of files (multi, single, new, dropping).
        /// Do a lot of operation on the file opening
        /// </summary>
        /// <param name="fileNames">The file names.</param>
        /// <param name="checkFormat">if set to <c>true</c> [check format].</param>
        /// <exception cref="FileNotFoundException">if the file can't be found</exception>
        /// <exception cref="InvalidOperationException">if an error occur while reading the format</exception>
        public static void OpenFiles(String[] fileNames, bool checkFormat = true)
        {
            var opener = new GbsManager();
            foreach (var fileName in fileNames)
            {
                var loadGbsFile = opener.LoadGbsFile(fileName, checkFormat);
                var vmFile = new GbsFileViewModel(Path.GetFileNameWithoutExtension(fileName))
                {
                    FullFileName = fileName,
                    RootCoatOfArm = loadGbsFile.XmlCoatOfArms.ConvertToViewModel()
                };
                vmFile.CurrentlySelectedComponent = vmFile.RootCoatOfArm;
                //si le fichier est déjà ouvert, on ne fait que lui donner le focus (currently displayed), sinon on l'ajoute bien entendu ^^
                var alreadyOpenedFile =
                    GlobalApplicationViewModel.GetApplicationViewModel.OpenedFiles.FirstOrDefault(
                        vmExistingFile => vmExistingFile.FileName == vmFile.FileName);
                if (alreadyOpenedFile != default(GbsFileViewModel))
                {
                    GlobalApplicationViewModel.GetApplicationViewModel.CurrentlyDisplayedFile = alreadyOpenedFile;
                }
                else
                {
                    GlobalApplicationViewModel.GetApplicationViewModel.OpenedFiles.Add(vmFile);
                    GlobalApplicationViewModel.GetApplicationViewModel.CurrentlyDisplayedFile = vmFile;
                }
                //sauvegarde des fichiers recents - sous traités
                GlobalApplicationViewModel.GetApplicationViewModel.SaveOpenedOrSavedFileAsRecent(fileName);
            }
        }
        #endregion

        #region file properties

        public String FullFileName { get; set; }

        public String FileName
        {
            get { return _fileName; }
            set
            {
                if (value == _fileName) return;
                _fileName = value;
                OnPropertyChanged("FileName");
            }
        }
        private String _fileName;

        #endregion

        #region INotifyPropertyChanged Members

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
