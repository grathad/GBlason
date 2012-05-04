using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GBL.Repository.CoatOfArms;
using GBSFormatManager;

namespace FormatManager.Serializer
{
    /// <summary>
    /// Handle the I/O operation with the GBS Format
    /// </summary>
    public class GbsManager : XmlManager
    {
        /// <summary>
        /// Saves as GBS.
        /// </summary>
        /// <param name="toSave">the Coat of Arms To save.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="checkFormat">if set to <c>true</c> [check format].</param>
        /// <exception cref="UnauthorizedAccessException">If the file exist and is not writable</exception>
        /// <exception cref="InvalidOperationException">if a serialization error occur</exception>
        public void SaveAsGbs(CoatOfArms toSave, String filePath, bool checkFormat = true)
        {
            //TODO @Grathad : managing the XSD to check on the format.
            var shell = new GbsFormat {XmlCoatOfArms = toSave};
            var fileStream = PrepareFileStream(filePath, FileMode.Create);
            Serialize(shell, ref fileStream);
            fileStream.Close();
        }

        /// <summary>
        /// Loads the GBS file.
        /// </summary>
        /// <param name="pathName">Name of the path.</param>
        /// <param name="checkFormat">if set to <c>true</c> [check format].</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">Returned if there are no corresponding file</exception>
        public GbsFormat LoadGbsFile(String pathName, bool checkFormat = true)
        {
            var fileStream = PrepareFileStream(pathName, FileMode.Open);
            var shell = new GbsFormat();
            Deserialize(ref shell, fileStream);
            fileStream.Close();
            return shell;
        }
        
    }
}
