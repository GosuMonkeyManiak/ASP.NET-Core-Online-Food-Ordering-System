namespace FoodFun.Infrastructure.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Common.DataConstants.DishCategory;

    public class DishCategory
    {
        [Key]
        public int Id { get; init; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        public bool IsDisable { get; set; }

        public ICollection<Dish> Dishes { get; init; } = new HashSet<Dish>();
    }
}
