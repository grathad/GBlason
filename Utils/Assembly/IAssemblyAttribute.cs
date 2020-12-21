using System;
using System.Collections.Generic;
using System.Reflection;

namespace Utils.Assembly
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAssemblyAttribute
    {
        /// <summary>
        /// Retrieves a collection of custom attributes of a specified type that are applied to the given type
        /// </summary>
        /// <typeparam name="T">The type of attribute to search for.</typeparam>
        /// <returns>
        /// A collection of the custom attributes that are applied to element and that match 
        /// T, or an empty collection if no such attributes exist.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">if <see cref="Type"/> is null</exception>
        IEnumerable<T> GetCustomAttributes<T>() where T : Attribute;

        /// <summary>
        /// Gets an System.Reflection.AssemblyName for this assembly.
        /// </summary>
        /// <returns>An object that contains the fully parsed display name for this assembly.</returns>
        AssemblyName GetName();
    }
}