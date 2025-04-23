namespace JorgeLanches.Pagination
{
    public class PaginationParameters
    {
        const int MAX_PAGESIZE = 50;

        public int Pagenumber { get; set; } = 1;
        private int _pageSize = 25;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > MAX_PAGESIZE) ? MAX_PAGESIZE : value;
            }
        }
    }
}
