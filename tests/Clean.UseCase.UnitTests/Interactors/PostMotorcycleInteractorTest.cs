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
    /// PostMotorcycleInteractorTest implementes tests for the PostMotorcycleInteractor. This class cannot be inherited.
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public sealed class PostMotorcycleInteractorTest : IDisposable
    {
        private readonly IAuthService _authService;
        private readonly IMotorcycleRepository _motorcycleRepository;

        #region Setup/Teardown

        public PostMotorcycleInteractorTest(IAuthService authService, IMotorcycleRepository motorcycleRepository)
        {
            _authService = authService;
            _motorcycleRepository = motorcycleRepository;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Dispose()
        {
            // Configure common test teardown, here...
            _authService?.Dispose();
            _motorcycleRepository?.Dispose();
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
            (_, IError error) = PostMotorcycleInteractor.NewPostMotorcycleInteractor(null, authServiceMock.Object);


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
            (_, IError error) = PostMotorcycleInteractor.NewPostMotorcycleInteractor(repositoryMock.Object, null);


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
            (PostMotorcycleRequest request, _) = PostMotorcycleRequest.NewPostMotorcycleRequest("Honda", "Shadow", 2006, "01234567890123456");

            (PostMotorcycleInteractor interactor, _) = PostMotorcycleInteractor.NewPostMotorcycleInteractor(repositoryMock.Object, authServiceMock.Object);

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
            var (request, _) = PostMotorcycleRequest.NewPostMotorcycleRequest("Honda", "Shadow", 2006, "01234567890123456");

            (PostMotorcycleInteractor interactor, _) = PostMotorcycleInteractor.NewPostMotorcycleInteractor(repositoryMock.Object, authServiceMock.Object);

            // ACT
            (_, IError error) = await interactor.HandleAsync(request);

            // ASSERT
            Assert.NotNull(error);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestInteractor_Post creates a motorcycle in the repository.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public async Task TestInteractor_Insert()
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

            var tcsExists = new TaskCompletionSource<(bool, OperationStatus, IError)>();
            tcsExists.SetResult((false, OperationStatus.NotFound, null));
            repositoryMock.Setup(r => r.ExistsByIdAsync(It.IsAny<long>())).Returns(tcsExists.Task);
            
            var tcsInsert = new TaskCompletionSource<(Motorcycle, OperationStatus, IError)>();
            tcsInsert.SetResult((motorcycle, OperationStatus.Ok, null));
            repositoryMock.Setup(r => r.InsertAsync(It.IsAny<Motorcycle>(), It.IsAny<Func<Motorcycle, Task<(bool exists, OperationStatus status, IError error)>>>())).Returns(tcsInsert.Task);
            
            // Create the request to add a new motorcycle to the repository.
            var (request, _) = PostMotorcycleRequest.NewPostMotorcycleRequest("Honda", "Shadow", 2006, "01234567890123456");

            // Create the interactor that will coordinate adding a new motorcycle to the repository.
            (PostMotorcycleInteractor interactor, _) = PostMotorcycleInteractor.NewPostMotorcycleInteractor(repositoryMock.Object, authServiceMock.Object);
            
            // ACT
            (PostMotorcycleResponse response, _) = await interactor.HandleAsync(request);

            // ASSERT
            Assert.True(response.Id > Domain.Constants.InvalidEntityId);
            Assert.Null(response.Error);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TestInteractor_NotExist attempts to gets a motorcycle from the repository that does not exist.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public async Task TestInteractor_VinAlreadyExists()
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

            var tcsExists = new TaskCompletionSource<(bool, OperationStatus, IError)>();
            tcsExists.SetResult((false, OperationStatus.NotFound, null));
            repositoryMock.Setup(r => r.ExistsByIdAsync(It.IsAny<long>())).Returns(tcsExists.Task);

            var tcsInsert = new TaskCompletionSource<(Motorcycle, OperationStatus, IError)>();
            tcsInsert.SetResult((motorcycle, OperationStatus.Ok, null));
            repositoryMock.Setup(r => r.InsertAsync(It.IsAny<Motorcycle>(), It.IsAny<Func<Motorcycle, Task<(bool exists, OperationStatus status, IError error)>>>())).Returns(tcsInsert.Task);

            // Create the request to add a new motorcycle to the repository.
            var (request, _) = PostMotorcycleRequest.NewPostMotorcycleRequest("Honda", "Shadow", 2006, "01234567890123456");

            // Create the interactor that will coordinate adding a new motorcycle to the repository.
            (PostMotorcycleInteractor interactor, _) = PostMotorcycleInteractor.NewPostMotorcycleInteractor(repositoryMock.Object, authServiceMock.Object);

            // ACT
            (PostMotorcycleResponse response, _) = await interactor.HandleAsync(request);

            // ASSERT
            Assert.True(response.Id > Domain.Constants.InvalidEntityId);
            Assert.Null(response.Error);
        }

    }
}