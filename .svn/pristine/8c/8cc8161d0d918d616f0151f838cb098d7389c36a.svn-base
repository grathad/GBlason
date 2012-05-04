using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using GBlason.ViewModel;
using Microsoft.Win32;

namespace GBlason.Dictionary
{
    public partial class ApplicationRibbonCommandDictionary
    {
        #region Ribbon events


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

        private void OpenCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var dialogOpener = new OpenFileDialog
            {
                DefaultExt = String.Format(CultureInfo.CurrentCulture,
                                           ".{0}",
                                           GBlason.Properties.Resources.GBSFormatExtension),
                Filter = String.Format(CultureInfo.CurrentCulture,
                                       "{0} (.{1})|*.{1}",
                                       GBlason.Properties.Resources.
                                           OpenDialogBoxFormatDescription,
                                       GBlason.Properties.Resources.GBSFormatExtension),
                Multiselect = true
            };
            try
            {
                var result = dialogOpener.ShowDialog();
                if (result.Value)
                {
                    GbsFileViewModel.OpenFiles(dialogOpener.FileNames);
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

        #endregion

        #region canexecute

        private void RibbonCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion
    }
}
