using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using GBL.Repository.CoatOfArms;

namespace FormatManager
{
    /// <summary>
    /// Handle the resource root node of a Xml resource file that contains the soft resources
    /// </summary>
    [Serializable]
    public class GbrFormat
    {
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

        /// <summary>
        /// This is not an interface, because this object is meant to be serialized and deserialized
        /// </summary>
        public List<Shape> Shapes { get; set; }
    }
}
