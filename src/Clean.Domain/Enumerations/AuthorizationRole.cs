// SOLUTION: Clean
// PROJECT: Clean.Domain
// FILE: AuthorizationRole.cs
// CREATED: Mike Gardner

// Namespace Enumerations contains the domain enumerations.
namespace Clean.Domain.Enumerations
{
    using System.ComponentModel;

    // To get the description for a particular enumeration: i.e. AuthorizationRole.Accounting.GetDescription()

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// AuthorizationRole is an authorization given to an authenticated user to access a resource.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public enum AuthorizationRole
    {
        /// UndefinedRole is when an authorization role has not been assigned.
        [Description("Undefined")]
        Undefined = 0,

        /// None is when the user does not require any authorization roles to access resources.
        [Description("None")]
        None,

        /// Admin is a user with authorization to access administrative resources.
        [Description("Admin")]
        Admin,

        /// Accounting is a user with authorization to access accounting resources.
        [Description("Accounting")]
        Accounting,

        /// General is a user with administrative authorization to access general resources.
        [Description("General")]
        General
    }
}