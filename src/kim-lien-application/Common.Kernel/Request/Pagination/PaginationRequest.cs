namespace Common.Kernel.Request.Pagination
{
    public class PaginationRequest
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int? Skip { get; set; }
        public List<string>? SortField { get; set; }
        public bool? Asc { get; set; }
    }
    public class PaginationRequest<TFilter> : PaginationRequest
        where TFilter : class
    {
        public TFilter? Filter { get; set; }
    }
}
