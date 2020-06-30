using PRIS.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Storage
{
    public interface IRepository<T>
    {
        IQueryable<TEntity> Query<TEntity>() where TEntity : class, IEntity;
        Task<T> FindByIdAsync(int? id);
        Task<T> InsertAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(int? id);
        bool Exists(int? id);
    }
}
