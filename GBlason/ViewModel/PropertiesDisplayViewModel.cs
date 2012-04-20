using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using GBlason.Culture;

namespace GBlason.ViewModel
{
    /// <summary>
    /// Contain a lot of getter, to display all the data we need for the "properties" area, the way it is intended to be bound
    /// </summary>
    public class PropertiesDisplayViewModel : INotifyPropertyChanged
    {
        public PropertiesDisplayViewModel()
        {
        }

        public String ComponentName
        {
            get
            {
                var tampon = GlobalApplicationViewModel.GetApplicationViewModel.CurrentlyDisplayedFile.CurrentlySelectedComponent;
                if(tampon == null)
                    return String.Empty;
                var type = tampon.GetType();
                var rman = new ResourceManager(typeof (BlasonVocabulary));
                return rman.GetString(type.Name);
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
}
