// SOLUTION: Clean
// PROJECT: Clean.UseCase
// FILE: PutMotorcycleRequest.cs
// CREATED: Mike Gardner

// Namespace Requests contains the requests for the use cases.
namespace Clean.UseCase.Requests
{
    using Domain.Entities;
    using Shared;
    using Shared.Interfaces;

    /// <summary>   A put motorcycle request. </summary>
    public sealed class PutMotorcycleRequest
    {
        #region Properties

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the identifier. </summary>
        ///
        /// <value> The identifier. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public long Id { get; private set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the motorcycle instance. </summary>
        ///
        /// <value> The modified motorcycle </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public Motorcycle Motorcycle { get; private set; }

        #endregion

        #region Other Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// NewPutMotorcycleRequest creates a new instance of a PutMotorcycleRequest.
        /// </summary>
        ///
        /// <param name="id">   The identifier. </param>
        /// <param name="motorcycle"> An instance of a motorcycle that has been modified.</param>
        ///
        /// <returns>
        /// Returns (null, IError) when there is an error, otherwise (PutMotorcycleRequest, null)
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static (PutMotorcycleRequest request, IError error) NewPutMotorcycleRequest(long id, Motorcycle motorcycle)
        {
            var request = new PutMotorcycleRequest
            {
                Id = id,
                Motorcycle = motorcycle
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
            return Motorcycle.Validate();
        }

        #endregion
    }
}