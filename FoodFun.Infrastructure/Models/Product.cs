namespace FoodFun.Infrastructure.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Common.DataConstants;
    using static Common.DataConstants.Product;

    public class Product
    {
        [Key]
        [MaxLength(DefaultIdMaxLength)]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; init; }

        [Required]
        [MaxLength(UrlMaxLength)]
        public string ImageUrl { get; init; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; init; }

        public ProductCategory Category { get; init; }

        public decimal Price { get; init; }

        [Required]
        public string Description { get; init; }

        public long Quantity { get; init; }

        public ICollection<OrderProduct> ProductInOrders { get; init; } = new HashSet<OrderProduct>();
    }
}
