using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using GBlason.Culture;
using GBlason.ViewModel.Contract;

namespace GBlason.ViewModel
{
    public class ShapeViewModel : INotifyPropertyChanged
    {
        public String Name { get; set; }

        public String LocalizedName
        {
            get
            {
                var manager = new ResourceManager(typeof(BlasonVocabulary));
                return manager.GetString(Name);
            }
        }

        public String Geometry { get; set; }

        public String Description { get; set; }

        public Guid Identifier { get; set; }

        public uint PreferedWidth { get; set; }

        public uint PreferedHeight { get; set; }

        public double ScaleX { get; set; }

        public double ScaleY { get; set; }

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
