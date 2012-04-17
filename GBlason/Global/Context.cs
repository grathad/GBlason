using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using FormatManager.Serializer;
using GBlason.Properties;
using GBlason.ViewModel;

namespace GBlason.Global
{
    public class Context : INotifyPropertyChanged
    {
        private static Context _context;
        public static Context GetContext
        {
            get { return _context ?? (_context = new Context()); }
        }

        private Context()
        {
            
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
