namespace realtorAPI.DTOs
{
    public class ApiResponse<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();
        public T? Data { get; set; }

        public static ApiResponse<T> Success(T data, string message = "Operation successful")
        {
            return new ApiResponse<T>
            {
                Succeeded = true,
                Message = message,
                Errors = new List<string>(),
                Data = data
            };
        }

        public static ApiResponse<T> Error(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Succeeded = false,
                Message = message,
                Errors = errors ?? new List<string>(),
                Data = default(T)
            };
        }

        public static ApiResponse<T> Error(string message, string error)
        {
            return new ApiResponse<T>
            {
                Succeeded = false,
                Message = message,
                Errors = new List<string> { error },
                Data = default(T)
            };
        }
    }
}

