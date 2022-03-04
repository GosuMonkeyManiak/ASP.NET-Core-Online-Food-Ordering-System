namespace FoodFun.Infrastructure.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public ICollection<UserAddress> UserAddresses { get; init; } = new HashSet<UserAddress>();

        public ICollection<Order> Orders { get; init; } = new HashSet<Order>();

        public ICollection<Reservation> Reservations { get; init; } = new HashSet<Reservation>();
    }
}
