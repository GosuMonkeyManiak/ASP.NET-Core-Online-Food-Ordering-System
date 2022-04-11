namespace FoodFun.Core.Services
{
    using FoodFun.Core.Contracts;
    using FoodFun.Core.Models.Table;
    using FoodFun.Infrastructure.Common.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using FoodFun.Core.Extensions;
    using global::AutoMapper;

    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly ITableService tableService;
        private readonly IMapper mapper;

        public ReservationService(
            IReservationRepository reservationRepository,
            ITableService tableService,
            IMapper mapper)
        {
            this.reservationRepository = reservationRepository;
            this.tableService = tableService;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TableServiceModel>> FreeTables(DateOnly date)
        {
            var reservationsByDate = await this.reservationRepository.GetAllByDate(date);

            var allTables = (await this.tableService.All());

            if (reservationsByDate.Any())
            {
                allTables = allTables
                    .Where(t => reservationsByDate.Any(x => x.TableId != t.Id))
                    .ToList();
            }

            return allTables;
        }
    }
}
