// SOLUTION: Clean
// PROJECT: Clean.Shared
// FILE: DatabaseContextBase.cs
// CREATED: Mike Gardner

namespace Clean.Shared.Data
{
    using System;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    /// <typeparam name="TP">   Type of the primary key (i.e. ulong, string, etc.) </typeparam>
    /// <typeparam name="TE">   Type of the entity </typeparam>
    public abstract class DatabaseContextBase<TE> : DbContext, IDatabaseContextBase<TE> where TE : class, IEntity
    {
        #region IDisposable

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Dispose();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Dispose()
        {
            foreach (var entity in Entities) { entity?.Dispose(); }

            base.Dispose();
        }

        #endregion

        #region Constructors / Finalizers

        protected DatabaseContextBase(DbContextOptions options) : base(options) { }

        protected DatabaseContextBase(DbContextOptions<DatabaseContextBase<TE>> options) : this((DbContextOptions)options) { }

        #endregion

        #region IDatabaseContextBase<TP,TE> Members

        public DbSet<TE> Entities { get; set; }

        public long TenantId { get; set; }

        public DbSet<TE> Set()
        {
            return Set<TE>();
        }

        /// <summary>   Validate verifies that all of the fields in a DatabaseContextBase instance meet the requirements. </summary>
        ///
        /// <returns>   Null on success, otherwise an Error. </returns>
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

        #region Other Members

        /// <summary>   Creates a new context options object for testing purposes. </summary>
        ///
        /// <returns>   The new context options. </returns>
        public static DbContextOptions<DatabaseContextBase<TE>>  CreateTestingContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase()
                                                         .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.  Using a GUID as a unique database name.
            var builder = new DbContextOptionsBuilder<DatabaseContextBase<TE>>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString())
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        /* protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // When no options are provided, we will assume that we will use a real database, rather than an in-memory database, which is used for testing purposes.
            if (!options.IsConfigured)
            {
                // options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0");
            }

            options.UseInMemoryDatabase("MotoMinder");
            base.OnConfiguring(options);
        }
        */

        #endregion
    }
}