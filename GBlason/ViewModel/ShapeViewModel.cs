using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using GBL.Repository.Resources;
using GBlason.Culture;
using GBlason.ViewModel.Contract;
using ResourceManager = System.Resources.ResourceManager;

namespace GBlason.ViewModel
{
    /// <summary>
    /// Presentation view model to store the data for the shape property.
    /// The shape is used as a coat of arm property. Or as a resource (to edit the coat of arm property)
    /// </summary>
    public class ShapeViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the technical name.
        /// </summary>
        /// <value>
        /// The name, used as a resource key for the default shapes.
        /// </value>
        public String Name { get; set; }

        /// <summary>
        /// Gets the localized name of the shape. Only available for the default shapes defined in the application (no custom shapes).
        /// Return the technical name if no key is found in the resource files
        /// </summary>
        /// <value>
        /// The name of the localized.
        /// </value>
        public String LocalizedName
        {
            get
            {
                var manager = new ResourceManager(typeof(BlasonVocabulary));
                var name = manager.GetString(Name);
                return name ?? Name;
            }
        }

        /// <summary>
        /// Gets or sets the geometry of the shape, define the whole coat of arm base shape.
        /// </summary>
        /// <value>
        /// The geometry.
        /// </value>
        public String Geometry { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public String Description { get; set; }

        /// <summary>
        /// Gets or sets the identifier. To be sure even custom added element get a unique identifier access
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets or sets the width the user want to show the form (can be different from the original width)
        /// </summary>
        /// <value>
        /// The width of the prefered.
        /// </value>
        public ushort PreferedWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the prefered.
        /// </summary>
        /// <value>
        /// The height of the prefered.
        /// </value>
        public ushort PreferedHeight { get; set; }

        public ResourceType TypeOfResource { get; set; }

        #region INotifyPropertyChanged Members

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

    }
}
