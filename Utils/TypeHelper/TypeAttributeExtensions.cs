using System;

namespace Utils.TypeHelper
{
    /// <summary>
    /// Tools to manipulate enums and extract meta information
    /// </summary>
    public static class TypeAttributeExtensions
    {
        /// <summary>
        /// Entry point to access the extension method for enum's attribute helpers
        /// </summary>
        /// <param name="target">the enum to access for the extension</param>
        /// <returns>The contract implementation of the extension</returns>
        public static ITypeAttribute Type(this Type target)
        {
            return TypeAttributeFactory(target);
        }

        /// <summary>
        /// The factory to provide the instance of contract for <see cref="ITypeAttribute"/> helper
        /// </summary>
        public static Func<Type, ITypeAttribute> TypeAttributeFactory { get; internal set; }

        static TypeAttributeExtensions()
        {
            TypeAttributeFactory = st => new TypeAttributeHelper(st);
        }
    }
}
