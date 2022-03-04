namespace FoodFun.Infrastructure.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Common.DataConstants.Address;

    public class Address
    {
        [Key]
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; init; }

        public ICollection<UserAddress> Users { get; init; } = new HashSet<UserAddress>();

        public ICollection<Order> Orders { get; init; } = new HashSet<Order>();
    }
}
