using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Utils.Enum
{
    /// <inheritdoc />
    /// <summary>
    /// Implementation of the contract to extend the enum support to easily read an attribute
    /// </summary>
    internal class EnumAttributeHelper : IEnumAttribute
    {
        internal readonly System.Enum Enumeration;

        public EnumAttributeHelper(System.Enum en)
        {
            Enumeration = en;
        }

        /// <summary>
        /// Get the first of an enum value's attribute.
        /// </summary>
        /// <typeparam name="T">A class used as an attribute on an enumeration value. Needs to be the exact type</typeparam>
        /// <returns>The value if found or default(TExpected)</returns>
        public virtual T GetAttribute<T>()
        {
            if (Enumeration == null)
            {
                throw new ArgumentNullException(nameof(Enumeration));
            }

            return Enumeration
                    .GetType()
                    .GetTypeInfo()
                    .GetMember(Enumeration.ToString())
                    .FirstOrDefault(member => member.MemberType == MemberTypes.Field)
                    .GetCustomAttributes(typeof(T), false)
                    .Cast<T>()
                    .SingleOrDefault();
        }


        /// <summary>
        /// Get the list of an enum value's attributes.
        /// <remarks>Accept inheritance when looking for the type in the attribute. This is not the case for <see cref="GetAttribute{T}"/></remarks>
        /// </summary>
        /// <typeparam name="T">A class used as an attribute on an enumeration value, can be the parent type</typeparam>
        /// <returns>The value if found or default(TExpected)</returns>
        public virtual IEnumerable<T> GetAttributes<T>()
        {
            if (Enumeration == null)
            {
                throw new ArgumentNullException(nameof(Enumeration));
            }

            return Enumeration
                    .GetType()
                    .GetTypeInfo()
                    .GetMember(Enumeration.ToString())
                    .FirstOrDefault(member => member.MemberType == MemberTypes.Field)
                    .GetCustomAttributes(typeof(T), true)
                    .Cast<T>();
        }

        /// <inheritdoc />
        /// <summary>
        /// Look in the given enum for all the attributes declared of type <typeparamref name="T" />.
        /// The attribute declared at the enum level are overwritten by the attributes at the enum value level
        ///  </summary>
        /// <returns>The final list of all the attributes matching the provided type is returned</returns>
        public virtual List<T> FindAttributes<T>() where T : Attribute
        {
            //getting the list of attribute to try to pre execute
            //first level the enum itself
            var enumLevelAttributes = Enumeration.GetType().GetTypeInfo().GetCustomAttributes<T>()?.ToList();
            //second level, the field itself, it override the enum level attribute
            var enumValueLevelAttributes = GetAttributes<T>()?.ToList();

            //we only assign the enum values attributes at start
            var finalAttributes = enumValueLevelAttributes;
            if (!(finalAttributes?.Any() ?? false))
            {
                //if the list was empty, then we only assign the type attributes
                finalAttributes = enumLevelAttributes;
            }
            else if (enumLevelAttributes?.Any() ?? false)
            {
                //if it was not empty AND we have some type level attribute as well, then we do the union
                //we start with the enumValueLevelAttributes as the base list
                //and we add all the elements in the enum level attributes list which type are NOT in the enumValueLevelAttributes
                finalAttributes.AddRange(
                    enumLevelAttributes.Where(
                        la => enumValueLevelAttributes.All(evl => evl.GetType() != la.GetType())));
            }
            return finalAttributes;
        }
    }
}