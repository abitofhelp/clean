// SOLUTION: Clean
// PROJECT: Clean.UseCase
// FILE: ListMotorcyclesRequest.cs
// CREATED: Mike Gardner

// Namespace Requests contains the requests for the use cases.
namespace Clean.UseCase.Requests
{
    using Shared.Interfaces;

    /// <summary>   A list motorcycles request. </summary>
    public sealed class ListMotorcyclesRequest
    {
        #region Properties
        #endregion

        #region Other Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// NewListMotorcyclesRequest creates a new instance of a ListMotorcyclesRequest.
        /// </summary>
        ///
        /// <returns>
        /// Returns (null, IError) when there is an error, otherwise (ListMotorcyclesRequest, null)
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static (ListMotorcyclesRequest request, IError error) NewListMotorcyclesRequest()
        {
            var request = new ListMotorcyclesRequest();

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
            return null;
        }

        #endregion
    }
}