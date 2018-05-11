// SOLUTION: Clean
// PROJECT: Clean.Domain
// FILE: IAuthService.cs
// CREATED: Mike Gardner

// Namespace security contains implementations of interfaces dealing security, authentication, and authorization.
namespace Clean.Domain.Interfaces
{
    using System;
    using Enumerations;
    using Shared.Interfaces;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// IAuthService is a contract that provides authentication and authorization services.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public interface IAuthService : IDisposable
    {
        #region Other Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// IsAuthenticated determines whether the User has been authenticated by the system.
        /// </summary>
        ///
        /// <returns>   Returns true if the User has passed authentication, otherwise false. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        bool IsAuthenticated();

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// IsAuthorized determines whether the User possesses the required authorization role(s).
        /// </summary>
        ///
        /// <param name="role"> The role that needs to be evaluated. </param>
        ///
        /// <returns>
        /// Returns the value associated with the role, which is true or false.  If it is not found false
        /// is returned.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        bool IsAuthorized(AuthorizationRole role);

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Validate verifies that an AuthService's fields contain valid data. </summary>
        ///
        /// <returns>   Returns null if the AuthService contains valid data, otherwise an IError. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        IError Validate();

        #endregion
    }
}