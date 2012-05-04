using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Xml;
using FormatManager.Serializer;
using GBlason.Properties;
using GBlason.ViewModel;
using Color = System.Windows.Media.Color;

namespace GBlason.Global
{
    public class ApplicationSettingsManager
    {
        /// <summary>
        /// Saves the recent files and directories in the settings.
        /// </summary>
        /// <remarks>The setting is NOT SAVED in this function</remarks>
        public static void SaveCollectionSettings<T>(ref StringCollection settingObject, IList<T> collectionToSave) where T : class
        {
            if (settingObject == null)
                settingObject = new StringCollection();

            settingObject.Clear();
            foreach (var rF in collectionToSave)
            {
                var stringBuilder = new StringBuilder();
                var writer = XmlWriter.Create(stringBuilder, new XmlWriterSettings { OmitXmlDeclaration = true });
                XmlManager.Serialize(rF, ref writer);
                var serializedRecentSave = stringBuilder.ToString();
                settingObject.Add(serializedRecentSave);
            }
        }

        /// <summary>
        /// Load the list of recent files and directories, from the settings
        /// </summary>
        public static IList<T> LoadCollectionSettings<T>(StringCollection settingObject) where T : new()
        {
            var retour = new Collection<T>();
            if (settingObject == null) return retour;
            foreach (var fName in settingObject)
            {
                var recentObject = new T();
                XmlManager.Deserialize(ref recentObject,
                                       new MemoryStream(Encoding.Unicode.GetBytes(fName)));

                retour.Add(recentObject);
            }
            return retour;
        }

        public static void LoadPreferences()
        {
            //la taille de la fenetre
            GlobalApplicationViewModel.GetApplicationViewModel.MainWindowWidth = Settings.Default.PreferredSize.Width;
            GlobalApplicationViewModel.GetApplicationViewModel.MainWindowHeight = Settings.Default.PreferredSize.Height;
            //La position de la fenetre
            GlobalApplicationViewModel.GetApplicationViewModel.MainWindowTop = Settings.Default.PreferredPosition.Y;
            GlobalApplicationViewModel.GetApplicationViewModel.MainWindowLeft = Settings.Default.PreferredPosition.X;
            //L'etat de la fenetre
            GlobalApplicationViewModel.GetApplicationViewModel.MainWindowState = Settings.Default.PreferredState;
            //color preferences
            GlobalApplicationViewModel.GetApplicationViewModel.BlasonViewerBackgroundColor = new SolidColorBrush(Color.FromArgb(
                Settings.Default.BackgroundViewerColor.A,
                Settings.Default.BackgroundViewerColor.R,
                Settings.Default.BackgroundViewerColor.G,
                Settings.Default.BackgroundViewerColor.B));
            //language preference
            GlobalApplicationViewModel.GetApplicationViewModel.PropertyPreferenceCultureInfo = Settings.Default.PreferredLanguage;

        }

        public static void SavePreference()
        {
            //la taille de la fenetre
            Settings.Default.PreferredSize = new Size(GlobalApplicationViewModel.GetApplicationViewModel.MainWindowWidth, GlobalApplicationViewModel.GetApplicationViewModel.MainWindowHeight);
            //La position de la fenetre
            Settings.Default.PreferredPosition = new Point(GlobalApplicationViewModel.GetApplicationViewModel.MainWindowLeft, GlobalApplicationViewModel.GetApplicationViewModel.MainWindowTop);
            //L'etat de la fenetre
            Settings.Default.PreferredState = GlobalApplicationViewModel.GetApplicationViewModel.MainWindowState;
            //language preference
            Settings.Default.PreferredLanguage = GlobalApplicationViewModel.GetApplicationViewModel.PropertyPreferenceCultureInfo;
        }
    }
}
