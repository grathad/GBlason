using System;
using System.ComponentModel;

namespace GBlason.Common
{
    public class CustomException : Exception, INotifyPropertyChanged
    {
        public CustomException(String message, Exception inner = null)
            : base(message, inner)
        {

        }

        private ExceptionImportance _importance;
        public ExceptionImportance Importance
        {
            get { return _importance; }
            set
            {
                if (_importance == value)
                    return;
                _importance = value;
                OnPropertyChanged("Importance");
            }
        }

        private String _name;
        public String Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        #region INotifyPropertyChanged Members

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

    }

    public enum ExceptionImportance
    {
        //the worst kind of exception. The application is crashed (usually, not handled)
        //the only case that can be foreseen would be a format error requiring the reload of the application after format update
        RestartApplication,
        //The last action ended on an expcetion, it may have to be retried after few other actions
        RestartAction,
        //the exception is handled, and no action is required from the user.
        NoImpact
    }
}