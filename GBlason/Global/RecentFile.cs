using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.IO;
using FormatManager.Serializer;

namespace GBlason.Global
{
    /// <summary>
    /// Store the recent file name, for the presentation recent file purpose only
    /// </summary>
    [Serializable]
    public class RecentFile
    {
        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        /// <value>
        /// The file name.
        /// </value>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the path without the file name (access only).
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public String Path { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this file is fixed (presentation layer wise).
        /// </summary>
        /// <value>
        ///   <c>true</c> if this file is fixed; otherwise, <c>false</c>.
        /// </value>
        public bool IsFixed { get; set; }

    }

    /// <summary>
    /// Read the properties information and load it
    /// Format of a recent opened file string storage xml auto serialized
    /// </summary>
    public class RecentFiles : ObservableCollection<RecentFile>
    {
        public RecentFiles()
        {
            try
            {
                var settingFiles = Properties.Settings.Default.RecentFiles;
                foreach (var fName in settingFiles)
                {
                    try
                    {
                        var recentFile = new RecentFile();
                        XmlManager.Deserialize(ref recentFile,
                                               new MemoryStream(Encoding.Unicode.GetBytes(fName)));
                        Add(recentFile);
                    }
                    catch (SecurityException)
                    {
                        //pas le droit d'accès ... on met le fichier en rouge ?
                        //TODO @Grathad gestion globale des exceptions : dans ce cas on passe toujours au fichier suivant
                        continue;
                    }
                    catch (ArgumentException)
                    {
                        //fichier de settings corrompu .. à gérer
                        //TODO @Grathad gestion globale des exceptions : dans ce cas le fichier de settings est corrompu .. à gérer
                        continue;
                    }
                    // les autres exceptions petent volontairement.
                }
            }
            catch (Exception)
            {
                //TODO @Grathad gestion globale des exceptions : dans ce cas le fichier de settings est corrompu .. à gérer
                return;
            }

        }
    }
}
