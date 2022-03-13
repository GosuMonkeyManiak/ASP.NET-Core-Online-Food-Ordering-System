namespace FoodFun.Core.Extensions
{
    using System.Collections;
    using global::AutoMapper;

    public static class EnumerableAutoMapperExtension
    {
        public static IEnumerable<TDestination> ProjectTo<TDestination>(
            this IEnumerable collection,
            IMapper mapper)
            where TDestination : class
        {
            var projectedElements = new List<TDestination>();

            foreach (var element in collection)
            {
                projectedElements.Add(mapper.Map<TDestination>(element));
            }

            return projectedElements;
        }
    }
}
