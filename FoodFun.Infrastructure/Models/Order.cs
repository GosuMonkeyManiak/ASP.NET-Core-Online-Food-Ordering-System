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

        [ForeignKey(nameof(Address))]
        public int AddressId { get; init; }

        public Address Address { get; init; }

        public decimal Price { get; init; }

        public bool IsSent { get; init; }

        public bool IsDelivered { get; init; }

        public ICollection<OrderProduct> OrderProducts { get; init; } = new HashSet<OrderProduct>();

        public ICollection<OrderDish> OrderDishes { get; init; } = new HashSet<OrderDish>();
    }
}
