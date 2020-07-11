using PRIS.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Storage
{
    public interface IRepository
    {
        Task SaveAsync();
        IQueryable<TEntity> Query<TEntity>() where TEntity : class, IEntity;
        Task<TEntity> FindByIdAsync<TEntity>(int? id) where TEntity : class, IEntity;
        Task<TEntity> InsertAsync<TEntity>(TEntity entity) where TEntity : class, IEntity;
        Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : class, IEntity;
        Task<TEntity> DeleteAsync<TEntity>(int? id) where TEntity : class, IEntity;
        bool Exists<TEntity>(int? id) where TEntity : class, IEntity;
    }
}
