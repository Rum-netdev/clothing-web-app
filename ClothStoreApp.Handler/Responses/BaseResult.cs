namespace ClothStoreApp.Handler.Responses
{
    public class BaseResult
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }
    }

    public class BaseResult<T> : BaseResult
    {
        public T Data { get; set; }
    }
}
