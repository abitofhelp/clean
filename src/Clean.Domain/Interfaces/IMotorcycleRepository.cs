// SOLUTION: Clean
// PROJECT: Clean.Shared
// FILE: IMotorcycleRepository.cs
// CREATED: Mike Gardner

namespace Clean.Shared.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Clean.Domain.Entities;
    using Enumerations;

    // using Data;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Interface for a motorcycle repository. </summary>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public interface IMotorcycleRepository : IRepository<Motorcycle>, IDisposable
    {
        #region Other Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Determines whether a motorcycle exists using its unique VIN. </summary>
        ///
        /// <param name="vin">  The unique Vehicle Identification Number field value to find. </param>
        ///
        /// <returns>   (true, Found, null) for success, or (false, Not Found, null) </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        System.Threading.Tasks.Task<(bool exists, OperationStatus status, IError error)> ExistsByVinAsync(string vin);

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Fetches the motorcycle using its unique VIN. </summary>
        ///
        /// <param name="vin">  The unique Vehicle Identification Number field value to find. </param>
        ///
        /// <returns>   (entity, Found, null) for success, otherwise (null, Not Found, Error) </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        System.Threading.Tasks.Task<(Motorcycle entity, OperationStatus status, IError error)> FetchByVinAsync(string vin);

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Determines whether a motorcycle with the VIN already exists in the repository.
        /// </summary>
        ///
        /// <remarks>
        /// This method only determines whether a VIN exists in the repository.  It does not consider the
        /// implications when an insert or update operation is involved.
        /// </remarks>
        ///
        /// <param name="vin">  The unique Vehicle Identification Number field value to find. </param>
        ///
        /// <returns>   (true, NotFound, null) for success, or (false, Not Found, null) </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        System.Threading.Tasks.Task<(bool exists, OperationStatus status, IError error)> IsVinUniqueAsync(string vin);
        
        #endregion
    }
}