using System;
using System.ComponentModel;

namespace GBlason.ViewModel
{
    /// <summary>
    /// Store the recent file name, for the presentation recent file purpose only
    /// </summary>
    [Serializable]
    public class RecentFileViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        /// <value>
        /// The file name.
        /// </value>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the path without the file name (access only).
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public String Path { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this file is fixed (presentation layer wise).
        /// </summary>
        /// <value>
        ///   <c>true</c> if this file is fixed; otherwise, <c>false</c>.
        /// </value>
        public bool IsFixed
        {
            get { return _isFixed; }
            set
            {
                if (value == _isFixed) return;
                _isFixed = value;
                OnPropertyChanged("IsFixed");
            }
        }

        private bool _isFixed;
        /// <summary>
        /// Gets or sets the last open UTC.
        /// </summary>
        /// <value>
        /// The last open UTC. Used to allow the sorting of the last files
        /// </value>
        public DateTime LastOpenUtc
        {
            get { return _lastOpenUtc; }
            set
            {
                if (value == _lastOpenUtc) return;
                _lastOpenUtc = value;
                OnPropertyChanged("LastOpenUtc");
            }
        }

        private DateTime _lastOpenUtc;

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
