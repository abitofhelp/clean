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
    /// DeleteMotorcycleInteractorTest implementes tests for the DeleteMotorcycleInteractor. This class cannot be inherited.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public sealed class DeleteMotorcycleInteractorTest : IDisposable
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
            (_, IError error) = DeleteMotorcycleInteractor.NewDeleteMotorcycleInteractor(null, authServiceMock.Object);


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
            (_, IError error) = DeleteMotorcycleInteractor.NewDeleteMotorcycleInteractor(repositoryMock.Object, null);


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
            var (request, _) = DeleteMotorcycleRequest.NewDeleteMotorcycleRequest(123);

            (DeleteMotorcycleInteractor interactor, _) = DeleteMotorcycleInteractor.NewDeleteMotorcycleInteractor(repositoryMock.Object, authServiceMock.Object);

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
            var (request, _) = DeleteMotorcycleRequest.NewDeleteMotorcycleRequest(123);

            (DeleteMotorcycleInteractor interactor, _) = DeleteMotorcycleInteractor.NewDeleteMotorcycleInteractor(repositoryMock.Object, authServiceMock.Object);

            // ACT
            (_, IError error) = await interactor.HandleAsync(request);

            // ASSERT
            Assert.NotNull(error);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestInteractor_Delete deletes a motorcycle from the repository.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public async Task TestInteractor_Delete()
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

            //    Repository Delete Mocking
            var deleteTcs = new TaskCompletionSource<(OperationStatus, IError)>();
            deleteTcs.SetResult((OperationStatus.Ok, null));
            repositoryMock.Setup(d => d.DeleteAsync(It.IsAny<long>()))
                          .Returns(deleteTcs.Task);
            var (request, _) = DeleteMotorcycleRequest.NewDeleteMotorcycleRequest(motorcycle.Id);

            (DeleteMotorcycleInteractor interactor, _) = DeleteMotorcycleInteractor.NewDeleteMotorcycleInteractor(repositoryMock.Object, authServiceMock.Object);

            // ACT
            (DeleteMotorcycleResponse response, _) = await interactor.HandleAsync(request);

            // ASSERT
            Assert.True(response.Id == motorcycle.Id);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestInteractor_NotExist attempts to delete a motorcycle from the repository that does not exist.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public async Task TestInteractor_NotExist()
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

            //    Repository Delete Mocking
            var deleteTcs = new TaskCompletionSource<(OperationStatus, IError)>();
            deleteTcs.SetResult((OperationStatus.NotFound, new Error("Does not exist.")));
            repositoryMock.Setup(d => d.DeleteAsync(It.IsAny<long>()))
                          .Returns(deleteTcs.Task);
            var (request, _) = DeleteMotorcycleRequest.NewDeleteMotorcycleRequest(123);

            (DeleteMotorcycleInteractor interactor, _) = DeleteMotorcycleInteractor.NewDeleteMotorcycleInteractor(repositoryMock.Object, authServiceMock.Object);

            // ACT
            (_, IError error) = await interactor.HandleAsync(request);

            // ASSERT
            Assert.NotNull(error);
        }

    }
}


/*


// For inserting testing...
//            // var insertTcs = new TaskCompletionSource<(Motorcycle, IError)>();
//       (Motorcycle motorcycle, IError error) = Motorcycle.NewMotorcycle("Honda", "Shadow", 2006, "01234567890123456");
//        motorcycle.Id = 1;
//      //  insertTcs.SetResult((motorcycle.motorcycle, null));
//            var insertRequest = repositoryMock.Setup(d => d.InsertAsync(It.IsAny<Motorcycle>(), (m => false)))
//                                            .Returns(insertTcs.Task);



 */
