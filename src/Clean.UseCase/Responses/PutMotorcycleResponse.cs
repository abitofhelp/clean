// SOLUTION: Clean
// PROJECT: Clean.UseCase
// FILE: PutMotorcycleResponse.cs
// CREATED: Mike Gardner

// Namespace Responses contains the responses for the use cases.
namespace Clean.UseCase.Responses
{
    using System;
    using Newtonsoft.Json;
    using Shared;
    using Shared.Enumerations;
    using Shared.Interfaces;

    /// <summary>   A put motorcycle response. </summary>
    public sealed class PutMotorcycleResponse
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
        /// <summary>   ID will be set to the value that was requested to be putd. </summary>
        ///
        /// <value> The identifier. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [JsonProperty("id")]
        public long Id { get; set; }

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
        /// <summary>   Creates a new put motorcycle response. </summary>
        ///
        /// <param name="id">       Identifier for the request. </param>
        /// <param name="status">   The not authenticated. </param>
        /// <param name="error">    The error. </param>
        ///
        /// <returns>   Returns (response, null) on success, or (null, error) on failure. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static (PutMotorcycleResponse response, IError error) NewPutMotorcycleResponse(long id, OperationStatus status, IError error)
        {
            // We return a (nil, error) only when validation of the response message fails, not for whether the
            // response message indicates failure.

            var response = new PutMotorcycleResponse
            {
                Id = id,
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
            
            return Id <= 0
                ? new Error("The id cannot be zero or a negative number.")
                : null;
        }

        #endregion
    }
}