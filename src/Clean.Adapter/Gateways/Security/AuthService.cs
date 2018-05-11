// SOLUTION: Clean
// PROJECT: Clean.Adapter
// FILE: AuthService.cs
// CREATED: Mike Gardner

// Namespace security contains implementations of interfaces dealing security, authentication, and authorization.
namespace Clean.Adapter.Gateways.Security
{
    using System.Collections.Generic;
    using Domain.Enumerations;
    using Domain.Interfaces;
    using Shared;
    using Shared.Interfaces;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// AuthService provides authentication and authorization services.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public sealed class AuthService : IAuthService
    {
        #region Fields

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// _isAuthenticated indicates whether the user has successfully logged into the system.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private bool _isAuthenticated;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// _roles is a look-up to determine whether a user has been granted a role. Each role has a
        /// boolean value indicating the user's access to each role.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private Dictionary<AuthorizationRole, bool> _roles;

        #endregion

        #region IAuthService Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// IsAuthenticated determines whether the User has been authenticated by the system.
        /// </summary>
        ///
        /// <returns>   Returns true if the User has passed authentication, otherwise false. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool IsAuthenticated() => _isAuthenticated;

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
        public bool IsAuthorized(AuthorizationRole role) => _roles.ContainsKey(role) && _roles[role];

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Validate verifies that an AuthService's fields contain valid data. </summary>
        ///
        /// <returns>   Returns null if the AuthService contains valid data, otherwise an IError. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IError Validate()
        {
            return _roles == null
                ? new Error("The roles and access permissions cannot be null.")
                : null;
        }

        #region Implementation of IDisposable

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Dispose()
        {
            _roles = null;
        }

        #endregion

        #endregion

        #region Other Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// NewAuthService creates a new instance of an AuthService including validation.
        /// </summary>
        ///
        /// <param name="isAuthenticated">  True if the user has been successfully authenticated. </param>
        /// <param name="roles">            It is a look-up to determine whether a user has been granted
        ///                                 a role. </param>
        ///
        /// <returns>
        /// Returns (null, IError) when there is an error, otherwise (authService, null).
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static (IAuthService authService, IError error) NewAuthService(bool isAuthenticated, Dictionary<AuthorizationRole, bool> roles)
        {
            var authService = new AuthService
            {
                _isAuthenticated = isAuthenticated,
                _roles = roles
            };

            var err = authService.Validate();
            if (err != null)
            {
                return (null, err);
            }

            // All okay
            return (authService, null);
        }

        #endregion
    }
}