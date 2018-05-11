// SOLUTION: Clean
// PROJECT: Clean.UseCase
// FILE: PostMotorcycleInteractor.cs
// CREATED: Mike Gardner

// Namespace Interactors contains use cases, which contain the application specific business rules.
// Interactors encapsulate and implement all of the use cases of the system.  They orchestrate the
// flow of data to and from the entity, and can rely on their business rules to achieve the goals
// of the use case.  They do not have any dependencies, and are totally isolated from things like
// a database, UI or special frameworks, which exist in the outer rings.  They Will almost certainly
// require refactoring if details of the use case requirements change.
namespace Clean.UseCase.Interactors
{
    using System.Threading.Tasks;
    using Domain.Entities;
    using Domain.Enumerations;
    using Domain.Interfaces;
    using Requests;
    using Responses;
    using Shared;
    using Shared.Enumerations;
    using Shared.Interfaces;

    /// <summary>   A create motorcycle interactor. This class cannot be inherited. </summary>
    /// <remarks> 
    ///    TITLE
    ///    Insert a new motorcycle make, model, and year into the motorcycle repository.
    ///
    ///    DESCRIPTION
    ///    User accesses the system to add a new motorcycle make, model, and year to it.
    ///
    ///    PRIMARY ACTOR
    /// User
    ///
    /// PRECONDITIONS
    /// User is logged into system.
    /// User possesses the necessary security authorizations to insert a motorcycle.
    /// A motorcycle of the same make, model, and year does not already exist.
    /// The network and configuration is working properly.
    ///
    /// POSTCONDITIONS
    /// User has inserted a new motorcycle make, model, and year into the system,
    /// unless it already exists.
    ///
    /// MAIN SUCCESS SCENARIO
    /// 1. User selects "Add Motorcycle..." from the menu.
    /// 2. System displays a view in which the user enters the make, model, and year of the motorcycle.
    /// 3. User click the "Submit" button.
    /// 4. System inserts the motorcycle into the motorcycle repository, and displays a confirmation message.
    /// 5. User clicks the "OK" button, and returns to the primary view.
    ///
    /// EXTENSIONS
    /// (3a) The user cannot log into the system.
    ///       System displays an error message saying that authentication has failed,
    ///       and provides suggestions for resolving the issue.The User clicks the
    ///	   "OK" button, and returns to the login view.
    ///
    /// (3b) The user does not possess the required authorization to insert a motorcycle.
    ///       System displays an error message saying that the user does possess the required
    ///
    ///       security authorizations to insert a motorcycle.  It recommends contacting the
    ///
    ///       System Administrator.  The User clicks the "OK" button, and returns to the
    ///
    ///       primary view.
    ///
    /// (3c) A motorcycle with the same make, model, and year already exists.
    ///       System displays an error message indicating that a motorcycle with the same
    ///
    ///       make, model, and year already exists.The User clicks the "OK" button, and
    ///       returns to the primary view.
    /// </remarks>
    public sealed class PostMotorcycleInteractor
    {
        #region Properties

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the authentication service. </summary>
        ///
        /// <value> The authentication service. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private IAuthService _authService { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the motorcycle repository. </summary>
        ///
        /// <value> The motorcycle repository. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private IMotorcycleRepository _motorcycleRepository { get; set; }

        #endregion

        #region Other Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Handle processes the request message and generates the response message.  It is performing
        /// the use case. The request message is a dto containing the required data for completing the
        /// use case.
        /// </summary>
        ///
        /// <param name="request">  The request. </param>
        ///
        /// <returns>
        /// On success, the method returns the (response message, nil), otherwise (nil, error).
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<(PostMotorcycleResponse response, IError error)> HandleAsync(PostMotorcycleRequest request)
        {
            // Verify that the user has been properly authenticated.
            if (!_authService.IsAuthenticated())
            {
                return PostMotorcycleResponse.NewPostMotorcycleResponse(Domain.Constants.InvalidEntityId,
                                                                            OperationStatus.NotAuthenticated,
                                                                            new Error("Post operation failed due to not being authenticated."));
            }

            // Verify that the user has the necessary authorizations.
            if (!_authService.IsAuthorized(AuthorizationRole.Admin))
            {
                return PostMotorcycleResponse.NewPostMotorcycleResponse(Domain.Constants.InvalidEntityId,
                                                                            OperationStatus.NotAuthorized,
                                                                            new
                                                                                Error("Post operation failed due to not being authorized, so please contact your system administrator."));
            }

            // Create the new motorcycle instance.
            (Motorcycle motorcycle, IError error) = Motorcycle.NewMotorcycle(request.Make, request.Model, request.Year, request.Vin);
            if (error != null)
            {
                return PostMotorcycleResponse.NewPostMotorcycleResponse(Domain.Constants.InvalidEntityId, OperationStatus.InternalError, error);
            }

            OperationStatus status = OperationStatus.InternalError;

            // Post the motorcycle with Id from the repository.
            (motorcycle, status, error) = await _motorcycleRepository.InsertAsync(motorcycle,
                                                                   (moto) =>
                                                                   {
                                                                       return _motorcycleRepository.ExistsByIdAsync(moto.Id);
                                                                   });

            if (error != null)
            {
                return PostMotorcycleResponse.NewPostMotorcycleResponse(Domain.Constants.InvalidEntityId, status, error);
            }

            // Save the changes.
            (status, error) = await _motorcycleRepository.SaveAsync();

            if (error != null)
            {
                return PostMotorcycleResponse.NewPostMotorcycleResponse(Domain.Constants.InvalidEntityId, status, error);
            }

            // Return the successful response message.
            return PostMotorcycleResponse.NewPostMotorcycleResponse(motorcycle.Id, OperationStatus.Ok, null);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// NewPostMotorcycleInteractor creates a new instance of a PostMotorcycleInteractor.
        /// </summary>
        ///
        /// <param name="motorcycleRepository"> The motorcycle repository. </param>
        /// <param name="authService">          The authentication service. </param>
        ///
        /// <returns>
        /// Returns  (null, error) when there is an error, otherwise (PostMotorcycleInteractor, null).
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static (PostMotorcycleInteractor interactor, IError error) NewPostMotorcycleInteractor(
            IMotorcycleRepository motorcycleRepository,
            IAuthService authService)
        {
            var interactor = new PostMotorcycleInteractor
            {
                _motorcycleRepository = motorcycleRepository,
                _authService = authService
            };

            // Validate the interactor
            var err = interactor.Validate();

            if (err != null)
            {
                return (null, err);
            }

            // All okay
            return (interactor, null);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Validate verifies that all of the fields in a interactor instance meet the requirements.
        /// </summary>
        ///
        /// <returns>   Null on success, otherwise an Error. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IError Validate()
        {
            var error = new Error();

            if (_motorcycleRepository == null) error.Add("The motorcycle repository cannot be null.");

            if (_authService == null) error.Add("The authorization service cannot be null.");

            return error.Messages.Count > 0
                ? error
                : null;
        }

        #endregion
    }
}