namespace Clean.UseCase.Interactors.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Domain.Enumerations;
    using Domain.Interfaces;
    using Moq;
    using Requests;
    using Responses;
    using Shared;
    using Shared.Enumerations;
    using Shared.Interfaces;
    using Xunit;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// ListMotorcycleInteractorTest implementes tests for the ListMotorcycleInteractor. This class cannot be inherited.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public sealed class ListMotorcyclesInteractorTest : IDisposable
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
        /// TestInteractor__MotorcycleRepositoryIsNull  verifies that a nil motorcycle repository fails properly.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public void TestInteractor__MotorcycleRepositoryIsNull()
        {
            // ARRANGE
            var authServiceMock = new Mock<IAuthService>();

            // ACT
            (_, IError error) = ListMotorcyclesInteractor.NewListMotorcycleInteractor(null, authServiceMock.Object);


            // ASSERT
            Assert.NotNull(error);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestInteractor_AuthServiceIsNull verifies that a nil authorization service fails properly.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public void TestInteractor_AuthServiceIsNull()
        {
            // ARRANGE
            //            var roles = new Dictionary<AuthorizationRole, bool>
            //            {
            //                {
            //                    AuthorizationRole.None, true
            //                }
            //            };

            var repositoryMock = new Mock<IMotorcycleRepository>();

            // ACT
            (_, IError error) = ListMotorcyclesInteractor.NewListMotorcycleInteractor(repositoryMock.Object, null);


            // ASSERT
            Assert.NotNull(error);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestInteractor_NotAuthenticated verifies that a non-authenticated user fails properly.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public async Task TestInteractor_NotAuthenticated()
        {
            // ARRANGE
            var authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(d => d.IsAuthenticated()).Returns(false);
            var repositoryMock = new Mock<IMotorcycleRepository>();
            var (request, _) = ListMotorcyclesRequest.NewListMotorcyclesRequest();

            (ListMotorcyclesInteractor interactor, _) = ListMotorcyclesInteractor.NewListMotorcycleInteractor(repositoryMock.Object, authServiceMock.Object);

            // ACT
            (_, IError error) = await interactor.HandleAsync(request);

            // ASSERT
            Assert.NotNull(error);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestInteractor_NotAuthorized verifies that an authenticated user lacking an authorization role cannot insert a motorcycle.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public async Task TestInteractor_NotAuthorized()
        {
            // ARRANGE
            var authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(d => d.IsAuthenticated())
                           .Returns(true);
            authServiceMock.Setup(d => d.IsAuthorized(AuthorizationRole.Undefined))
                           .Returns(false);
            var repositoryMock = new Mock<IMotorcycleRepository>();
            var (request, _) = ListMotorcyclesRequest.NewListMotorcyclesRequest();

            (ListMotorcyclesInteractor interactor, _) = ListMotorcyclesInteractor.NewListMotorcycleInteractor(repositoryMock.Object, authServiceMock.Object);

            // ACT
            (_, IError error) = await interactor.HandleAsync(request);

            // ASSERT
            Assert.NotNull(error);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestInteractor_EmptyList gets an empty list of motorcycles from the repository.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public async Task TestInteractor_EmptyList()
        {
            // ARRANGE
            //   Authorization Service Mocking
            var authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(d => d.IsAuthenticated())
                           .Returns(true);
            authServiceMock.Setup(d => d.IsAuthorized(AuthorizationRole.Admin))
                           .Returns(true);

            //   Repository Mocking
            var repositoryMock = new Mock<IMotorcycleRepository>();

            //    Repository List Mocking
            var tcs = new TaskCompletionSource<(IReadOnlyCollection<Motorcycle>, OperationStatus, IError)>();
            tcs.SetResult((new List<Motorcycle>().AsReadOnly(), OperationStatus.Ok, null));
            repositoryMock.Setup(d => d.ListAsync())
                          .Returns(tcs.Task);

            var (request, _) = ListMotorcyclesRequest.NewListMotorcyclesRequest();

            (ListMotorcyclesInteractor interactor, _) = ListMotorcyclesInteractor.NewListMotorcycleInteractor(repositoryMock.Object, authServiceMock.Object);

            // ACT
            (ListMotorcyclesResponse response, _) = await interactor.HandleAsync(request);

            // ASSERT
            Assert.True(response.Motorcycles.Count == 0);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestInteractor_NotEmptyList gets a non-empty list of motorcycles from the repository.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public async Task TestInteractor_NotEmptyList()
        {
            // ARRANGE
            //   Authorization Service Mocking
            var authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(d => d.IsAuthenticated())
                           .Returns(true);
            authServiceMock.Setup(d => d.IsAuthorized(AuthorizationRole.Admin))
                           .Returns(true);

            //   Repository Mocking
            (Motorcycle motorcycle, _) = Motorcycle.NewMotorcycle("Honda", "Shadow", 2006, "01234567890123456");
            motorcycle.Id = 1;
            var repositoryMock = new Mock<IMotorcycleRepository>();
            var motorcycles = new List<Motorcycle> { motorcycle };

            //    Repository List Mocking
            var tcs = new TaskCompletionSource<(IReadOnlyCollection<Motorcycle>, OperationStatus, IError)>();
            tcs.SetResult((motorcycles, OperationStatus.Ok, null));
            repositoryMock.Setup(d => d.ListAsync())
                          .Returns(tcs.Task);
            var (request, _) = ListMotorcyclesRequest.NewListMotorcyclesRequest();

            (ListMotorcyclesInteractor interactor, _) = ListMotorcyclesInteractor.NewListMotorcycleInteractor(repositoryMock.Object, authServiceMock.Object);

            // ACT
            (ListMotorcyclesResponse response, _) = await interactor.HandleAsync(request);

            // ASSERT
            Assert.True(response.Motorcycles.Count == 1);
        }
    }
}