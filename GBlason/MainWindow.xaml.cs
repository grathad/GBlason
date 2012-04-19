using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GBlason.Global;
using GBlason.Properties;
using GBlason.ViewModel;
using Microsoft.Win32;
using Microsoft.Windows.Controls.Ribbon;

namespace GBlason
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [STAThreadAttribute]
        [DebuggerNonUserCodeAttribute]
        public static void Main()
        {
            bool onlyInstance;
            var mutex = new Mutex(true, "UniqueGBlasonInstance", out onlyInstance);
            if (!onlyInstance)
            {
                return;
            }
            var app = new App();
            app.InitializeComponent();
            app.Run();
            GC.KeepAlive(mutex);
        }

        public MainWindow()
        {
            ApplicationStarter();
            Closing += MainWindow_Closing;
        }

        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            //sauvegarde des settings
            var tamponFile = new StringCollection();
            ApplicationSettingsManager.SaveCollectionSettings(ref tamponFile, GlobalApplicationViewModel.GetApplicationViewModel.RecentFiles);
            Settings.Default.RecentFiles = tamponFile;
            var tamponDir = new StringCollection();
            ApplicationSettingsManager.SaveCollectionSettings(ref tamponDir, GlobalApplicationViewModel.GetApplicationViewModel.RecentDirectories);
            Settings.Default.RecentDirectories = tamponDir;
            ApplicationSettingsManager.SavePreference();
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
                GlobalApplicationViewModel.GetApplicationViewModel.AddRecentFilesCollection(ApplicationSettingsManager.LoadCollectionSettings<RecentFileViewModel>(Settings.Default.RecentFiles));
                GlobalApplicationViewModel.GetApplicationViewModel.AddRecentDirectoriesCollection(ApplicationSettingsManager.LoadCollectionSettings<RecentFileViewModel>(Settings.Default.RecentDirectories));

                ApplicationSettingsManager.LoadPreferences();
            }
            catch (InvalidOperationException ioe)
            {
                //TODO @Grathad : mettre en place des fenêtres d'erreurs en fonction de la gravité, et afficher les messages, proprement avec des ressources
                MessageBox.Show("Erreur lors du chargement des settings recent files (nouvelle version ?) " + ioe.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Settings.Default.RecentFiles.Clear();
                Settings.Default.RecentDirectories.Clear();
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
                Settings.Default.RecentDirectories.Clear();
            }
        }

        #region events
        private void RecentObjectSelected(object sender, ExecutedRoutedEventArgs e)
        {
            var recentFile = e.Parameter as RecentFileViewModel;
            if (recentFile == null)
            {
                MessageBox.Show("Erreur lors du chargement des objets resource ", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var path = recentFile.Path;
            if (!File.Exists(path))
            {
                //it should be a directory
                if (Directory.Exists(path))
                {
                    OpenCommandExecuted(this, e);
                }
                return;
            }
            GbsFileViewModel.OpenFiles(new[] { path });
            TabHome.IsSelected = true;
        }


        private void OpenCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var dirSelected = e.Parameter as RecentFileViewModel;

            var dialogOpener = new OpenFileDialog
            {
                DefaultExt = String.Format(CultureInfo.CurrentCulture,
                                           ".{0}",
                                           Properties.Resources.GBSFormatExtension),
                Filter = String.Format(CultureInfo.CurrentCulture,
                                       "{0} (.{1})|*.{1}",
                                       Properties.Resources.OpenDialogBoxFormatDescription,
                                       Properties.Resources.GBSFormatExtension),
                Multiselect = true
            };
            if (dirSelected != null)
                dialogOpener.InitialDirectory = dirSelected.Path;
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

        private void NewCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var newCoatOfArmsFile = new GbsFileViewModel{FileName = GlobalApplicationViewModel.GetNewFileName()};
            GlobalApplicationViewModel.GetApplicationViewModel.OpenedFiles.Add(newCoatOfArmsFile);
            GlobalApplicationViewModel.GetApplicationViewModel.CurrentlyDisplayedFile = newCoatOfArmsFile;
            TabHome.IsSelected = true;
        }
        #endregion

    }
}
