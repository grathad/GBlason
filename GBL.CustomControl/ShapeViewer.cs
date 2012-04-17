using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GBL.CustomControl
{
    public class ShapeViewer : Control
    {
        static ShapeViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ShapeViewer), new FrameworkPropertyMetadata(typeof(ShapeViewer)));
        }

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

        public static DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(ShapeViewer),
            new PropertyMetadata(new SolidColorBrush()));

        public static readonly RoutedEvent FillChangedEvent = EventManager.RegisterRoutedEvent("FillChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Brush>), typeof(ShapeViewer));

        protected virtual void OnFillChanged(Brush oldValue, Brush newValue)
        {
            var args = new RoutedPropertyChangedEventArgs<Brush>(oldValue, newValue) { RoutedEvent = FillChangedEvent };
            RaiseEvent(args);
        }

        #endregion

        #region ScaleY property

        /// <summary>
        /// Represent the path given with the markup syntax (used by microsoft and svg)
        /// </summary>
        public Double ScaleY
        {
            get
            {
                return (Double)GetValue(ScaleYProperty);
            }
            set
            {
                SetValue(ScaleYProperty, value);
            }
        }

        public static DependencyProperty ScaleYProperty = DependencyProperty.Register("ScaleY", typeof(Double), typeof(ShapeViewer),
            new PropertyMetadata(Double.NaN));

        public static readonly RoutedEvent ScaleYChangedEvent = EventManager.RegisterRoutedEvent("ScaleYChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Double>), typeof(ShapeViewer));

        protected virtual void OnScaleYChanged(Double oldValue, Double newValue)
        {
            var args = new RoutedPropertyChangedEventArgs<Double>(oldValue, newValue) { RoutedEvent = ScaleYChangedEvent };
            RaiseEvent(args);
        }

        #endregion

        #region ScaleX property

        /// <summary>
        /// Represent the path given with the markup syntax (used by microsoft and svg)
        /// </summary>
        public Double ScaleX
        {
            get
            {
                return (Double)GetValue(ScaleXProperty);
            }
            set
            {
                SetValue(ScaleXProperty, value);
            }
        }

        public static DependencyProperty ScaleXProperty = DependencyProperty.Register("ScaleX", typeof(Double), typeof(ShapeViewer),
            new PropertyMetadata(Double.NaN));

        public static readonly RoutedEvent ScaleXChangedEvent = EventManager.RegisterRoutedEvent("ScaleXChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Double>), typeof(ShapeViewer));

        protected virtual void OnScaleXChanged(Double oldValue, Double newValue)
        {
            var args = new RoutedPropertyChangedEventArgs<Double>(oldValue, newValue) { RoutedEvent = ScaleXChangedEvent };
            RaiseEvent(args);
        }

        #endregion

        #region Geometry property

        /// <summary>
        /// Represent the path given with the markup syntax (used by microsoft and svg)
        /// </summary>
        public String Geometry
        {
            get
            {
                return (String)GetValue(GeometryProperty);
            }
            set
            {
                SetValue(GeometryProperty, value);
            }
        }

        public static DependencyProperty GeometryProperty = DependencyProperty.Register("Geometry", typeof(String), typeof(ShapeViewer),
            new PropertyMetadata(String.Empty, OnGeometryInvalidated));

        public static readonly RoutedEvent GeometryChangedEvent = EventManager.RegisterRoutedEvent("GeometryChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<String>), typeof(ShapeViewer));

        protected virtual void OnGeometryChanged(String oldValue, String newValue)
        {
            var args = new RoutedPropertyChangedEventArgs<String>(oldValue, newValue)
                           {RoutedEvent = GeometryChangedEvent};
            RaiseEvent(args);
        }

        private static void OnGeometryInvalidated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (ShapeViewer)d;
            var oldValue = (String)e.OldValue;
            var newValue = (String)e.NewValue;
            obj.OnGeometryChanged(oldValue, newValue);
        }
        #endregion
    }
}
