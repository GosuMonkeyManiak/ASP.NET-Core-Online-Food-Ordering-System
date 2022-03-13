namespace FoodFun.Infrastructure.Common.Contracts
{
    using System.Linq.Expressions;

    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        Task<IEnumerable<TEntity>> All();

        Task<IEnumerable<TEntity>> AllAsNoTracking();

        Task AddAsync(TEntity entity);

        void Remove(TEntity entity);

        void Update(TEntity entity);

        Task<TEntity> FindOrDefault(Expression<Func<TEntity, bool>> predicate);

        Task<int> SaveChangesAsync();
    }
}
