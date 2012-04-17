using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GBlason.ViewModel;

namespace GBlason.Control.CustomUserControl
{
    /// <summary>
    /// Interaction logic for ShapeViewerUC.xaml
    /// </summary>
    public partial class ShapeViewerUC
    {
        public ShapeViewerUC()
        {
            InitializeComponent();
        }

        #region ShapeViewModel property

        /// <summary>
        /// Represent the path given with the markup syntax (used by microsoft and svg)
        /// </summary>
        public ShapeViewModel Shape
        {
            get
            {
                return (ShapeViewModel)GetValue(ShapeProperty);
            }
            set
            {
                SetValue(ShapeProperty, value);
            }
        }

        public static DependencyProperty ShapeProperty = DependencyProperty.Register("Shape", typeof(ShapeViewModel), typeof(ShapeViewerUC),
            new PropertyMetadata(new ShapeViewModel()));

        public static readonly RoutedEvent ShapeChangedEvent = EventManager.RegisterRoutedEvent("ShapeChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<ShapeViewModel>), typeof(ShapeViewerUC));

        protected virtual void OnShapeChanged(ShapeViewModel oldValue, ShapeViewModel newValue)
        {
            var args = new RoutedPropertyChangedEventArgs<ShapeViewModel>(oldValue, newValue) { RoutedEvent = ShapeChangedEvent };
            RaiseEvent(args);
        }

        #endregion

        #region Fill property

        /// <summary>
        /// Represent the path given with the markup syntax (used by microsoft and svg)
        /// </summary>
        public Brush Fill
        {
            get
            {
                return (Brush)GetValue(FillProperty);
            }
            set
            {
                SetValue(FillProperty, value);
            }
        }

        public static DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(ShapeViewerUC),
            new PropertyMetadata(new SolidColorBrush()));

        public static readonly RoutedEvent FillChangedEvent = EventManager.RegisterRoutedEvent("FillChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Brush>), typeof(ShapeViewerUC));

        protected virtual void OnFillChanged(Brush oldValue, Brush newValue)
        {
            var args = new RoutedPropertyChangedEventArgs<Brush>(oldValue, newValue) { RoutedEvent = FillChangedEvent };
            RaiseEvent(args);
        }

        #endregion
    }
}
