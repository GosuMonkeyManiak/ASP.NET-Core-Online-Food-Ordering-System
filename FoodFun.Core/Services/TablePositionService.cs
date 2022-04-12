namespace FoodFun.Core.Services
{
    using FoodFun.Core.Contracts;
    using FoodFun.Infrastructure.Common.Contracts;
    using System.Threading.Tasks;
    using Infrastructure.Models;
    using System.Collections.Generic;
    using FoodFun.Core.Models.TablePosition;
    using FoodFun.Core.Extensions;
    using global::AutoMapper;

    public class TablePositionService : ITablePositionService
    {
        private readonly ITablePositionRepository tablePositionRepository;
        private readonly IMapper mapper;

        public TablePositionService(
            ITablePositionRepository tablePositionRepository,
            IMapper mapper)
        { 
            this.tablePositionRepository = tablePositionRepository;
            this.mapper = mapper;
        }

        public async Task Add(string position)
        {
            await this.tablePositionRepository
                .AddAsync(new TablePosition() { Position = position });

            await this.tablePositionRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TablePositionServiceModel>> All()
            => (await this.tablePositionRepository
                .AllAsNoTracking())
                .ProjectTo<TablePositionServiceModel>(this.mapper);

        public async Task<TablePositionServiceModel> GetByIdOrDefault(int id)
        {
            if (!await IsExist(id))
            {
                return null;
            }

            var tablePosition = await this.tablePositionRepository.FindAsync(x => x.Id == id);

            return mapper.Map<TablePositionServiceModel>(tablePosition);
        }

        public async Task<bool> IsExist(string position)
            => await this.tablePositionRepository
                .FindOrDefaultAsync(x => x.Position == position) != null;

        public async Task<bool> IsExist(int id)
            => await this.tablePositionRepository
                .FindOrDefaultAsync(x => x.Id == id) != null;

        public async Task Update(int id, string position)
        {
            var tablePosition = new TablePosition()
            {
                Id = id,
                Position = position
            };

            this.tablePositionRepository
                .Update(tablePosition);

            await this.tablePositionRepository
                    .SaveChangesAsync();
        }
    }
}
