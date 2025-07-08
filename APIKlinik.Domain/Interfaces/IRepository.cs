using APIKlinik.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task DeleteEntityAsync(T entity);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);

        Task<PagedResult<T>> GetPagedAsync(int page, int pageSize, Expression<Func<T, bool>>? filter = null);
        Task<PagedResult<T>> GetPagedWithIncludesAsync(int page, int pageSize, Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes);
        Task<PagedResult<T>> GetPagedOrderedAsync<TKey>(int page, int pageSize,
           Expression<Func<T, bool>>? filter = null,
           Expression<Func<T, TKey>>? orderBy = null,
           bool ascending = true);
    }
}
