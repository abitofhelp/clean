// SOLUTION: Clean
// PROJECT: Clean.Shared
// FILE: EnumExtensions.cs
// CREATED: Mike Gardner

namespace Clean.Shared.Extensions
{
    using System;
    using System.ComponentModel;

    /// <summary>   An enum extensions. </summary>
    public static class EnumExtensions
    {
        #region Other Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   An Enum extension method that gets a description attribute from the enum. </summary>
        ///
        /// <remarks>
        /// To get the description for a particular enumeration: i.e.
        /// AuthorizationRole.Accounting.GetDescription()
        /// </remarks>
        ///
        /// <param name="genericEnum">  The GenericEnum to act on. </param>
        ///
        /// <returns>   The description. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static string GetDescription(this Enum genericEnum)
        {
            var genericEnumType = genericEnum.GetType();
            var memberInfo = genericEnumType.GetMember(genericEnum.ToString());

            if ((memberInfo != null && memberInfo.Length > 0))
            {
                dynamic attribs = memberInfo[0]
                    .GetCustomAttributes(typeof(DescriptionAttribute), false);
                if ((attribs != null && attribs.Length > 0))
                {
                    return ((DescriptionAttribute) attribs[0]).Description;
                }
            }

            return genericEnum.ToString();
        }

        #endregion
    }
}