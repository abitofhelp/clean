// SOLUTION: Clean
// PROJECT: Clean.UseCase
// FILE: PostMotorcycleRequest.cs
// CREATED: Mike Gardner

// Namespace Requests contains the requests for the use cases.
namespace Clean.UseCase.Requests
{
    using Domain.Entities;
    using Shared;
    using Shared.Interfaces;

    /// <summary>   A post motorcycle request. </summary>
    public sealed class PostMotorcycleRequest
    {
        #region Properties

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the make, which is the manufacturer of the motorcycle. </summary>
        ///
        /// <value> The make. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public string Make { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets or sets the model, which is the particular version of a motorcycle from a manufacturer.
        /// </summary>
        ///
        /// <value> The model. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public string Model { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the vehicle identification number ("VIN"). </summary>
        ///
        /// <value> The vin. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public string Vin { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the year when the motorcycle was manufactured. </summary>
        ///
        /// <value> The year. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public uint Year { get; set; }

        #endregion

        #region Other Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// NewPostMotorcycleRequest creates a new instance of a PostMotorcycleRequest.
        /// </summary>
        ///
        /// <param name="make">     The make. </param>
        /// <param name="model">    The model. </param>
        /// <param name="year">     The year. </param>
        /// <param name="vin">      The vin. </param>
        ///
        /// <returns>
        /// Returns (null, IError) when there is an error, otherwise (PostMotorcycleRequest, null)
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static (PostMotorcycleRequest request, IError error) NewPostMotorcycleRequest(string make, string model, uint year, string vin)
        {
            var request = new PostMotorcycleRequest
            {
                Make = make,
                Model = model,
                Year = year,
                Vin = vin
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
            return Motorcycle.ValidateNonIdFields(Make, Model, Year, Vin);
        }

        #endregion
    }
}