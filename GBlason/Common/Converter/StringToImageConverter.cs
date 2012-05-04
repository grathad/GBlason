using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GBlason.Common.Converter
{
    public class StringToImageConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var strParameter = parameter as string;
            
            String strValue;
            if (String.IsNullOrEmpty(strParameter))
                strValue = values.Aggregate((st, next) => st.ToString() + next.ToString()).ToString();
            else
            {
                strValue = String.Format(culture,
                              strParameter,
                              values);
            }
            if (String.IsNullOrEmpty(strValue)) return null;

            if (PictureManager.ImageStorage.ContainsKey(strValue))
                return PictureManager.ImageStorage[strValue];
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(strValue);
            image.EndInit();

            PictureManager.ImageStorage.Add(strValue, image);

            return image;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
