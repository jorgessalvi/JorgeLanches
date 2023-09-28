using JorgeLanches.Context;
using Microsoft.EntityFrameworkCore;

namespace JorgeLanches.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected AppDbContext _Context;

        public Repository(AppDbContext context)
        {
            _Context = context;
        }

        public void Add(T entity)
        {
            _Context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _Context.Set<T>().Remove(entity);
        }

        public IQueryable<T> Get()
        {
            return _Context.Set<T>().AsNoTracking();
        }

        public T GetById(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _Context.Set<T>().SingleOrDefault(predicate);
        }

        public void Update(T entity)
        {
            _Context.Entry(entity).State = EntityState.Modified;
            _Context.Set<T>().Update(entity);
        }
    }
}
