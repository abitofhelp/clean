// SOLUTION: Clean
// PROJECT: Clean.UseCase
// FILE: GetMotorcycleRequest.cs
// CREATED: Mike Gardner

// Namespace Requests contains the requests for the use cases.
namespace Clean.UseCase.Requests
{
    using Shared;
    using Shared.Interfaces;

    /// <summary>   A get motorcycle request. </summary>
    public sealed class GetMotorcycleRequest
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
        /// NewGetMotorcycleRequest creates a new instance of a GetMotorcycleRequest.
        /// </summary>
        ///
        /// <param name="id">   The identifier. </param>
        ///
        /// <returns>
        /// Returns (null, IError) when there is an error, otherwise (GetMotorcycleRequest, null)
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static (GetMotorcycleRequest request, IError error) NewGetMotorcycleRequest(long id)
        {
            var request = new GetMotorcycleRequest
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