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
        public string Name { get; set; }

        [Required]
        [MaxLength(UrlMaxLength)]
        public string ImageUrl { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public ProductCategory Category { get; set; }

        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        public long Quantity { get; set; }

        public ICollection<OrderProduct> ProductInOrders { get; init; } = new HashSet<OrderProduct>();
    }
}
