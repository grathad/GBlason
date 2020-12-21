using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Utils.Assembly
{
    /// <summary>
    /// 
    /// </summary>
    public class AssemblyAttributeHelper : IAssemblyAttribute
    {
        internal readonly System.Reflection.Assembly Type;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="st"></param>
        public AssemblyAttributeHelper(System.Reflection.Assembly st)
        {
            Type = st;
        }

        /// <inheritdoc />
        public virtual IEnumerable<T> GetCustomAttributes<T>() where T : Attribute => Type.GetCustomAttributes<T>();


        /// <inheritdoc />
        public virtual AssemblyName GetName() => Type.GetName();
    }
}