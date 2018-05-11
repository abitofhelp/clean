// SOLUTION: Clean
// PROJECT: Clean.Shared
// FILE: IRepository.cs
// CREATED: Mike Gardner

namespace Clean.Shared.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Enumerations;

    // using Data;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Interface for a repository. </summary>
    ///
    /// <typeparam name="TEntity">  Type of the entity. </typeparam>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        #region Properties

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the context. </summary>
        ///
        /// <value> The context. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        IDbContext Context { get; set; }

        #endregion

        #region Other Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the asynchronous described by ID. </summary>
        ///
        /// <param name="id">   The identifier. </param>
        ///
        /// <returns>   An asynchronous result that yields the delete. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<(OperationStatus status, IError error)> DeleteAsync(long id);

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Exists by identifier asynchronous. </summary>
        ///
        /// <param name="id">   The identifier. </param>
        ///
        /// <returns>   An asynchronous result that yields the exists by identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<(bool exists, OperationStatus status, IError error)> ExistsByIdAsync(long id);

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Fetches by identifier asynchronous. </summary>
        ///
        /// <param name="id">   The identifier. </param>
        ///
        /// <returns>   An asynchronous result that yields the fetch by identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<(TEntity entity, OperationStatus status, IError error)> FetchByIdAsync(long id);

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Inserts an entity into the repository. </summary>
        ///
        /// <param name="entity">   The entity to insert into the repository. </param>
        /// <param name="doesEntityExist">   Implement the validation as to whether this entity already exists in
        ///                         the repository. </param>
        ///
        /// <returns>
        /// (entity, Ok, null) for success, otherwise (null, InternalError/Found/NotFound, Error)
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<(TEntity entity, OperationStatus status, IError error)> InsertAsync(TEntity entity, Func<TEntity, Task<(bool exists, OperationStatus status, IError error)>> doesEntityExist);

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   List asynchronous. </summary>
        ///
        /// <returns>   An asynchronous result that yields the list. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<(IReadOnlyCollection<TEntity> list, OperationStatus status, IError error)> ListAsync();

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves the asynchronous. </summary>
        ///
        /// <returns>   An asynchronous result that yields the save. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<(OperationStatus status, IError error)> SaveAsync();

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Update the entity in the repository. </summary>
        ///
        /// <remarks>   There are some fields that are immutable, such as Id and CreatedUtc. </remarks>
        ///
        /// <param name="id">       The primary key field value to find. </param>
        /// <param name="entity">   The entity to insert into the repository. </param>
        /// <param name="isEntityUnique"> Are any of the fields for the updated entity going to violate any
        ///                         unique constraints with existing entities? For example, Car1 {"Id":1,
        ///                         "Vin":"123"}, Car2 {"Id":1, "Vin":"321"}, Updated Car1 {"Id":1,
        ///                         "Vin":"321"} </param>
        ///
        /// <returns>
        /// (entity, Ok, null) for success, otherwise (null, InternalError/Found/NotFound, Error)
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<(TEntity entity, OperationStatus status, IError error)> UpdateAsync(long id, TEntity entity, Func<TEntity,  Task<bool>> isEntityUnique);

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the validate. </summary>
        ///
        /// <returns>   An IError. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        IError Validate();

        #endregion
    }
}