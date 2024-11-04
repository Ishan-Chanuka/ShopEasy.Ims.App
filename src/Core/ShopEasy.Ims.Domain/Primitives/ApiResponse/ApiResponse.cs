namespace ShopEasy.Ims.Domain.Primitives.ApiResponse
{
    public class ApiResponse<T>
    {
        public ApiResponse() { }

        public ApiResponse(int statusCode, string message, bool isSuccess)
        {
            StatusCode = statusCode;
            Message = message;
            IsSuccess = isSuccess;
        }

        public ApiResponse(int statusCode, string message, bool isSuccess, T data)
        {
            StatusCode = statusCode;
            Message = message;
            IsSuccess = isSuccess;
            Data = data;
        }

        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
    }
}
