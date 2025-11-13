namespace Common.Kernel.Response.Pagination
{
    public abstract class PaginationResponseBase
    {
        public int CurrentPage { get; set; }

        public int PageCount
        {
            get
            {
                if (PageSize == 0) return 1;

                var pageCount = (double)RowCount / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
        }

        public int PageSize { get; set; }

        public int RowCount { get; set; }

        public int FirstRowOnPage
        {
            get
            {
                return CurrentPage * PageSize + 1;
            }
        }

        public int LastRowOnPage
        {
            get
            {
                return Math.Min((CurrentPage + 1) * PageSize, RowCount);
            }
        }
    }
}
