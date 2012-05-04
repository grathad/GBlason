using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using GBL.Repository.CoatOfArms;

namespace GBL.Repository.Resources
{
    /// <summary>
    /// Handle the resource loading from the resource files (local) and from the webservice (custom)
    /// Allow resource saving to file (custom) and to webservice (custom)
    /// </summary>
    public class ResourceManager
    {
        /// <summary>
        /// Gets or sets the available shapes custom and original.
        /// </summary>B
        /// <value>
        /// The available shapes.
        /// </value>
        public IEnumerable<Shape> Shapes { get; private set; }


        /// <summary>
        /// Updates the resource from the given GBR file.
        /// Use the current Culture to load the names and description data (use the default resx file if the current culture is non available)
        /// </summary>
        /// <param name="gbrFilePath">The GBR file path.</param>
        public void UpdateResourceFromGbr(String gbrFilePath)
        {
            UpdateResourceFromGbr(gbrFilePath, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Updates the resource from the given GBR file.
        /// Loading from the given culture (if the culture is not supported the default one will be used)
        /// </summary>
        /// <param name="gbrFilePath">The GBR file path.</param>
        /// <param name="culture">The culture.</param>
        public void UpdateResourceFromGbr(String gbrFilePath, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the new shapes (only for the custom types)s.
        /// </summary>
        /// <param name="toAdd">the shapes to add.</param>
        public void AddNewShapes(IEnumerable<Shape> toAdd)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the resource from web service.
        /// </summary>
        public void UpdateResourceFromWebService()
        {
            throw new NotImplementedException();
        }


    }
}
