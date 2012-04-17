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
            foreach (var shape in formatGbr.Shapes)
            {
                var newShapeVM = new ShapeViewModel
                                     {
                                         Description = shape.Description,
                                         Geometry = shape.Geometry,
                                         Identifier = shape.Identifier,
                                         Name = shape.Name,
                                         PreferedHeight = shape.PathHeight,
                                         PreferedWidth = shape.PathWidth
                                     };
                var calculation = DefineScale(newShapeVM.Geometry, newShapeVM.PreferedHeight, newShapeVM.PreferedWidth,
                                              60, 60);
                newShapeVM.ScaleX = calculation.Item1;
                newShapeVM.ScaleY = calculation.Item2;
                ScaledForMenuShapeResources.Add(newShapeVM);
            }
            OnPropertyChanged("GetResources");
        }

        private Tuple<Double,Double> DefineScale(String geometry, uint PreferedH, uint PreferedW, uint wantedH, uint WantedW)
        {
            var path = new System.Windows.Shapes.Path {Data = Geometry.Parse(geometry)};
            
            var realWidth = path.Data.Bounds.Width;
            var realHeight = path.Data.Bounds.Height;

            //on applique le ratio
            var ratioX = WantedW/realWidth;
            var ratioY = wantedH/realHeight;

            return new Tuple<double, double>(ratioX, ratioY);
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
