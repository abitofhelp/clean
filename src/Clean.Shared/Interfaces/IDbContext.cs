﻿// SOLUTION: Clean
// PROJECT: Clean.Shared
// FILE: IDbContext.cs
// CREATED: Mike Gardner

namespace Clean.Shared.Interfaces
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    /// <summary>   Interface for database context. </summary>
    public interface IDbContext
    {
        #region Properties

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the identifier of the tenant. </summary>
        ///
        /// <value> The identifier of the tenant. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        long TenantId { get; set; }

        #endregion

        #region Other Members

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves the changes asynchronous. </summary>
        ///
        /// <returns>   An asynchronous result that yields the save changes. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        Task<int> SaveChangesAsync();

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the set. </summary>
        ///
        /// <typeparam name="TEntity">  Type of the entity. </typeparam>
        ///
        /// <returns>   A DbSet&lt;TEntity&gt; </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the validate. </summary>
        ///
        /// <returns>   An IError. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        IError Validate();

        #endregion
    }
}