namespace FoodFun.Core.Services
{
    using FoodFun.Core.Contracts;
    using FoodFun.Infrastructure.Common.Contracts;
    using System.Threading.Tasks;
    using Infrastructure.Models;
    using System.Collections.Generic;
    using FoodFun.Core.Models.TableSize;
    using FoodFun.Core.Extensions;
    using global::AutoMapper;

    public class TableSizeService : ITableSizeService
    {
        private readonly ITableSizeRepository tableSizeRepository;
        private readonly IMapper mapper;

        public TableSizeService(
            ITableSizeRepository tableSizeRepository, 
            IMapper mapper)
        {
            this.tableSizeRepository = tableSizeRepository;
            this.mapper = mapper;
        }

        public async Task Add(int seats)
        {
            await this.tableSizeRepository
                    .AddAsync(new TableSize() { Seats = seats });

            await this.tableSizeRepository
                    .SaveChangesAsync();
        }

        public async Task<IEnumerable<TableSizeServiceModel>> All()
            => (await this.tableSizeRepository
                .AllAsNoTracking())
                .ProjectTo<TableSizeServiceModel>(this.mapper);

        public async Task<TableSizeServiceModel> GetByIdOrDefault(int id)
        {
            if (!await IsExistById(id))
            {
                return null;
            }

            var tableSize = await this.tableSizeRepository.FindAsync(x => x.Id == id);

            return this.mapper.Map<TableSizeServiceModel>(tableSize);
        }

        public async Task<bool> IsExist(int seats)
            => await this.tableSizeRepository
                .FindOrDefaultAsync(x => x.Seats == seats) != null;

        public async Task<bool> IsExistById(int id)
            => await this.tableSizeRepository
                .FindOrDefaultAsync(x => x.Id == id) != null;

        public async Task Update(int id, int seats)
        {
            var tableSize = new TableSize() { Id = id, Seats = seats };

            this.tableSizeRepository
                    .Update(tableSize);

            await this.tableSizeRepository.SaveChangesAsync();
        }
    }
}
