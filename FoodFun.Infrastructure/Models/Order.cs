namespace FoodFun.Infrastructure.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Common.DataConstants;

    public class Order
    {
        [Key]
        public int Id { get; init; }

        [Required]
        [MaxLength(DefaultIdMaxLength)]
        [ForeignKey(nameof(User))]
        public string UserId { get; init; }

        public User User { get; init; }

        public decimal Price { get; init; }

        public bool IsSent { get; set; }

        public bool IsDelivered { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; init; } = new HashSet<OrderProduct>();

        public ICollection<OrderDish> OrderDishes { get; init; } = new HashSet<OrderDish>();
    }
}
