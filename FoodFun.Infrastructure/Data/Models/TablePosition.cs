namespace FoodFun.Infrastructure.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Common.DataConstants.TablePosition;

    public class TablePosition
    {
        [Key]
        public int Id { get; init; }

        [Required]
        [MaxLength(PositionMaxLength)]
        public string Position { get; init; }

        public ICollection<Table> Tables { get; init; }
    }
}
