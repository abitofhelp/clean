// SOLUTION: Clean
// PROJECT: Clean.Shared
// FILE: IDatabaseContextBase.cs
// CREATED: Mike Gardner

namespace Clean.Shared.Interfaces
{
    using System;
    using Microsoft.EntityFrameworkCore;

    public interface IDatabaseContextBase<TE> : IDisposable where TE : class, IEntity 
    {
        #region Properties

        DbSet<TE> Entities { get; set; }

        long TenantId { get; set; }

        DbSet<TE> Set();

        IError Validate();

        #endregion
    }

 
}