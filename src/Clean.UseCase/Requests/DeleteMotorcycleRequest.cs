// SOLUTION: Clean
// PROJECT: Clean.UseCase
// FILE: DeleteMotorcycleRequest.cs
// CREATED: Mike Gardner

// Namespace Requests contains the requests for the use cases.
namespace Clean.UseCase.Requests
{
    using Shared;
    using Shared.Interfaces;

    /// <summary>   A delete motorcycle request. </summary>
    public sealed class DeleteMotorcycleRequest
    {
        #region Properties

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the identifier. </summary>
        ///
        /// <value> The identifier. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public long Id { get; private set; }

        #endregion

        #region Other Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// NewDeleteMotorcycleRequest creates a new instance of a DeleteMotorcycleRequest.
        /// </summary>
        ///
        /// <param name="id">   The identifier. </param>
        ///
        /// <returns>
        /// Returns (null, IError) when there is an error, otherwise (DeleteMotorcycleRequest, null)
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static (DeleteMotorcycleRequest request, IError error) NewDeleteMotorcycleRequest(long id)
        {
            var request = new DeleteMotorcycleRequest
            {
                Id = id
            };

            var error = request.Validate();
            if (error != null)
            {
                return (null, error);
            }

            // All okay
            return (request, null);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Validate verifies that all of the fields in this instance meet the requirements.
        /// </summary>
        ///
        /// <returns>   Null on success, otherwise an Error. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IError Validate()
        {
            return Id <= 0
                ? new Error("The id cannot be zero or a negative number.")
                : null;
        }

        #endregion
    }
}