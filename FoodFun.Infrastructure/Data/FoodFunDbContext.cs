namespace FoodFun.Infrastructure.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class FoodFunDbContext : IdentityDbContext<User>
    {
        public FoodFunDbContext(DbContextOptions<FoodFunDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Product> Products { get; init; }

        public DbSet<ProductCategory> ProductsCategories { get; init; }

        public DbSet<Dish> Dishes { get; init; }

        public DbSet<DishCategory> DishesCategories { get; init; }

        public DbSet<Order> Orders { get; init; }

        public DbSet<OrderProduct> OrdersProducts { get; init; }

        public DbSet<OrderDish> OrdersDishes { get; init; }

        public DbSet<Address> Addresses { get; init; }

        public DbSet<UserAddress> UsersAddresses { get; init; }

        public DbSet<Table> Tables { get; init; }

        public DbSet<TableSize> TableSizes { get; init; }

        public DbSet<TablePosition> TablePositions { get; init; }

        public DbSet<Reservation> Reservations { get; init; }

        public DbSet<ReservationDish> ReservationsDishes { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<OrderProduct>()
                .HasKey(k => new { k.OrderId, k.ProductId });

            builder.Entity<OrderDish>()
                .HasKey(k => new { k.OrderId, k.DishId });

            builder.Entity<ReservationDish>()
                .HasKey(k => new { k.ReservationId, k.DishId });

            builder.Entity<UserAddress>()
                .HasKey(k => new { k.UserId, k.AddressId });

            builder
                .Entity<ProductCategory>()
                .HasMany(x => x.Products)
                .WithOne(x => x.Category)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Product>()
                .HasMany(x => x.ProductInOrders)
                .WithOne(x => x.Product)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<DishCategory>()
                .HasMany(x => x.Dishes)
                .WithOne(x => x.Category)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Dish>()
                .HasMany(x => x.DishInOrders)
                .WithOne(x => x.Dish)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
