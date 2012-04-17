using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using FormatManager.Serializer;
using GBSFormatManager;
using GBlason.Global;
using GBlason.Properties;

namespace GBlason.ViewModel
{
    public class GbsFileViewModel : INotifyPropertyChanged
    {
        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GbsFileViewModel"/> class.
        /// Used when creating a document by using the "New" command
        /// </summary>
        /// <remarks>Increase the number of "new document" and affect the default file title to the new document</remarks>
        public GbsFileViewModel()
        {
            FileName = String.Format(CultureInfo.CurrentCulture,
                                     Properties.Resources.NewDocumentLabel,
                                     ++GlobalApplicationViewModel.NumberOfNewDocument);
            if (GbrFileViewModel.GetResources.ScaledForMenuShapeResources.Any())
                _currentShape = GbrFileViewModel.GetResources.ScaledForMenuShapeResources[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GbsFileViewModel"/> class.
        /// Used when opening a document by using the "Open" command
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="format">The format.</param>
        public GbsFileViewModel(String fileName, GbsFormat format)
        {
            FileName = fileName;

        }

        #endregion

        #region Tools and "from repo" building functions

        /// <summary>
        /// Opens the files.
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
                var vmFile = new GbsFileViewModel(Path.GetFileNameWithoutExtension(fileName), loadGbsFile){FullFileName = fileName};
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

        #region COA properties

        public ShapeViewModel CurrentShape
        {
            get { return _currentShape; }
            set
            {
                if (_currentShape == value)
                    return;
                    _currentShape = value;
                OnPropertyChanged("currentShape");
            }
        }
        private ShapeViewModel _currentShape;

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
