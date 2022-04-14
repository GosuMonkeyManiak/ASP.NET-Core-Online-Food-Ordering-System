namespace FoodFun.Core.Services
{
    using FoodFun.Core.Contracts;
    using FoodFun.Core.Extensions;
    using FoodFun.Core.Models.Table;
    using FoodFun.Infrastructure.Common.Contracts;
    using FoodFun.Infrastructure.Models;
    using global::AutoMapper;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class TableService : ITableService
    {
        private readonly ITableRepository tableRepository;
        private readonly IMapper mapper;

        public TableService(ITableRepository tableRepository, IMapper mapper)
        { 
            this.tableRepository = tableRepository;
            this.mapper = mapper;
        }

        public async Task Add(int sizeId, int positionId)
        {
            await this.tableRepository
                    .AddAsync(new Table() { TableSizeId = sizeId, TablePositionId = positionId });

            await this.tableRepository
                    .SaveChangesAsync();
        }

        public async Task<IEnumerable<TableServiceModel>> All()
        {
            var tables = await this.tableRepository.AllWithPositionsAndSizes();

            return tables.ProjectTo<TableServiceModel>(this.mapper);
        }

        public async Task<bool> IsTableExist(string id)
            => await this.tableRepository
                .FindOrDefaultAsync(x => x.Id == id) != null;
    }
}
