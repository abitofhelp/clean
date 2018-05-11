// SOLUTION: Clean
// PROJECT: Clean.Adapter
// FILE: MotorcycleContext.cs
// CREATED: Mike Gardner

namespace Clean.Adapter.Gateways.Repositories
{
    using System;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
    using Microsoft.Extensions.DependencyInjection;
    using Shared.Data;

    /// <summary>   A motorcycle context. This class cannot be inherited. </summary>
    public sealed class MotorcycleContext : DbContext<Motorcycle>
    {
        #region Constructors / Finalizers

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="options">  Options for controlling the operation. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public MotorcycleContext(DbContextOptions<MotorcycleContext> options) : base(options) { }

        #endregion

        #region Other Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Configure Motorcycle. </summary>
        ///
        /// <param name="builder">  The builder being used to construct the model for this context.
        ///                         Databases (and other extensions) typically define extension methods
        ///                         on this object that allow you to configure aspects of the model that
        ///                         are specific to a given database. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ConfigureMotorcycle(EntityTypeBuilder<Motorcycle> builder)
        {
            // Using query filters to automatically filter out any motorcycles that have been 
            // soft-deleted and to only return motorcycles belonging to the particular tenant.
            builder.HasQueryFilter(p => !p.IsDeleted && p.TenantId == TenantId);

            builder.ToTable("Motorcycles");

            builder.HasKey(ci => ci.Id);
            builder.Property(ci => ci.Id)
                   .HasValueGenerator<InMemoryIntegerValueGenerator<long>>();

            builder.HasIndex(u => u.TenantId);
            builder.Property(cb => cb.TenantId)
                   .IsRequired();

            builder.Property(cb => cb.Make)
                   .IsRequired();

            builder.Property(cb => cb.Model)
                   .IsRequired();

            builder.Property(cb => cb.Vin)
                   .IsRequired()
                   .HasMaxLength(17);

            builder.Property(cb => cb.Year)
                   .IsRequired();

            builder.Property(cb => cb.CreatedUtc)
                   .IsRequired();

            builder.Property(cb => cb.Make)
                   .IsRequired();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Creates a new context options object for testing purposes. </summary>
        ///
        /// <returns>   The new context options. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public new static DbContextOptions<MotorcycleContext> CreateTestingContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase()
                                                         .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.  Using a GUID as a unique database name.
            var builder = new DbContextOptionsBuilder<MotorcycleContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid()
                                            .ToString())
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Load the database context with test data. </summary>
        ///
        /// <param name="context">  Context is for the database. </param>
        /// <param name="count">    (Optional) Count is the number of entities to create. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void LoadContextWithTestData(MotorcycleContext context, long count = 1)
        {
            for (var i = 0;
                 i < count;
                 i++)
            {
                // Generate a motorcycle with test data.
                (Motorcycle motorcycle, _) = Motorcycle.NewTestMotorcycle();

                context.Entities.Add(motorcycle);
            }

            context.SaveChanges();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from
        /// the entity types exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties
        /// on your derived context. The resulting model may be cached and re-used for subsequent
        /// instances of your derived context.
        /// </summary>
        ///
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </remarks>
        ///
        /// <param name="builder">  The builder being used to construct the model for this context.
        ///                         Databases (and other extensions) typically define extension methods
        ///                         on this object that allow you to configure aspects of the model that
        ///                         are specific to a given database. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Motorcycle>(ConfigureMotorcycle);
            base.OnModelCreating(builder);
        }

        #endregion
    }
}