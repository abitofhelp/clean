// SOLUTION: Clean
// PROJECT: Clean.Adapter
// FILE: MotorcycleRepository.cs
// CREATED: Mike Gardner

// Namespace Repositories contains implementations of data repositories.
namespace Clean.Adapter.Gateways.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Shared.Data;
    using Shared.Enumerations;
    using Shared.Interfaces;

    /// <summary>   A motorcycle repository. This class cannot be inherited. </summary>
    public sealed class MotorcycleRepository : RepositoryBase<Motorcycle>, IMotorcycleRepository
    {
        #region Constructors / Finalizers

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="dbContext">    Context for the database. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        internal MotorcycleRepository(MotorcycleContext dbContext) : base(dbContext) { }

        #endregion

        #region Other Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Determines whether a motorcycle exists using its unique VIN. </summary>
        ///
        /// <param name="vin">  The unique Vehicle Identification Number field value to find. </param>
        ///
        /// <returns>   (true, Found, null) for success, or (false, Not Found, null) </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<(bool exists, OperationStatus status, IError error)> ExistsByVinAsync(string vin)
        {
            var exists = await DbSet.AnyAsync(o => o.Vin.Equals(vin, StringComparison.CurrentCultureIgnoreCase));

            return (exists, exists
                ? OperationStatus.Found
                : OperationStatus.NotFound, null);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Fetches the motorcycle using its unique VIN. </summary>
        ///
        /// <param name="vin">  The unique Vehicle Identification Number field value to find. </param>
        ///
        /// <returns>   (entity, Found, null) for success, otherwise (null, Not Found, Error) </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<(Motorcycle entity, OperationStatus status, IError error)> FetchByVinAsync(string vin)
        {
            var entity = await DbSet.SingleOrDefaultAsync(x => x.Vin.Equals(vin, StringComparison.CurrentCultureIgnoreCase));

            return (entity, entity != null
                ? OperationStatus.Found
                : OperationStatus.NotFound, null);
        }

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
        public async Task<(bool exists, OperationStatus status, IError error)> IsVinUniqueAsync(string vin)
        {
            (bool exists, _, _) = await ExistsByVinAsync(vin);

            return (!exists, exists
                ? OperationStatus.Found
                : OperationStatus.NotFound, null);
        }

        #endregion
    }
}