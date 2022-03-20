namespace FoodFun.Infrastructure.Common.Contracts
{
    using Models;

    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> AllWithFilter(string searchTerm);
    }
}
