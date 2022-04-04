namespace FoodFun.Core.Extensions
{
    using Microsoft.EntityFrameworkCore;

    public static class DbContextExtensions
    {
        public static bool IsAttach(this DbContext context, object entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var entry = context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                return false;
            }

            return true;
        }
    }
}
