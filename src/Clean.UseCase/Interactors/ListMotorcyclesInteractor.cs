// SOLUTION: Clean
// PROJECT: Clean.UseCase
// FILE: ListMotorcycleInteractor.cs
// CREATED: Mike Gardner

// Namespace Interactors contains use cases, which contain the application specific business rules.
// Interactors encapsulate and implement all of the use cases of the system.  They orchestrate the
// flow of data to and from the entity, and can rely on their business rules to achieve the goals
// of the use case.  They do not have any dependencies, and are totally isolated from things like
// a database, UI or special frameworks, which exist in the outer rings.  They Will almost certainly
// require refactoring if details of the use case requirements change.
namespace Clean.UseCase.Interactors
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Domain.Enumerations;
    using Domain.Interfaces;
    using Requests;
    using Responses;
    using Shared;
    using Shared.Enumerations;
    using Shared.Interfaces;

    /// <summary>   A list motorcycle interactor. This class cannot be inherited. </summary>
    /// <remarks> 
    ///    TITLE
    ///    Get an unsorted list of motorcycles from the motorcycle repository.
    ///
    ///    DESCRIPTION
    ///    User accesses the system to get a list of motorcycles.
    ///
    ///
    ///    PRIMARY ACTOR
    ///    User
    ///
    ///    PRECONDITIONS
    ///    User is logged into system.
    ///    User possesses the necessary security authorizations to insert a motorcycle.
    /// The network and configuration is working properly.
    ///
    ///
    ///    POSTCONDITIONS
    ///    User has received a list of motorcycles from the system, and the list can be empty.
    ///
    ///
    ///    MAIN SUCCESS SCENARIO
    ///    1. User selects "Get Motorcycles" from the menu.
    ///    2. System displays a view showing the unsorted list of motorcycles.
    /// 3. User clicks the "OK" button, and returns to the primary view.
    ///
    ///
    ///    EXTENSIONS
    ///    (3a) The user cannot log into the system.
    ///           System displays an error message saying that authentication has failed,
    ///           and provides suggestions for resolving the issue.The User clicks the
    ///	   "OK" button, and returns to the login view.
    ///
    /// (3b) The user does not possess the required authorization to get a list of motorcycles.
    ///       System displays an error message saying that the user does possess the required
    ///
    ///       security authorizations.  It recommends contacting the
    ///
    ///       System Administrator.  The User clicks the "OK" button, and returns to the
    ///
    ///       primary view.
    /// </remarks>
    public sealed class ListMotorcyclesInteractor
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
        public async Task<(ListMotorcyclesResponse response, IError error)> HandleAsync(ListMotorcyclesRequest request)
        {
            // Verify that the user has been properly authenticated.
            if (!_authService.IsAuthenticated())
            {
                return ListMotorcyclesResponse.NewListMotorcyclesResponse(null,
                                                                            OperationStatus.NotAuthenticated,
                                                                            new Error("List operation failed due to not being authenticated."));
            }

            // Verify that the user has the necessary authorizations.
            if (!_authService.IsAuthorized(AuthorizationRole.Admin))
            {
                return ListMotorcyclesResponse.NewListMotorcyclesResponse(null,
                                                                            OperationStatus.NotAuthorized,
                                                                            new
                                                                                Error("List operation failed due to not being authorized, so please contact your system administrator."));
            }

            // List the motorcycle with Id from the repository.
            (IReadOnlyCollection<Motorcycle> motorcycles, OperationStatus status, IError error) = await _motorcycleRepository.ListAsync();

            if (error != null)
            {
                return ListMotorcyclesResponse.NewListMotorcyclesResponse(null, status, error);
            }

            // Save the changes.
            (status, error) = await _motorcycleRepository.SaveAsync();

            if (error != null)
            {
                return ListMotorcyclesResponse.NewListMotorcyclesResponse(null, status, error);
            }

            // Return the successful response message.
            return ListMotorcyclesResponse.NewListMotorcyclesResponse(motorcycles, OperationStatus.Ok, null);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// NewListMotorcycleInteractor creates a new instance of a ListMotorcycleInteractor.
        /// </summary>
        ///
        /// <param name="motorcycleRepository"> The motorcycle repository. </param>
        /// <param name="authService">          The authentication service. </param>
        ///
        /// <returns>
        /// Returns  (null, error) when there is an error, otherwise (ListMotorcycleInteractor, null).
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static (ListMotorcyclesInteractor interactor, IError error) NewListMotorcycleInteractor(
            IMotorcycleRepository motorcycleRepository,
            IAuthService authService)
        {
            var interactor = new ListMotorcyclesInteractor
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