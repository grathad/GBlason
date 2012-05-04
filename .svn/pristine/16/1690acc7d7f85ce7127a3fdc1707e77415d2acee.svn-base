using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GBSFormatManager;

namespace FormatManager.Serializer
{
    public class GbrManager : XmlManager
    {
        /// <summary>
        /// Loads the GBS file.
        /// </summary>
        /// <param name="pathName">Name of the path.</param>
        /// <param name="checkFormat">if set to <c>true</c> [check format].</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">Returned if there are no corresponding file</exception>
        public GbrFormat LoadGbrFile(String pathName, bool checkFormat = true)
        {
            var fileStream = PrepareFileStream(pathName, FileMode.Open);
            var shell = new GbrFormat();
            Deserialize(ref shell, fileStream);
            fileStream.Close();
            return shell;
        }
    }
}
