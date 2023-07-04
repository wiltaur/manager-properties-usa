namespace manager_properties_usa.Utilities
{
    /// <summary>
    /// Generic responses for all requests.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T>
    {
        public ApiResponse(T data)
        {
            Data = data;
            IsSuccess = true;
            ReturnMessage = "";
        }
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string ReturnMessage { get; set; }
    }
}