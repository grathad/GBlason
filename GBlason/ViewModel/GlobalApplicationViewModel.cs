using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
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
            RecentFiles = new ObservableCollection<RecentFileViewModel>();
            RecentDirectories = new ObservableCollection<RecentFileViewModel>();
            RecentFiles.CollectionChanged += RecentFilesCollectionChanged;
            RecentDirectories.CollectionChanged += RecentFilesCollectionChanged;
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

        #region Windows Preference Settings

        const double Epsilon = 0.1;

        public Int32 MainWindowWidth
        {
            get { return _mainWindowWidth; }
            set
            {
                if (_mainWindowWidth == value)
                    return;
                _mainWindowWidth = value;
                OnPropertyChanged("MainWindowWidth");
            }
        }

        private Int32 _mainWindowWidth;

        public Int32 MainWindowHeight
        {
            get { return _mainWindowHeight; }
            set
            {
                if (_mainWindowHeight == value)
                    return;
                _mainWindowHeight = value;
                OnPropertyChanged("MainWindowHeight");
            }
        }

        private Int32 _mainWindowHeight;

        private System.Drawing.Point _mainWindowPosition;

        public Int32 MainWindowTop
        {
            get { return _mainWindowPosition.Y; }
            set
            {
                if (value == _mainWindowPosition.Y)
                    return;
                _mainWindowPosition.Y = value;
                OnPropertyChanged("MainWindowTop");
            }
        }

        public Int32 MainWindowLeft
        {
            get { return _mainWindowPosition.X; }
            set
            {
                if (value == _mainWindowPosition.X)
                    return;
                _mainWindowPosition.X = value;
                OnPropertyChanged("MainWindowLeft");
            }
        }

        public WindowState MainWindowState
        {
            get { return _mainWindowState; }
            set
            {
                if (_mainWindowState == value)
                    return;
                _mainWindowState = value;
                OnPropertyChanged("MainWindowState");
            }
        }

        private WindowState _mainWindowState;
        #endregion

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

        public ObservableCollection<RecentFileViewModel> RecentDirectories
        {
            get { return _recentDirectories; }
            set { _recentDirectories = value; }
        }

        private ObservableCollection<RecentFileViewModel> _recentDirectories;

        public void AddRecentDirectoriesCollection(IList<RecentFileViewModel> collection)
        {
            if (collection == null) return;
            foreach (var val in collection)
                RecentDirectories.Add(val);
        }

        public Visibility FlaggedDirectoriesVisibility
        {
            get
            {
                return RecentDirectoriesFlaggedOrderedByDate.Any() ? Visibility.Visible : Visibility.Collapsed;
            }
        }


        public ObservableCollection<RecentFileViewModel> RecentDirectoriesOrderedByDate
        {
            get { return new ObservableCollection<RecentFileViewModel>(_recentDirectories.Where(rf => !rf.IsFixed).OrderByDescending(recentFile => recentFile.LastOpenUtc)); }
        }

        public ObservableCollection<RecentFileViewModel> RecentDirectoriesFlaggedOrderedByDate
        {
            get { return new ObservableCollection<RecentFileViewModel>(_recentDirectories.Where(rf => rf.IsFixed).OrderByDescending(recentFile => recentFile.LastOpenUtc)); }
        }

        /// <summary>
        /// Gets or sets the recent files ordered by utc date of recentness.
        /// </summary>
        /// <value>
        /// The recent files.
        /// </value>
        public ObservableCollection<RecentFileViewModel> RecentFiles
        {
            get { return _recentFiles; }
            set { _recentFiles = value; }
        }

        private ObservableCollection<RecentFileViewModel> _recentFiles;

        public void AddRecentFilesCollection(IList<RecentFileViewModel> collection)
        {
            if(collection == null) return;
            foreach(var val in collection)
                RecentFiles.Add(val);
        }

        void RecentFilesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null) return;
            foreach (var tampon in e.NewItems.OfType<RecentFileViewModel>())
            {
                tampon.PropertyChanged += GlobalApplicationViewModelPropertyChanged;
            }
        }

        void GlobalApplicationViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "LastOpenUtc" && e.PropertyName != "IsFixed") return;
            UpdatePropertyForRecentFileDisplay();
        }

        private void UpdatePropertyForRecentFileDisplay()
        {
            OnPropertyChanged("RecentFilesOrderedByDate");
            OnPropertyChanged("RecentFilesFlaggedOrderedByDate");
            OnPropertyChanged("FlaggedFilesVisibility");
            OnPropertyChanged("RecentDirectoriesOrderedByDate");
            OnPropertyChanged("RecentDirectoriesFlaggedOrderedByDate");
            OnPropertyChanged("FlaggedDirectoriesVisibility");
        }

        public Visibility FlaggedFilesVisibility
        {
            get
            {
                return RecentFilesFlaggedOrderedByDate.Any() ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public ObservableCollection<RecentFileViewModel> RecentFilesOrderedByDate
        {
            get { return new ObservableCollection<RecentFileViewModel>(_recentFiles.Where(rf => !rf.IsFixed).OrderByDescending(recentFile => recentFile.LastOpenUtc)); }
        }

        public ObservableCollection<RecentFileViewModel> RecentFilesFlaggedOrderedByDate
        {
            get { return new ObservableCollection<RecentFileViewModel>(_recentFiles.Where(rf => rf.IsFixed).OrderByDescending(recentFile => recentFile.LastOpenUtc)); }
        }


        /// <summary>
        /// Saves the opened or saved file as recent in the application settings.
        /// </summary>
        /// <param name="fullPath">The full path of the file.</param>
        public void SaveOpenedOrSavedFileAsRecent(String fullPath)
        {
            var recentFile = new RecentFileViewModel
                                 {
                                     IsFixed = false,
                                     Name = Path.GetFileNameWithoutExtension(fullPath),
                                     Path = fullPath,
                                     LastOpenUtc = DateTime.UtcNow
                                 };
            var dirInfo = Directory.GetParent(fullPath);
            var recentDir = new RecentFileViewModel
            {
                IsFixed = false,
                Name = dirInfo.Name,
                Path = dirInfo.FullName,
                LastOpenUtc = DateTime.UtcNow
            };
            var existingRecentFile = RecentFiles.FirstOrDefault(recentF => recentF.Path == fullPath);
            var existingRecentDirectory = RecentDirectories.FirstOrDefault(recentD => recentD.Path == dirInfo.FullName);
            if (existingRecentFile != null)
            {
                //fichier existant, on le passe au début de la liste
                existingRecentFile.LastOpenUtc = DateTime.UtcNow;
            }
            else
            {
                //on l'ajoute a la liste
                RecentFiles.Add(recentFile);
            }
            if (existingRecentDirectory != null)
            {
                //fichier existant, on le passe au début de la liste
                existingRecentDirectory.LastOpenUtc = DateTime.UtcNow;
            }
            else
            {
                //on l'ajoute a la liste
                RecentDirectories.Add(recentDir);
            }
            UpdatePropertyForRecentFileDisplay();
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
