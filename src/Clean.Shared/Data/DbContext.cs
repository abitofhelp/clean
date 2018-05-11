// SOLUTION: Clean
// PROJECT: Clean.Shared
// FILE: DbContext.cs
// CREATED: Mike Gardner

namespace Clean.Shared.Data
{
    using System;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    //using Microsoft.EntityFrameworkCore.ChangeTracking;
    //using Microsoft.EntityFrameworkCore.Infrastructure;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A database context. </summary>
    ///
    /// <typeparam name="TEntity">  Type of the entity. </typeparam>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public abstract class DbContext<TEntity> : DbContext, IDisposable, IDbContext where TEntity : class, IEntity
    {
        #region Constructors / Finalizers

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Specialised constructor for use only by derived class. </summary>
        ///
        /// <param name="options">  A builder used to create or modify options for this context.
        ///                         Databases (and other extensions)
        ///                         typically define extension methods on this object that allow you to
        ///                         configure the context. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        protected DbContext(DbContextOptions options) : base(options) { }

        #endregion

        #region Other Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Creates a new context options object for testing purposes. </summary>
        ///
        /// <returns>   The new context options. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static DbContextOptions<DbContext<TEntity>> CreateTestingContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase()
                                                         .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.  Using a GUID as a unique database name.
            var builder = new DbContextOptionsBuilder<DbContext<TEntity>>();
            builder.UseInMemoryDatabase(Guid.NewGuid()
                                            .ToString())
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// <para>
        ///                 Override this method to configure the database (and other options) to be used
        ///                 for this context. This method is called for each instance of the context that
        ///                 is created.
        ///             </para>
        /// <para>
        ///                 In situations where an instance of
        ///                 <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" /> may or may
        ///                 not have been passed to the constructor, you can use
        /// <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" /> to
        /// determine if the options have already been set, and skip some or all of the logic in
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />.
        ///             </para>
        /// </summary>
        ///
        /// <param name="options">  A builder used to create or modify options for this context.
        ///                         Databases (and other extensions)
        ///                         typically define extension methods on this object that allow you to
        ///                         configure the context. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // When no options are provided, we will assume that we will use a real database, rather than an in-memory database, which is used for testing purposes.
            if (!options.IsConfigured)
            {
                // options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0");
            }

            options.UseInMemoryDatabase("MotoMinder");
            base.OnConfiguring(options);
        }

        #endregion

        #region IDisposable

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Releases the unmanaged resources used by the Clean.Shared.Data.DbContext&lt;TEntity&gt; and
        /// optionally releases the managed resources.
        /// </summary>
        ///
        /// <param name="disposing">    True to release both managed and unmanaged resources; false to
        ///                             release only unmanaged resources. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var entity in Entities) { entity?.Dispose(); }

                base.Dispose();
            }
        }

        /// <summary>   Releases the allocated resources for this context. </summary>
        public sealed override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IDbContext<TPrimaryKey,TEntity>

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the entities. </summary>
        ///
        /// <value> The entities. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public DbSet<TEntity> Entities { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the identifier of the tenant. </summary>
        ///
        /// <value> The identifier of the tenant. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public long TenantId { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves the changes asynchronous. </summary>
        ///
        /// <returns>   An asynchronous result that yields the save changes. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Validate verifies that all of the fields in a DatabaseContextBase instance meet the
        /// requirements.
        /// </summary>
        ///
        /// <returns>   Null on success, otherwise an Error. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual IError Validate()
        {
            var error = new Error();

            if (Entities == null) error.Add("The collection of entities cannot be null.");

            if (TenantId < 0) error.Add("The TenantId cannot be a negative value.");

            return error.Messages.Count > 0
                ? error
                : null;
        }

        #endregion
    }
}