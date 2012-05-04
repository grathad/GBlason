using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using GBlason.ViewModel;
using GBlason.ViewModel.Contract;

namespace GBlason.Common.TemplateSelector
{
    public class PropertyTable : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is PropertyDisplayer)
            {
                var window = Application.Current.MainWindow;
                var propDis = item as PropertyDisplayer;
                if(propDis.PropertyValue is ShapeViewModel)
                    return window.FindResource("PropertyTableCellItemTemplate") as DataTemplate;
            }
            
            return null;
        }
    }
}
