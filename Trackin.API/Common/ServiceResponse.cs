namespace Trackin.API.Common
{
    public class ServiceResponse<T> where T : class
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; }
        public ServiceResponse() { }
        public ServiceResponse(bool success, string message, T data, int statusCode)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
