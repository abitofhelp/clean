// SOLUTION: Clean
// PROJECT: Clean.UseCase
// FILE: GetMotorcycleResponse.cs
// CREATED: Mike Gardner

// Namespace Responses contains the responses for the use cases.
namespace Clean.UseCase.Responses
{
    using System;
    using Clean.Domain.Entities;
    using Newtonsoft.Json;
    using Shared;
    using Shared.Enumerations;
    using Shared.Interfaces;

    /// <summary>   A get motorcycle response. </summary>
    public sealed class GetMotorcycleResponse
    {
        #region Properties

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the error. </summary>
        ///
        /// <value> The error or null on success. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [JsonProperty("error")]
        public IError Error { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the motorcycle instance being returned. </summary>
        ///
        /// <value> The requested motorcycle </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [JsonProperty("motorcycle")]
        public Motorcycle Motorcycle { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets or sets the operation status, which indicates the outcome of the deletion request.
        /// </summary>
        ///
        /// <value> The operation status. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [JsonProperty("status")]
        public OperationStatus Status { get; set; }

        #endregion

        #region Other Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Creates a new get motorcycle response. </summary>
        ///
        /// <param name="motorcycle"> An instance of a motorcycle that has been requested.</param>
        /// <param name="status">   The not authenticated. </param>
        /// <param name="error">    The error. </param>
        ///
        /// <returns>   Returns (response, null) on success, or (null, error) on failure. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static (GetMotorcycleResponse response, IError error) NewGetMotorcycleResponse(Motorcycle motorcycle, OperationStatus status, IError error)
        {
            // We return a (nil, error) only when validation of the response message fails, not for whether the
            // response message indicates failure.

            var response = new GetMotorcycleResponse
            {
                Motorcycle = motorcycle,
                Status = status,
                Error = error
            };

            var err = response.Validate();

            // If we have a response message with a failure and validation failed, we will wrap the original error with the validation error.
            if (response.Error != null && err != null)
            {
                err.AddRange(response.Error.Messages);
                return (null, err);
            }

            // If we have a response message that indicates success, but validation failed, we will return the validation error.
            if (response.Error == null && err != null)
            {
                return (null, err);
            }

            // If we have a response message that failed, but validation was successful, we will return response's error.
            if (response.Error != null && err == null)
            {
                return (response, response.Error);
            }

            // Otherwise, all okay
            return (response, null);
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
            if (!Enum.IsDefined(typeof(OperationStatus), Status)) return new Error($"The status value '{Status}' does not exist in the enumeration.");

            return Motorcycle?.Validate();
        }

        #endregion
    }
}