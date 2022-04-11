namespace FoodFun.Core.Services
{
    using FoodFun.Core.Contracts;
    using FoodFun.Core.Extensions;
    using FoodFun.Core.Models.Table;
    using FoodFun.Infrastructure.Common.Contracts;
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

        public async Task<IEnumerable<TableServiceModel>> All()
        {
            var tables = await this.tableRepository.AllWithPositionsAndSizes();

            return tables.ProjectTo<TableServiceModel>(this.mapper);
        }
    }
}
