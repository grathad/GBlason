using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Grammar.PluginBase.Token;

namespace Format_Debugger
{
    public class LeftParentVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Token token && token != null)
            {
                if (token.Parent == null)
                {
                    return Visibility.Hidden;
                }
                //if the parent has another children BEFORE this one then we return visible
                return HasRightBrother(token, token.Parent as ContainerToken) ? Visibility.Visible : Visibility.Hidden;
            }

            return Visibility.Hidden;
        }

        protected bool HasRightBrother(Token toTest, ContainerToken parent)
        {
            if(parent == null || toTest == null | (!parent.Children?.Any() == null))
            {
                return false;
            }

            var pos = parent.Children.FindIndex(t => t.UniqueId == toTest.UniqueId);
            //there is a left brother if 1 the token to test is in the list of children
            //2 is not the first

            return pos > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
