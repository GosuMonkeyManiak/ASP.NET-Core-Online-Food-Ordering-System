namespace FoodFun.Infrastructure.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Common.DataConstants.ProductCategory;

    public class ProductCategory
    {
        [Key]
        public int Id { get; init; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; init; }

        public ICollection<Product> Products { get; init; } = new HashSet<Product>();
    }
}
