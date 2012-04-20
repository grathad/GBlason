using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using GBlason.Culture;

namespace GBlason.ViewModel.Contract
{
    /// <summary>
    /// Contract for all the different layers and component of the tree logic coat of arms composition
    /// Help defining all the data for the properties display
    /// </summary>
    public abstract class CoatOfArmComponent: INotifyPropertyChanged
    {
        public String ComponentName
        {
            get
            {
                var rman = new ResourceManager(typeof(BlasonVocabulary));
                return rman.GetString(GetType().Name) ?? String.Empty;
            }
        }

        public void UpdateBindingOnSelected()
        {
            OnPropertyChanged("ComponentName");
        }

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
