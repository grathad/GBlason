using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using GBlason.Global;
using GBlason.Properties;

namespace GBlason.ViewModel
{
    /// <summary>
    /// Static class to handle all the application related displays systems
    /// </summary>
    public class GlobalApplicationViewModel : INotifyPropertyChanged
    {
        #region Singleton

        private static GlobalApplicationViewModel _singleton;

        public static GlobalApplicationViewModel GetApplicationViewModel
        {
            get { return _singleton ?? (_singleton = new GlobalApplicationViewModel()); }
        }

        private GlobalApplicationViewModel()
        {
            OpenedFiles = new ObservableCollection<GbsFileViewModel>();
            //loading of the recent files (exceptions to check) TODO @Grathad exception and management of error messages
            RecentFiles = new ObservableCollection<RecentFile>();
            
        }

        #endregion

        #region Document and files

        /// <summary>
        /// Gets or sets the number of new document.
        /// </summary>
        /// <value>
        /// The number of new document (used to count the number to display when creating a new document)
        /// </value>
        public static ushort NumberOfNewDocument { get; set; }

        /// <summary>
        /// Gets or sets the opened files.
        /// </summary>
        /// <value>
        /// The list of opened files, used to display each of them in the tab area of the custom GUI
        /// </value>
        public ObservableCollection<GbsFileViewModel> OpenedFiles { get; set; }

        /// <summary>
        /// Gets or sets the file being currently displayed
        /// </summary>
        /// <value>
        /// The currently displayed file.
        /// </value>
        public GbsFileViewModel CurrentlyDisplayedFile
        {
            get { return _currentlyDisplayedFile; }
            set
            {
                if (value == _currentlyDisplayedFile) return;
                _currentlyDisplayedFile = value;
                OnPropertyChanged("CurrentlyDisplayedFile");
            }
        }

        private GbsFileViewModel _currentlyDisplayedFile;

        /// <summary>
        /// Gets or sets the recent files ordered by utc date of recentness.
        /// </summary>
        /// <value>
        /// The recent files.
        /// </value>
        public ObservableCollection<RecentFile> RecentFiles
        {
            get { return _recentFiles; }
            set { _recentFiles = value; }
        }

        public ObservableCollection<RecentFile> RecentFilesOrderedByDate
        {
            get { return new ObservableCollection<RecentFile>(_recentFiles.OrderByDescending(recentFile => recentFile.LastOpenUtc)); }
        }

        private ObservableCollection<RecentFile> _recentFiles;

        /// <summary>
        /// Saves the opened or saved file as recent in the application settings.
        /// </summary>
        /// <param name="fullPath">The full path of the file.</param>
        public void SaveOpenedOrSavedFileAsRecent(String fullPath)
        {
            var recentSave = new RecentFile { IsFixed = false, Name = Path.GetFileNameWithoutExtension(fullPath), Path = fullPath, LastOpenUtc = DateTime.UtcNow };
            var existingRecentFile = RecentFiles.FirstOrDefault(recentFile => recentFile.Path == fullPath);
            if (existingRecentFile != null)
            {
                //fichier existant, on le passe au début de la liste
                existingRecentFile.LastOpenUtc = DateTime.UtcNow;
            }
            else
            {
                //on l'ajoute a la liste
                RecentFiles.Add(recentSave);
            }
            OnPropertyChanged("RecentFilesOrderedByDate");
        }

        #endregion

        #region Application
        /// <summary>
        /// Property used to get the application title. (title + opened file)
        /// </summary>
        public String ApplicationTitle
        {
            get
            {
                return String.Format(CultureInfo.CurrentCulture,
                    "{0} {1} {2}",
                    Resources.ApplicationTitle,
                    Resources.ApplicationSeparatorChar,
                    CurrentlyDisplayedFile != null
                    ? CurrentlyDisplayedFile.FileName
                    : String.Empty);
            }
        }
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
