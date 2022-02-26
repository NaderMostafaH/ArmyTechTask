using System.Linq.Expressions;

namespace Account.DataAccess.IRepository
{
    public interface IRepositoryAsync<T> where T : class
    {
        Task<T> GetAsync(int id);
        Task<T> GetAsync(long? id);

        Task<IEnumerable<T>> GetAllAsync(
             Expression<Func<T, bool>> filter = null,
             Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
             string includeProperties = null,
             int skip = 0,
             int take = 0
             );
        Task<T> GetFirstOrDefaultAsync(
             Expression<Func<T, bool>> filter = null,
             string includeProperties = null
             );

        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entity);
        Task RemoveAsync(int id);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entity);
        Task<int> Count(Expression<Func<T, bool>> filter = null);
    }
}
