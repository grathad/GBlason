using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GBlason.ViewModel.Contract
{
    public class PropertyDisplayer : INotifyPropertyChanged
    {
        public String PropertyName { get; set; }

        public Object PropertyValue
        {
            get { return _propertyValue; }
            set { if (value == _propertyValue) return;
                _propertyValue = value;
                OnPropertyChanged("PropertyValue");
            }
        }

        private Object _propertyValue;

        #region INotifyPropertyChanged Members

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
