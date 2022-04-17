namespace FoodFun.Infrastructure.Models
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
        public int TableSizeId { get; set; }

        public TableSize TableSize { get; init; }

        [ForeignKey(nameof(TablePosition))]
        public int TablePositionId { get; set; }

        public TablePosition TablePosition { get; init; }

        public ICollection<Reservation> Reservations { get; init; } = new HashSet<Reservation>();
    }
}
