// SOLUTION: Clean
// PROJECT: Clean.Shared
// FILE: EnumExtensions.cs
// CREATED: Mike Gardner

namespace Clean.Shared.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

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
        public static string GetDescription<T>(this T anEnum) where T : IConvertible
        {
            string description = null;

            if (anEnum is Enum)
            {
                Type type = anEnum.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == anEnum.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (descriptionAttributes.Length > 0)
                        {
                            // we're only getting the first description we find
                            // others will be ignored
                            description = ((DescriptionAttribute)descriptionAttributes[0]).Description;
                        }

                        break;
                    }
                }
            }

            return description;
        }

        #endregion
    }
}