using System;

namespace Utils.Enum
{
    /// <summary>
    /// Tools to manipulate enums and extract meta information
    /// </summary>
    public static class EnumAttributeExtensions
    {
        /// <summary>
        /// Entry point to access the extension method for enum's attribute helpers
        /// </summary>
        /// <param name="target">the enum to access for the extension</param>
        /// <returns>The contract implementation of the extension</returns>
        public static IEnumAttribute Attributes(this System.Enum target)
        {
            return EnumAttributeFactory(target);
        }

        internal static Func<System.Enum, IEnumAttribute> EnumAttributeFactory { get; set; }

        static EnumAttributeExtensions()
        {
            EnumAttributeFactory = st => new EnumAttributeHelper(st);
        }
    }
}