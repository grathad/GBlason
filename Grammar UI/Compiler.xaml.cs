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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Grammar_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Compiler
    {
        public Compiler()
        {
            InitializeComponent();
        }

        public ViewModel Context { get; init; } = new ViewModel();

        private void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var button = sender as Button;
            if (openFileDialog.ShowDialog() == true)
            {
                if (button.Name.Contains("Root"))
                {
                    Context.RootGrammarFilePath = openFileDialog.FileName;
                    Context.Resources.LoadRoot(openFileDialog.FileName);
                }
                else if (button.Name.Contains("Keywords"))
                {
                    Context.KeywordsFilePath = openFileDialog.FileName;
                    Context.Resources.LoadKeywords(openFileDialog.FileName);
                }
                else if (button.Name.Contains("Custom"))
                {
                    Context.CustomGrammarFilePath = openFileDialog.FileName;
                }
                //Context.FilePath = openFileDialog.FileName;
                //if (File.Exists(Context.FilePath))
                //{
                //    Context.LoadFromFile(Context.FilePath);
                //}
            }
        }
    }
}
