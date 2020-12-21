using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Utils.TypeHelper
{
    /// <summary>
    /// 
    /// </summary>
    public class TypeAttributeHelper : ITypeAttribute
    {
        internal readonly Type Type;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="st"></param>
        public TypeAttributeHelper(System.Type st)
        {
            Type = st;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public virtual bool ContainsInterface<TInterface>()
        {
            return Type?.GetTypeInfo().GetInterfaces().Contains(typeof(TInterface)) ?? false;
        }

        /// <inheritdoc/>
        public virtual TypeInfo GetTypeInfo()
        {
            return Type.GetTypeInfo();
        }
    }
}