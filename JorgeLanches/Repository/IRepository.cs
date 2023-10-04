using System.Linq.Expressions;

namespace JorgeLanches.Repository
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();

        Task<T> GetById(Expression<Func<T, bool>> predicate);

        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
