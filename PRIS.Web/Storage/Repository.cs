using Microsoft.EntityFrameworkCore;
using PRIS.Core.Library.Entities;
using PRIS.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Storage
{
    public class Repository : IRepository, IDisposable
    {
        private readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<TEntity> Query<TEntity>() where TEntity : class, IEntity
        {
            return _context.Set<TEntity>();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<TEntity> InsertAsync<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<TEntity> DeleteAsync<TEntity>(int? id) where TEntity : class, IEntity
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return entity;
            }
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<TEntity> FindByIdAsync<TEntity>(int? id) where TEntity : class, IEntity
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(m => m.Id == id);
        }
        public bool Exists<TEntity>(int? id) where TEntity : class, IEntity
        {
            return _context.Set<TEntity>().Any(e => e.Id == id);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
