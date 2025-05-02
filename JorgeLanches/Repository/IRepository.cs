using JorgeLanches.Pagination;
using System.Linq.Expressions;

namespace JorgeLanches.Repository
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(PaginationParameters paginationParameters);        

        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);

        T Create(T entity);

        T Update(T entity);

        T Delete(T entity);

        Task CommitAsync();

        void Dispose();
    }
}
