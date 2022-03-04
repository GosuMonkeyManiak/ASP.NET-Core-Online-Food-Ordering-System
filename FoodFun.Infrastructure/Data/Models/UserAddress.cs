namespace FoodFun.Infrastructure.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Common.DataConstants;

    public class UserAddress
    {
        [Required]
        [MaxLength(DefaultIdMaxLength)]
        [ForeignKey(nameof(User))]
        public string UserId { get; init; }

        public User User { get; init; }

        [ForeignKey(nameof(Address))]
        public int AddressId { get; init; }

        public Address Address { get; init; }
    }
}
