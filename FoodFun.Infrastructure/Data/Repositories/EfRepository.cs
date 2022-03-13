namespace FoodFun.Infrastructure.Data.Repositories
{
    using Common.Contracts;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public abstract class EfRepository<TEntity> : IRepository<TEntity> 
        where TEntity : class
    {
        private readonly DbContext dbContext;

        protected EfRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;

            this.DbSet = dbContext.Set<TEntity>();
        }

        protected DbSet<TEntity> DbSet { get; private init; }

        public async Task<IEnumerable<TEntity>> All()
            => await this.DbSet.ToListAsync();

        public async Task<IEnumerable<TEntity>> AllAsNoTracking()
            => await this.DbSet
                .AsNoTracking()
                .ToListAsync();

        public async Task AddAsync(TEntity entity)
            => await this.DbSet.AddAsync(entity);

        public void Remove(TEntity entity)
            => this.DbSet.Remove(entity);

        public void Update(TEntity entity)
        {
            var entry = this.dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public async Task<TEntity> FindOrDefault(Expression<Func<TEntity, bool>> predicate)
            => await this.DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate);

        public async Task<int> SaveChangesAsync()
            => await this.dbContext.SaveChangesAsync();

        public void Dispose()
        {
            this.dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
