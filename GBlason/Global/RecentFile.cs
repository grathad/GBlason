using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Security;
using System.Text;
using System.IO;
using System.Xml;
using FormatManager.Serializer;
using GBlason.Properties;

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

        /// <summary>
        /// Gets or sets the last open UTC.
        /// </summary>
        /// <value>
        /// The last open UTC. Used to allow the sorting of the last files
        /// </value>
        public DateTime LastOpenUtc { get; set; }

    }
}
