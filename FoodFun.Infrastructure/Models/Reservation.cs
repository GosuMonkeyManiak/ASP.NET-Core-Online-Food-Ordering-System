namespace FoodFun.Infrastructure.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static Common.DataConstants;

    public class Reservation
    {
        [Key]
        public int Id { get; init; }

        [Required]
        [MaxLength(DefaultIdMaxLength)]
        [ForeignKey(nameof(Table))]
        public string TableId { get; init; }

        public Table Table { get; init; }

        [Required]
        [MaxLength(DefaultIdMaxLength)]
        [ForeignKey(nameof(User))]
        public string UserId { get; init; }

        public User User { get; init; }

        public DateTime Date { get; init; }

        public decimal Price { get; init; }

        public ICollection<ReservationDish> ReservationDishes { get; init; } = new HashSet<ReservationDish>();
    }
}
