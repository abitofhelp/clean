// SOLUTION: Clean
// PROJECT: Clean.Shared
// FILE: RepositoryBase.cs
// CREATED: Mike Gardner

namespace Clean.Shared.Data
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Enumerations;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A repository. </summary>
    ///
    /// <typeparam name="TEntity">  Type of the entity. </typeparam>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class, IEntity
    {
        #region Properties

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Set of entities in the context. </summary>
        ///
        /// <value> The database set. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public DbSet<TEntity> DbSet { get; set; }

        #endregion

        #region Constructors / Finalizers

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Specialized constructor for use only by derived class. </summary>
        ///
        /// <throwses cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </throwses>
        ///
        /// <param name="context">  Context for the database. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        protected RepositoryBase(IDbContext context)
        {
            Context = (DbContext<TEntity>) context ?? throw new ArgumentNullException(nameof(context));

            DbSet = Context.Set<TEntity>();
        }

        #endregion

        #region IRepository<TEntity> Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   The database context/unit of work. </summary>
        ///
        /// <value> The context. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IDbContext Context { get; set; }

        #endregion

        #region IDisposable

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Releases the unmanaged resources used by the Clean.Shared.Data.RepositoryBase&lt;TEntity&gt;
        /// and optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">    True to release both managed and unmanaged resources; false to
        ///                             release only unmanaged resources. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        protected virtual void Dispose(bool disposing)
        {
            if (disposing) { }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IRepository<in TP,TE>

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Delete the entity by its primary key field (Id). </summary>
        ///
        /// <param name="id">   The primary key field value to find. </param>
        ///
        /// <returns>   (Ok, null) for success, otherwise (null, Error) </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<(OperationStatus status, IError error)> DeleteAsync(long id)
        {
            var (entity, status, _) = await FetchByIdAsync(id);
            if (status == OperationStatus.NotFound)
            {
                return (status, new Error($"The entity with Id '{id}' could not be found, so it was not deleted."));
            }

            // Note that if the entity exists in the context in the Added state, then 
            // this method will cause it to be detached from the context. This is because 
            // an Added entity is assumed not to exist in the database such that trying
            // to delete it does not make sense.
            DbSet.Remove(entity);

            await SaveAsync();

            var existsResult = await ExistsByIdAsync(id);
            if (existsResult.exists)
            {
                return (status, new Error($"The entity with Id '{id}' was not successfully deleted from the repository."));
            }

            return (OperationStatus.Ok, null);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Determines whether an entity exists using its primary key field (Id). </summary>
        ///
        /// <param name="id">   The primary key field value to find. </param>
        ///
        /// <returns>   (true, Found, null) for success, or (false, Not Found, null) </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //public (bool exists, OperationStatus status, IError error) ExistsById(long id)
        //{
        //    var exists = DbSet.Any(o => o.Id.Equals(id));

        //    return (exists, exists
        //        ? OperationStatus.Found
        //        : OperationStatus.NotFound, null);
       // }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Determines whether an entity exists using its primary key field (Id). </summary>
        ///
        /// <param name="id">   The primary key field value to find. </param>
        ///
        /// <returns>   (true, Found, null) for success, or (false, Not Found, null) </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<(bool exists, OperationStatus status, IError error)> ExistsByIdAsync(long id)
        {
            var exists = await DbSet.AnyAsync(o => o.Id.Equals(id));

            return (exists, exists
                ? OperationStatus.Found
                : OperationStatus.NotFound, null);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Fetches the entity by its primary key field (Id). </summary>
        ///
        /// <param name="id">   The primary key field value to find. </param>
        ///
        /// <returns>   (entity, Found, null) for success, otherwise (null, Not Found, Error) </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //public (TEntity entity, OperationStatus status, IError error) FetchById(long id)
        //{
        //    var entity = DbSet.Find(id);

        //    return (entity, entity != null
        //        ? OperationStatus.Found
        //        : OperationStatus.NotFound, null);
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Fetches the entity by its primary key field (Id). </summary>
        ///
        /// <param name="id">   The primary key field value to find. </param>
        ///
        /// <returns>   (entity, Found, null) for success, otherwise (null, Not Found, Error) </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<(TEntity entity, OperationStatus status, IError error)> FetchByIdAsync(long id)
        {
            var entity = await DbSet.FindAsync(id);

            return (entity, entity != null
                ? OperationStatus.Found
                : OperationStatus.NotFound, null);
        }

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
        public async Task<(TEntity entity, OperationStatus status, IError error)> InsertAsync(TEntity entity, Func<TEntity, Task<(bool exists, OperationStatus status, IError error)>> doesEntityExist)
        {
            if (doesEntityExist == null)
            {
                return (null, OperationStatus.InternalError,
                    new Error("The doesEntityExist parameter must be provided to determine whether the entity already exists in the repository."));
            }

            (bool exists, _, _) = await doesEntityExist(entity);
            if (exists)
            {
                return (null, OperationStatus.Found, new Error("The entity already exists in the repository."));
            }

            // Save the time when this entity was created in the repository.
            entity.CreatedUtc = DateTime.UtcNow;

            await DbSet.AddAsync(entity);
            await SaveAsync();

            (exists, _, _) = await doesEntityExist(entity);
            if (!exists)
            {
                return (null, OperationStatus.NotFound, new Error("The new entity was not successfully inserted into the repository."));
            }

            return (entity, OperationStatus.Ok, null);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get a read-only list of entities from the repository. </summary>
        ///
        /// <returns>   A list of entities, which may be empty. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<(IReadOnlyCollection<TEntity> list, OperationStatus status, IError error)> ListAsync()
        {
            var list = await DbSet.ToListAsync();

            return (new ReadOnlyCollection<TEntity>(list), OperationStatus.Ok, null);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves all of the changes to the repository. </summary>
        ///
        /// <returns>   (Ok, null) </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<(OperationStatus status, IError error)> SaveAsync()
        {
            await Context.SaveChangesAsync();

            return (OperationStatus.Ok, null);
        }

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
        public async Task<(TEntity entity, OperationStatus status, IError error)> UpdateAsync(long id, TEntity entity, Func<TEntity, Task<bool>> isEntityUnique)
        {
            var (_, status, _) = await ExistsByIdAsync(id);
            if (status == OperationStatus.NotFound)
            {
                return (null, OperationStatus.NotFound, new Error($"The entity with Id '{id}' could not be found, so it was not updated."));
            }

            // Are any of the fields for the updated entity going to violate any unique constraints with existing entities?
            // For example, Car1 {"Id":1, "Vin":"123"}, Car2 {"Id":1, "Vin":"321"}, Updated Car1 {"Id":1, "Vin":"321"}
            if (isEntityUnique == null)
            {
                return (null, OperationStatus.InternalError,
                    new Error("The isEntityUnique parameter must be provided to determine whether the entity exists and is unique in the repository."));
            }

            if (! await isEntityUnique(entity))
            {
                return (null, OperationStatus.InternalError,
                    new Error($"The entity in the repository with Id '{id}' was not updated because a unique field constraint would be violated.")
                    );
            }
            
            // Save the time when this entity was updated in the repository.
            entity.ModifiedUtc = DateTime.UtcNow;

            // Assumes that the entity is being tracked in the context.
            await SaveAsync();

            var findResult = await FetchByIdAsync(id);
            if (!entity.Equals(findResult.entity))
            {
                return (null, OperationStatus.InternalError, new Error($"The entity with Id '{id}' failed to be updated in the repository."));
            }

            return (findResult.entity, OperationStatus.Ok, null);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Validate verifies that all of the fields in a RepositoryBase instance meet the requirements.
        /// </summary>
        ///
        /// <returns>   Null on success, otherwise an IError. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual IError Validate()
        {
            var error = new Error();

            if (Context == null) error.Add("The database context cannot be null.");

            if (DbSet == null) error.Add("The collection of entities cannot be null.");

            return error.Messages.Count > 0
                ? error
                : null;
        }

        #endregion
    }
}