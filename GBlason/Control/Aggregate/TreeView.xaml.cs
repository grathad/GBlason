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
using GBlason.ViewModel;
using GBlason.ViewModel.Contract;

namespace GBlason.Control.Aggregate
{
    /// <summary>
    /// Interaction logic for TreeView.xaml
    /// </summary>
    public partial class TreeView
    {
        public TreeView()
        {
            InitializeComponent();
        }

        private void TreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            GlobalApplicationViewModel.GetApplicationViewModel.CurrentlyDisplayedFile.CurrentlySelectedComponent =
                e.NewValue as CoatOfArmComponent;
        }
    }
}
