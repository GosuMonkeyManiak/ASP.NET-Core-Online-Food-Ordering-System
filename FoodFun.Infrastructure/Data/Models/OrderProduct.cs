namespace FoodFun.Infrastructure.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Common.DataConstants;

    public class OrderProduct
    {
        [ForeignKey(nameof(Order))]
        public int OrderId { get; init; }

        public Order Order { get; init; }

        [Required]
        [MaxLength(DefaultIdMaxLength)]
        [ForeignKey(nameof(Product))]
        public string ProductId { get; init; }

        public Product Product { get; init; }
    }
}
