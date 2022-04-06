namespace FoodFun.Infrastructure.Models
{
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public ICollection<Order> Orders { get; init; } = new HashSet<Order>();

        public ICollection<Reservation> Reservations { get; init; } = new HashSet<Reservation>();
    }
}
