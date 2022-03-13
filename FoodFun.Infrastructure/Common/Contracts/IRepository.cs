namespace FoodFun.Infrastructure.Common.Contracts
{
    using System.Linq.Expressions;

    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task AddAsync(TEntity entity);

        void Remove(TEntity entity);

        void Update(TEntity entity);

        Task<TEntity> FindOrDefault(Expression<Func<TEntity, bool>> predicate);

        Task<int> SaveChangesAsync();
    }
}
