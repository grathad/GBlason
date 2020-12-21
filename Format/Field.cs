using System;
using System.ComponentModel;

namespace Blazon.Format
{
    /// <summary>
    /// The field is the first and only required element of the shield.
    /// The commencement of any blazon is of necessity a description of the field, the one word signifying its colour being employed if it be a simple field; 
    /// or, if it be composite, such terms as are necessary.
    /// The field is abstract because it can't exist on itself, it needs to be a specific definition.
    /// </summary>
    public abstract class Field
    {
        
    }

    public abstract class SimpleField : Field
    {
        public Tincture Tincture { get; set; }
    }

    public abstract class ComplexField : Field
    {
        
    }

    public abstract class Tincture
    {
        
    }

    public abstract class SimpleTincture : Tincture
    {
        
    }

    public class Colour : SimpleTincture
    {
        public TinctureColours Value { get; set; }
    }

    public class Metal : SimpleTincture
    {
        public TinctureMetals Value { get; set; }
    }

    public class Semé : Tincture
    {
        public SimpleTincture Background { get; set; }

        public SimpleMobileObject Charge { get; set; }
    }

    public class Fur : Semé
    {
        
    }

    public class Vairé : Fur
    {
        
    }

    public abstract class SimpleMobileObject
    {
        
    }

    public abstract class Division : ComplexField
    {
        
    }

    public class DivisionBy2 : Division
    {
        
    }

    public class DivisionBy3 : Division
    {
        
    }

    public class DivisionBy4 : Division
    {
        
    }

    public abstract class FieldVariation : ComplexField
    {
        
    }

    /// <summary>
    /// The metal default
    /// </summary>
    public enum TinctureMetals
    {
        /// <summary>
        /// Or
        /// </summary>
        Or,
        /// <summary>
        /// Argent
        /// </summary>
        Argent
    }

    /// <summary>
    /// The list of default colours
    /// </summary>
    public enum TinctureColours
    {
        /// <summary>
        /// Default blue
        /// </summary>
        Azure,
        /// <summary>
        /// Default red
        /// </summary>
        Gules,
        /// <summary>
        /// Default black
        /// </summary>
        Sable,
        /// <summary>
        /// Default green
        /// </summary>
        Vert,
        /// <summary>
        /// Default "skin color"
        /// </summary>
        Carnation,
        /// <summary>
        /// Default purple
        /// </summary>
        Purpure,
        /// <summary>
        /// Default blood or dark red
        /// </summary>
        Sanguine,
        /// <summary>
        /// Default orange
        /// </summary>
        Orange,
        /// <summary>
        /// Default dark purple
        /// </summary>
        Murrey,
        White,
        Tenné,
        BloodRed,
        AshColour,
        /// <summary>
        /// or bleu du ciel
        /// </summary>
        BleuCéleste,
        /// <summary>
        /// or columbine
        /// </summary>
        Amaranth,
        /// <summary>
        /// Not a colour per say, dynamic based on the object on which it is present, but stored here for simplicity
        /// </summary>
        Proper
    }

    

}
