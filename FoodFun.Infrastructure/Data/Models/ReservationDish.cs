namespace FoodFun.Infrastructure.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Common.DataConstants;

    public class ReservationDish
    {
        [ForeignKey(nameof(Reservation))]
        public int ReservationId { get; init; }

        public Reservation Reservation { get; init; }

        [Required]
        [MaxLength(DefaultIdMaxLength)]
        [ForeignKey(nameof(Dish))]
        public string DishId { get; init; }

        public Dish Dish { get; init; }
    }
}
