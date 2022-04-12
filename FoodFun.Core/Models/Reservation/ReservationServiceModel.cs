namespace FoodFun.Core.Models.Reservation
{
    using FoodFun.Core.Models.Table;

    public class ReservationServiceModel
    {
        public string UserName { get; set; }

        public TableServiceModel Table { get; init; }
    }
}
