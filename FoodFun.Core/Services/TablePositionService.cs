namespace FoodFun.Core.Services
{
    using FoodFun.Core.Contracts;
    using FoodFun.Infrastructure.Common.Contracts;
    using System.Threading.Tasks;
    using Infrastructure.Models;

    public class TablePositionService : ITablePositionService
    {
        private readonly ITablePositionRepository tablePositionRepository;

        public TablePositionService(ITablePositionRepository tablePositionRepository) 
            => this.tablePositionRepository = tablePositionRepository;

        public async Task Add(string position)
        {
            await this.tablePositionRepository
                .AddAsync(new TablePosition() { Position = position });

            await this.tablePositionRepository.SaveChangesAsync();
        }

        public async Task<bool> IsExist(string position)
            => await this.tablePositionRepository
                .FindOrDefaultAsync(x => x.Position == position) != null;
    }
}
