namespace FoodFun.Infrastructure.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Common.DataConstants;
    using static Common.DataConstants.Dish;

    public class Dish
    {
        [Key]
        [MaxLength(DefaultIdMaxLength)]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(UrlMaxLength)]
        public string ImageUrl { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public DishCategory Category { get; init; }

        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        public ulong Quantity { get; set; }

        public ICollection<OrderDish> DishInOrders { get; init; } = new HashSet<OrderDish>();
    }
}
