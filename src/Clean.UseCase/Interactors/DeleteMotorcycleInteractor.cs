// SOLUTION: Clean
// PROJECT: Clean.UseCase
// FILE: DeleteMotorcycleInteractor.cs
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

    /// <summary>   A delete motorcycle interactor. This class cannot be inherited. </summary>
    /// <remarks>            
    ///    TITLE
    ///    Delete an existing motorcycle from the motorcycle repository.
    ///
    ///    DESCRIPTION
    ///
    ///    User accesses the system to delete motorcycles.
    ///
    ///    PRIMARY ACTOR
    ///
    ///    User
    ///
    ///    PRECONDITIONS
    ///
    ///    User is logged into system.
    ///    User possesses the necessary security authorizations to delete a motorcycle.
    ///
    ///    A Motorcycle with the ID exists in the repository.
    ///
    ///    The network and configuration is working properly.
    ///
    ///
    ///    POSTCONDITIONS
    ///    User has deleted a motorcycle from the system, unless it didn't exist.
    ///
    ///
    ///    MAIN SUCCESS SCENARIO
    ///            1. User selects "Delete Motorcycle..." from the menu.
    ///            2. System displays a view in which the user selects a motorcycle to delete.
    ///            3. User click the "Submit" button.
    ///            4. System deletes the motorcycle from the motorcycle repository, and displays a confirmation message.
    ///            5. User clicks the "OK" button, and returns to the primary view.
    ///
    ///
    ///    EXTENSIONS
    ///
    ///    (3a) The user cannot log into the system.
    ///           System displays an error message saying that authentication has failed,
    ///           and provides suggestions for resolving the issue.The User clicks the
    ///	               "OK" button, and returns to the login view.
    ///
    ///            (3b) The user does not possess the required authorization to delete a motorcycle.
    ///                   System displays an error message saying that the user does possess the required
    ///
    ///                   security authorizations to delete a motorcycles.  It recommends contacting the
    ///
    ///                   System Administrator.  The User clicks the "OK" button, and returns to the
    ///
    ///                   primary view.
    ///
    ///            (3c) A motorcycle with the ID does not exist in the repository.
    ///                   System displays an error message indicating that a motorcycle with the
    ///
    ///                   ID does not exist.  The User clicks the "OK" button, and
    ///                   returns to the primary view.
    ///
    /// </remarks>
    public sealed class DeleteMotorcycleInteractor
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
        public async Task<(DeleteMotorcycleResponse response, IError error)> HandleAsync(DeleteMotorcycleRequest request)
        {
            // Verify that the user has been properly authenticated.
            if (!_authService.IsAuthenticated())
            {
                return DeleteMotorcycleResponse.NewDeleteMotorcycleResponse(request.Id,
                                                                            OperationStatus.NotAuthenticated,
                                                                            new Error("Delete operation failed due to not being authenticated."));
            }

            // Verify that the user has the necessary authorizations.
            if (!_authService.IsAuthorized(AuthorizationRole.Admin))
            {
                return DeleteMotorcycleResponse.NewDeleteMotorcycleResponse(request.Id,
                                                                            OperationStatus.NotAuthorized,
                                                                            new
                                                                                Error("Delete operation failed due to not being authorized, so please contact your system administrator."));
            }

            // Delete the motorcycle with Id from the repository.
            (OperationStatus status, IError error) = await _motorcycleRepository.DeleteAsync(request.Id);

            if (error != null)
            {
                return DeleteMotorcycleResponse.NewDeleteMotorcycleResponse(request.Id, status, error);
            }

            // Save the changes.
            (status, error) = await _motorcycleRepository.SaveAsync();

            if (error != null)
            {
                return DeleteMotorcycleResponse.NewDeleteMotorcycleResponse(request.Id, status, error);
            }

            // Return the successful response message.
            return DeleteMotorcycleResponse.NewDeleteMotorcycleResponse(request.Id, OperationStatus.Ok, null);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// NewDeleteMotorcycleInteractor creates a new instance of a DeleteMotorcycleInteractor.
        /// </summary>
        ///
        /// <param name="motorcycleRepository"> The motorcycle repository. </param>
        /// <param name="authService">          The authentication service. </param>
        ///
        /// <returns>
        /// Returns  (null, error) when there is an error, otherwise (DeleteMotorcycleInteractor, null).
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static (DeleteMotorcycleInteractor interactor, IError error) NewDeleteMotorcycleInteractor(
            IMotorcycleRepository motorcycleRepository,
            IAuthService authService)
        {
            var interactor = new DeleteMotorcycleInteractor
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