using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Ebnf_UI
{
    /// <summary>
    /// Interaction logic for EbnfUI.xaml
    /// </summary>
    public partial class EbnfUI
    {
        public EbnfUI()
        {
            InitializeComponent();
        }

        public ViewModel Context { get; init; } = new ViewModel();

        private void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Context.FileStream = File.OpenRead(openFileDialog.FileName);
                Context.FilePath = Context.FileStream.Name;
                Context.EbnfParser.Parse(Context.FileStream);
            }
        }

        private void searchRulesButton_Click(object sender, RoutedEventArgs e)
        {
            Context.EbnfParser.Filter(searchRulesText.Text);
        }

        private void GrammarContentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (sender as ListView)?.SelectedItem as TreeElementReferenceViewModel;
            if (selectedItem?.RealElement != null)
            {
                Context.EbnfParser.SelectedItem = selectedItem;
            }
        }

        private void GrammarContentTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var tv = (sender as TreeView);
            var selectedItem = tv?.SelectedItem as TreeElementReferenceViewModel;

            if (tv?.SelectedItem != null)
            {
                var container = FindTreeViewSelectedItemContainer(tv, tv.SelectedItem);
                if (container != null)
                {
                    container.IsSelected = false;
                }
            }

            if (selectedItem?.RealElement != null)
            {
                Context.EbnfParser.SelectedItem = selectedItem;
            }
            e.Handled = true;
        }

        private static TreeViewItem FindTreeViewSelectedItemContainer(ItemsControl root, object selection)
        {
            var item = root?.ItemContainerGenerator.ContainerFromItem(selection) as TreeViewItem;
            if (item == null && root?.Items != null && root.Items.Count > 0)
            {
                foreach (var subItem in root.Items)
                {
                    var subItemControl = (TreeViewItem)root.ItemContainerGenerator.ContainerFromItem(subItem);
                    if(subItemControl == null)
                    {
                        return null;
                    }
                    item = FindTreeViewSelectedItemContainer(subItemControl, selection);
                    if (item != null)
                    {
                        break;
                    }
                }
            }

            return item;
        }
    }
}
