namespace FoodFun.Core.Services.Base
{
    using Contracts;
    using Infrastructure.Common.Contracts.Base;

    public abstract class BaseItemService
    {
        protected int LastPageNumber { get; set; }

        protected async Task<Tuple<string, int, byte, int>> ValidateAndSetDefaultSearchParameters(
            string searchTerm,
            int categoryFilterId,
            byte orderNumber,
            int pageNumber,
            int pageSize,
            bool onlyAvailable,
            ICategory category,
            IBaseItemRepository baseItemRepository)
        {
            string searchTermResult = null;
            int categoryFilterIdResult = 0;
            byte orderNumberResult = 0;
            int pageNumberResult = 1;

            if (searchTerm != null)
            {
                searchTermResult = searchTerm;
            }

            if (await category.IsCategoryExist(categoryFilterId))
            {
                categoryFilterIdResult = categoryFilterId;
            }

            if (orderNumber == 1)
            {
                orderNumberResult = orderNumber;
            }

            await PopulateLastPageNumberByFilter(
                searchTermResult,
                categoryFilterIdResult,
                pageSize,
                onlyAvailable,
                baseItemRepository);

            if (pageNumber > 0
                && pageNumber <= this.LastPageNumber)
            {
                pageNumberResult = pageNumber;
            }

            return new(
                searchTermResult,
                categoryFilterIdResult,
                orderNumberResult,
                pageNumberResult);
        }

        private async Task PopulateLastPageNumberByFilter(
            string searchTerm,
            int categoryFilterId,
            int pageSize,
            bool onlyAvailable,
            IBaseItemRepository baseItemRepository)
        {
            var countOfProductsByFilters = await baseItemRepository
                .GetCountOfItemsByFilters(
                    searchTerm,
                    categoryFilterId,
                    onlyAvailable);

            this.LastPageNumber = (int)Math.Ceiling(countOfProductsByFilters / (pageSize * 1.0));
        }
    }
}
