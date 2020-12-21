using System;
using System.Collections.Generic;

namespace Utils.Enum
{
    /// <summary>
    /// Contract used to axtends enum attribute access
    /// </summary>
    public interface IEnumAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetAttribute<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetAttributes<T>();

        /// <summary>
        /// Find all the attributes for the given enumeration and the given type.
        /// With the value level attribute overwriting the enum level attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> FindAttributes<T>() where T : Attribute;
    }
}
