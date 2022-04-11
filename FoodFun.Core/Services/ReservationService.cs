namespace FoodFun.Core.Services
{
    using FoodFun.Core.Contracts;
    using FoodFun.Core.Models.Table;
    using FoodFun.Infrastructure.Common.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository reservationRepository;

        public ReservationService(IReservationRepository reservationRepository) 
            => this.reservationRepository = reservationRepository;

        public async Task<IEnumerable<TableServiceModel>> FreeTables(DateOnly date)
        {
            var reservationByDate = await this.reservationRepository.GetAllByDate(date);

            var allTables = 
        }
    }
}
