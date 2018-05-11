// SOLUTION: Clean
// PROJECT: Clean.Domain
// FILE: Constants.cs
// CREATED: Mike Gardner

// Namespace Domain contains the business domain artifacts.
namespace Clean.Domain
{
    /// <summary>   Constants are business domain values. </summary>
    public static class Constants
    {
        #region Constants

        /// <summary>   InvalidEntityId is used when there is an invalid ID for an entity. </summary>
        public static readonly uint InvalidEntityId = 0;

        /// <summary>   kInvalidTenantId is used when there is an invalid TenantId for an entity. </summary>
        public static readonly uint InvalidTenantId = 0;

        /// <summary>   MaxMakeLength is the maximum length string for a make. </summary>
        public static readonly uint MaxMakeLength = 20;

        /// <summary>   MaxModelLength is the maximum length string for a model. </summary>
        public static readonly uint MaxModelLength = 20;

        /// <summary>   MaxYear is the maximum year. </summary>
        public static readonly uint MaxYear = 2020;

        /// <summary>   MinEntityID is the minimum ID value. </summary>
        public static readonly uint MinEntityId = 1;

        /// <summary>   MinMakeLength is the minimum length string for a make. </summary>
        public static readonly uint MinMakeLength = 1;

        /// <summary>   MinModelLength is the minimum length string for a model. </summary>
        public static readonly uint MinModelLength = 1;

        /// <summary>   MinYear is the minimum year. </summary>
        public static readonly uint MinYear = 1999;

        /// <summary>   VinLength is the length of a VIN. </summary>
        public static readonly uint VinLength = 17;

        #endregion
    }
}