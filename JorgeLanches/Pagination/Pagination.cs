namespace JorgeLanches.Pagination
{
    public static class Pagination<T> where T : class
    {
        public static IQueryable<T> FilterPages(IQueryable<T> items, PaginationParameters parameters)
        {
            return items.Skip((parameters.Pagenumber - 1) * parameters.PageSize)
                        .Take(parameters.PageSize);
        }
    }
}
