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
    using FoodFun.Infrastructure.Models;
    using FoodFun.Core.Models.Reservation;

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

        public async Task<IEnumerable<ReservationServiceModel>> AllByDate(DateOnly date)
        {
            var table = (await this.reservationRepository
                .GetAllWithTablesByDate(date))
                .ToList();

            return table.ProjectTo<ReservationServiceModel>(this.mapper);
        }

        public async Task<IEnumerable<TableServiceModel>> FreeTables(DateOnly date)
        {
            var reservationsByDate = await this.reservationRepository.All(date);

            var allTables = (await this.tableService.All());

            if (reservationsByDate.Any())
            {
                allTables = allTables
                    .Where(t => !reservationsByDate.Any(x => x.TableId == t.Id))
                    .ToList();
            }

            return allTables;
        }

        public async Task<bool> Reserv(DateOnly date, string tableId, string userId)
        {
            var newReservation = new Reservation()
            {
                TableId = tableId,
                UserId = userId,
                Date = date.ToDateTime(new TimeOnly())
            };

            await this.reservationRepository
                .AddAsync(newReservation);

            try
            {
                await this.reservationRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
