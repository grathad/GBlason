using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using GBlason.Global;
using GBlason.Properties;
using GBlason.ViewModel;
using Microsoft.Win32;

namespace GBlason
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            ApplicationStarter();
            Closing += MainWindow_Closing;
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //sauvegarde des settings
            ApplicationSettingsManager.SaveRecentFiles();
            Settings.Default.Save();
        }

        /// <summary>
        /// Steps to start properly the application
        /// </summary>
        private void ApplicationStarter()
        {
            //chargement des settings
            GlobalApplicationViewModel.GetApplicationViewModel.RecentFiles.Clear();
            try
            {
                ApplicationSettingsManager.LoadRecentFiles();
            }
            catch (InvalidOperationException ioe)
            {
                //TODO @Grathad : mettre en place des fenêtres d'erreurs en fonction de la gravité, et afficher les messages, proprement avec des ressources
                MessageBox.Show("Erreur lors du chargement des settings recent files (nouvelle version ?) " + ioe.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Settings.Default.RecentFiles.Clear();
            }
            Settings.Default.Save();

            //initialisation des objets
            try
            {
                GbrFileViewModel.GetResources.InitFromFile();
            }
            catch (NullReferenceException nex)
            {
                //TODO @Grathad : mettre en place des fenêtres d'erreurs en fonction de la gravité, et afficher les messages, proprement avec des ressources
                MessageBox.Show("Erreur lors du chargement des objets resource " + nex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Settings.Default.RecentFiles.Clear();
            }
        }

        #region events

        private void RibbonHighlightingListMostRecentFileSelected(object sender, Microsoft.Windows.Controls.Ribbon.MostRecentFileSelectedEventArgs e)
        {
            var path = ((RecentFile) e.SelectedItem).Path;
            if (!File.Exists(path))
            {
                //remove from recent and leave it here
                GlobalApplicationViewModel.GetApplicationViewModel.RecentFiles.Remove((RecentFile)e.SelectedItem);
                return;
            }
            GbsFileViewModel.OpenFiles(new[] { path });
        }

        #endregion

        private void OpenCommandCanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCommandExecuted(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var dialogOpener = new OpenFileDialog
            {
                DefaultExt = String.Format(CultureInfo.CurrentCulture,
                                           ".{0}",
                                           Properties.Resources.GBSFormatExtension),
                Filter = String.Format(CultureInfo.CurrentCulture,
                                       "{0} (.{1})|*.{1}",
                                       Properties.Resources.
                                           OpenDialogBoxFormatDescription,
                                       Properties.Resources.GBSFormatExtension),
                Multiselect = true
            };
            try
            {
                var result = dialogOpener.ShowDialog();
                if (result.Value)
                {
                    GbsFileViewModel.OpenFiles(dialogOpener.FileNames);
                    TabHome.IsSelected = true;
                }

            }
            catch (FileNotFoundException fex)
            {
                //TODO @Grathad : mettre en place des fenêtres d'erreurs en fonction de la gravité, et afficher les messages, proprement avec des ressources
                MessageBox.Show("Plantu, file not found " + fex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ioex)
            {
                //TODO @grathad : mettre en place des fenêtres d'erreurs en fonction de la gravité, et afficher les messages, proprement avec des ressources
                MessageBox.Show("Plantu, impossible de lire le format " + ioex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
