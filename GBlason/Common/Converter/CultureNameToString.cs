using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows.Data;
using GBlason.Properties;

namespace GBlason.Common.Converter
{
    public class CultureNameToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var strValue = value as String;
            if (String.IsNullOrEmpty(strValue))
                return strValue;
            var rm = new ResourceManager(typeof (Resources));
            return rm.GetString(strValue.Replace('-', '_'), CultureInfo.GetCultureInfo(strValue));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
