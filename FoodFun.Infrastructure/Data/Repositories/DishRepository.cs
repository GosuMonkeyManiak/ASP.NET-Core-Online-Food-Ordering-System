namespace FoodFun.Infrastructure.Data.Repositories
{
    using Common.Contracts;
    using Models;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public class DishRepository : EfRepository<Dish>
    {
        public DishRepository(FoodFunDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
