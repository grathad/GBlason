using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using FormatManager.Serializer;
using GBlason.Properties;
using GBlason.ViewModel;

namespace GBlason.Global
{
    public class ApplicationSettingsManager
    {
        /// <summary>
        /// Saves the recent files in the settings.
        /// </summary>
        public static void SaveRecentFiles()
        {
            if (Settings.Default.RecentFiles == null)
                Settings.Default.RecentFiles = new StringCollection();

            Settings.Default.RecentFiles.Clear();
            foreach (var rF in GlobalApplicationViewModel.GetApplicationViewModel.RecentFiles)
            {
                var stringBuilder = new StringBuilder();
                var writer = XmlWriter.Create(stringBuilder, new XmlWriterSettings { OmitXmlDeclaration = true });
                XmlManager.Serialize(rF, ref writer);
                var serializedRecentSave = stringBuilder.ToString();

                if (Settings.Default.RecentFiles == null)
                {
                    Settings.Default.RecentFiles = new StringCollection();
                }
                Settings.Default.RecentFiles.Add(serializedRecentSave);
            }
        }

        public static void LoadRecentFiles()
        {
            foreach (var fName in Settings.Default.RecentFiles)
            {
                var recentFile = new RecentFile();
                XmlManager.Deserialize(ref recentFile,
                                       new MemoryStream(Encoding.Unicode.GetBytes(fName)));

                GlobalApplicationViewModel.GetApplicationViewModel.RecentFiles.Add(recentFile);
            }
        }
    }
}
