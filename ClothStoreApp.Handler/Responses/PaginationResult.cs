namespace ClothStoreApp.Handler.Responses
{
    public class PaginationResult<T> : BaseResult<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public PaginationResult(T data, int pageNumber, int pageSize)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
