namespace Common.Kernel.Response.Pagination
{
    public class PaginationResponse<TResponse> : PaginationResponseBase
    {
        public IList<TResponse> Results { get; set; } = new List<TResponse>();
    }
}
