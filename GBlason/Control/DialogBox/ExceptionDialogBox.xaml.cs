﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GBlason.Common;
using GBlason.Common.Converter;

namespace GBlason.Control.DialogBox
{
    /// <summary>
    /// Interaction logic for ExceptionDialogBox.xaml
    /// </summary>
    public partial class ExceptionDialogBox : Window
    {
        public ExceptionDialogBox()
        {
            InitializeComponent();
        }

        public ExceptionDialogBox(CustomException cex)
        {
            var sti = new StringToImageConverter();
            switch (cex.Importance)
            {
                case ExceptionImportance.NoImpact:
                    ExceptionImportanceImage.Source =
                        (BitmapImage)sti.Convert(new[] { "/GBlason;component/Pictures/Icons16/information.png" },
                                                       typeof(BitmapImage),
                                                       String.Empty,
                                                       CultureInfo.CurrentCulture);
                    break;
                case ExceptionImportance.RestartAction:
                    ExceptionImportanceImage.Source =
                        (BitmapImage)sti.Convert(new[] { "/GBlason;component/Pictures/Icons16/error.png" },
                                                       typeof(BitmapImage),
                                                       String.Empty,
                                                       CultureInfo.CurrentCulture);
                    break;
                case ExceptionImportance.RestartApplication:
                    ExceptionImportanceImage.Source =
                        (BitmapImage)sti.Convert(new[] { "/GBlason;component/Pictures/Icons16/exclamation.png" },
                                                       typeof(BitmapImage),
                                                       String.Empty,
                                                       CultureInfo.CurrentCulture);
                    break;
                default:
                    ExceptionImportanceImage.Source =
                        (BitmapImage)sti.Convert(new[] { "/GBlason;component/Pictures/Icons16/information.png" },
                                                       typeof(BitmapImage),
                                                       String.Empty,
                                                       CultureInfo.CurrentCulture);
                    break;
            }
            ExceptionNameTB.Text = cex.Message;
            StackTraceTB.Text = cex.StackTrace;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            return;
        }
    }
}
