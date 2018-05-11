// SOLUTION: Clean
// PROJECT: Clean.Adapter
// FILE: AuthServiceTest.cs
// CREATED: Mike Gardner

namespace Clean.Adapter.Gateways.Security.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Domain.Enumerations;
    using Domain.Interfaces;
    using Xunit;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// AuthServiceTest implementes tests for the AuthService. This class cannot be inherited.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public sealed class AuthServiceTest : IDisposable
    {
        #region Setup/Teardown

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Dispose()
        {
            // Configure common test teardown, here...
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestAuthService_DoesNotHaveRole verifies that a user is not authorized for a specific role.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static void TestAuthService_DoesNotHaveRole()
        {
            // ARRANGE
            var roles = new Dictionary<AuthorizationRole, bool>
            {
                {
                    AuthorizationRole.None, true
                }
            };

            // ACT
            (IAuthService authService, _) = AuthService.NewAuthService(true, roles);

            // ASSERT
            Assert.False(authService.IsAuthorized(AuthorizationRole.Admin));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestAuthService_DoesNotHaveRole verifies that a user is not authorized for a specific role.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static void TestAuthService_DoesNotHaveRoleInMap()
        {
            // ARRANGE
            var roles = new Dictionary<AuthorizationRole, bool>();

            // ACT
            (IAuthService authService, _) = AuthService.NewAuthService(true, roles);

            // ASSERT
            Assert.False(authService.IsAuthorized(AuthorizationRole.Admin));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestAuthService_HasARole verifies that a user is authorized for a specific role.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static void TestAuthService_HasARole()
        {
            // ARRANGE
            var roles = new Dictionary<AuthorizationRole, bool>
            {
                {
                    AuthorizationRole.Admin, true
                }
            };

            // ACT
            (IAuthService authService, _) = AuthService.NewAuthService(true, roles);

            // ASSERT
            Assert.True(authService.IsAuthorized(AuthorizationRole.Admin));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// AuthService_IsAuthenticated verifies an authenticated user has been detected.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static void TestAuthService_IsAuthenticated()
        {
            // ARRANGE
            var roles = new Dictionary<AuthorizationRole, bool>
            {
                {
                    AuthorizationRole.Admin, true
                }
            };

            // ACT
            (IAuthService authService, _) = AuthService.NewAuthService(true, roles);

            // ASSERT
            Assert.True(authService.IsAuthenticated());
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestAuthService_IsNotAuthenticated verifies a user who has not been authenticated is detected.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public static void TestAuthService_IsNotAuthenticated()
        {
            // ARRANGE
            var roles = new Dictionary<AuthorizationRole, bool>
            {
                {
                    AuthorizationRole.None, true
                }
            };

            // ACT
            (IAuthService authService, _) = AuthService.NewAuthService(false, roles);

            // ASSERT
            Assert.False(authService.IsAuthenticated());
        }
    }
}