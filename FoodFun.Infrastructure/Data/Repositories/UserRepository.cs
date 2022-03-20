namespace FoodFun.Infrastructure.Data.Repositories
{
    using Common.Contracts;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(FoodFunDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<User>> AllWithFilter(string searchTerm)
        {
            var query = this.DbSet.AsQueryable();

            if (searchTerm != null)
            {
                query = query
                    .Where(x => x.UserName.Contains(searchTerm)
                                || x.Email.Contains(searchTerm));
            }

            return await query
                .ToListAsync();
        }
    }
}
