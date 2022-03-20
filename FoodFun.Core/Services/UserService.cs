namespace FoodFun.Core.Services
{
    using Contracts;
    using Extensions;
    using global::AutoMapper;
    using Infrastructure.Common.Contracts;
    using Models.User;

    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UserService(
            IUserRepository userRepository, 
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<UserServiceModel>> All(string searchTerm)
        {
            string defaultValueSearchTerm = null;

            if (searchTerm != null)
            {
                defaultValueSearchTerm = searchTerm;
            }

            var users = await this.userRepository.AllWithFilter(searchTerm);

            return users.ProjectTo<UserServiceModel>(this.mapper);
        }
    }
}
