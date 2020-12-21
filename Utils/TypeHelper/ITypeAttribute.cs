using System.Reflection;

namespace Utils.TypeHelper
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITypeAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        bool ContainsInterface<TInterface>();

        /// <summary>
        /// Wrap the <see cref="IntrospectionExtensions.GetTypeInfo"/> for unit test purposes
        /// </summary>
        /// <returns>The type info for the type</returns>
        TypeInfo GetTypeInfo();
    }
}