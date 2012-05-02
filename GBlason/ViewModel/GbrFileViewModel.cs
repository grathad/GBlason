using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using FormatManager.Serializer;
using GBlason.Common.Converter;
using GBlason.Properties;

namespace GBlason.ViewModel
{
    public class GbrFileViewModel : INotifyPropertyChanged
    {
        #region singleton

        private static GbrFileViewModel _singleton;

        public static GbrFileViewModel GetResources
        {
            get
            {
                return _singleton ?? (_singleton = new GbrFileViewModel());
            }
        }

        private GbrFileViewModel()
        {
            ScaledForMenuShapeResources = new ObservableCollection<ShapeViewModel>();
        }

        #endregion

        #region initialisations

        public void InitFromFile()
        {
            var manager = new GbrManager();
            var path = Path.Combine(Directory.GetCurrentDirectory(), Settings.Default.GbrLocalFile);
            var formatGbr = manager.LoadGbrFile(path);
            //chargement des shapes
            foreach (var newShapeVm in formatGbr.Shapes)
            {
                ScaledForMenuShapeResources.Add(newShapeVm.ConvertToViewModel());
            }
            OnPropertyChanged("GetResources");
        }

        private void InitFromWebService()
        {}

        #endregion

        #region Resources

        public ObservableCollection<ShapeViewModel> ScaledForMenuShapeResources { get; set; }

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
