using APIKlinik.Domain.Entities;
using APIKlinik.Domain.Interfaces;
using APIKlinik.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly APIDbContext _context;
        protected readonly DbSet<T> _entities;

        public BaseRepository(APIDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _entities.ToListAsync();

        public async Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _entities;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }


        public async Task<T> GetByIdAsync(int id) => await _entities.FindAsync(id);

        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _entities.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _entities.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
            await _entities.Where(predicate).ToListAsync();

        public async Task<PagedResult<T>> GetPagedAsync(int page, int pageSize, Expression<Func<T, bool>>? filter = null)
        {
            var query = _entities.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            var totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<T>> GetPagedOrderedAsync<TKey>(
        int page,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, TKey>>? orderBy = null,
        bool ascending = true)
            {
                var query = _entities.AsQueryable();

                if (filter != null)
                    query = query.Where(filter);

                if (orderBy != null)
                {
                    query = ascending ?
                        query.OrderBy(orderBy) :
                        query.OrderByDescending(orderBy);
                }                

                var totalItems = await query.CountAsync();
                var items = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PagedResult<T>
                {
                    Items = items,
                    TotalItems = totalItems,
                    Page = page,
                    PageSize = pageSize
                };
            }

        public async Task<PagedResult<T>> GetPagedWithIncludesAsync(
            int page,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            params Expression<Func<T, object>>[] includes)
        {
            var query = _entities.AsQueryable();

            foreach (var include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            var totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task DeleteEntityAsync(T entity)
        {
            if (entity != null)
            {
                _entities.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
