namespace FoodFun.Infrastructure.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Common.DataConstants;

    public class OrderDish
    {
        [ForeignKey(nameof(Order))]
        public int OrderId { get; init; }

        public Order Order { get; init; }

        [Required]
        [MaxLength(DefaultIdMaxLength)]
        [ForeignKey(nameof(Dish))]
        public string DishId { get; init; }

        public Dish Dish { get; init; }

        public ulong Quantity { get; init; }
    }
}
