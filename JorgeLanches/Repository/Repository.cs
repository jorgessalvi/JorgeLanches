using JorgeLanches.Context;
using JorgeLanches.Models;
using JorgeLanches.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JorgeLanches.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public T Create(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public T Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();                        
        }

        public async Task<IEnumerable<T>> GetAllAsync(PaginationParameters paginationParameters)
        {
            var itens = await _context.Set<T>().AsNoTracking().ToListAsync();

            return Pagination<T>.FilterPages(itens.AsQueryable(), paginationParameters);
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return entity;
        }
    }
}
