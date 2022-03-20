namespace FoodFun.Core.Contracts
{
    using Models.User;

    public interface IUserService
    {
        Task<IEnumerable<UserServiceModel>> All(string searchTerm);
    }
}
