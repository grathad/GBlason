using System;
using System.Xml.Serialization;
using GBL.Repository.Resources;

namespace GBL.Repository.CoatOfArms
{
    /// <summary>
    /// A shape is a description of the coat of arms national origin. This is for the main orginal shapes.
    /// The project is designed to support custom and personalized shapes. Thus allowing extra user created shapes.
    /// The main shapes are stored in this very same dll, allowing the soft to run not connected. The customs ones would be gotten by webservices.
    /// The geometry can be used as a unique identifier, but, for optimisation purpose and because same geometry but different shapes can be allowed, a unique identifier is given to the shape
    /// </summary>
    [Serializable]
    public class Shape
    {
        //path.Data = Geometry.Parse("M 100,200 C 100,25 400,350 400,175 H 280");
        /// <summary>
        /// Gets or sets the geometry.
        /// It's a XAML format that can be feed directly as a path logic (see the Microsoft path markup syntax)
        /// </summary>
        /// <value>
        /// The geometry.
        /// </value>
        [XmlElement]
        public String Geometry { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// To define any shape as a unique entity
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [XmlAttribute]
        public Guid Identifier { get; set; }


        /// <summary>
        /// Gets or sets the type of resource.
        /// <see cref="ResourceType"/>
        /// </summary>
        /// <value>
        /// The type of resource, to define if the data is from the soft or modified by the user.
        /// </value>
        [XmlAttribute]
        public ResourceType TypeOfResource { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }
    }
}
