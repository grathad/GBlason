using System;
using Utils.TypeHelper;

namespace Utils.Assembly
{
    /// <summary>
    /// Tools to manipulate enums and extract meta information
    /// </summary>
    public static class AssemblyAttributeExtensions
    {
        /// <summary>
        /// Entry point to access the extension method for enum's attribute helpers
        /// </summary>
        /// <param name="target">the enum to access for the extension</param>
        /// <returns>The contract implementation of the extension</returns>
        public static IAssemblyAttribute Assembly(this System.Reflection.Assembly target)
        {
            return AssemblyAttributeFactory(target);
        }

        internal static Func<System.Reflection.Assembly, IAssemblyAttribute> AssemblyAttributeFactory { get; set; }

        static AssemblyAttributeExtensions()
        {
            AssemblyAttributeFactory = st => new AssemblyAttributeHelper(st);
        }
    }
}
