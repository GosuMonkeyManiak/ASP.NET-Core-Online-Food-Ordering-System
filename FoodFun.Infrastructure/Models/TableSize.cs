namespace FoodFun.Infrastructure.Models
{
    using System.ComponentModel.DataAnnotations;

    public class TableSize
    {
        [Key]
        public int Id { get; init; }

        public int Seats { get; init; }

        public ICollection<Table> Tables { get; init; }
    }
}
