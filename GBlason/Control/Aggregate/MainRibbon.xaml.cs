using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GBlason.Control.Aggregate
{
    /// <summary>
    /// Interaction logic for MainRibbon.xaml
    /// </summary>
    public partial class MainRibbon : UserControl
    {
        public MainRibbon()
        {
            InitializeComponent();
        }

        #region Ribbon events

        private void RibbonCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OnPaste(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void OnCut(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void OnCopy(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void RibbonCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }

        #endregion
    }
}
