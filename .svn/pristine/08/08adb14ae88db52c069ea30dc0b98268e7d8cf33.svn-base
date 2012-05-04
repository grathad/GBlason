using System;
using System.Xml.Serialization;
using GBL.Repository.CoatOfArms;

namespace GBSFormatManager
{
    /// <summary>
    /// Represent the root of the saved xml file format (because a file may contain more than just the coat of arm treeview)
    /// </summary>
    [Serializable]
    public class GbsFormat
    {
        /// <summary>
        /// Gets or sets the XML coat of arms. Must be present in the coat of arms saving file of course.
        /// </summary>
        /// <value>
        /// The XML coat of arms.
        /// </value>
        public CoatOfArms XmlCoatOfArms { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [XmlAttribute]
        public String Version
        {
            get { return _version; }
            set { _version = value; }
        }

        private String _version = "0.9.0";
    }
}
