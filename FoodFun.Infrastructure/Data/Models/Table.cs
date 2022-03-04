namespace FoodFun.Infrastructure.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Common.DataConstants;

    public class Table
    {
        [Key]
        [MaxLength(DefaultIdMaxLength)]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(TableSize))]
        public int TableSizeId { get; init; }

        public TableSize TableSize { get; init; }

        [ForeignKey(nameof(TablePosition))]
        public int TablePositionId { get; init; }

        public TablePosition TablePosition { get; init; }

        public ICollection<Reservation> Reservations { get; init; } = new HashSet<Reservation>();
    }
}
