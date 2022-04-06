namespace FoodFun.Web.Models.Product
{
    using System.ComponentModel.DataAnnotations;
    using Core.ValidationAttributes.Product;

    using static Constants.GlobalConstants.Messages;

    public class ProductListingModel
    {
        [Required]
        [MustBeExistingProduct]
        [MustBeInActiveProductCategory]
        public string Id { get; init; }

        public string Name { get; init; }

        public string ImageUrl { get; init; }

        public string CategoryTitle { get; init; }
       
        public decimal Price { get; init; }
      
        public string Description { get; init; }

        [Range(
            1,
            ulong.MaxValue,
            ErrorMessage = QuantityError)]
        public ulong Quantity { get; set; }
    }
}
