using Common.Abstractions;

namespace Common.Responses;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public string? ErrorMessage { get; set; }

    public static ApiResponse<T> Ok(T data, string message = "", int statusCode = StatusCodes.Status200OK)
        => new()
        {
            Data = data,
            Success = true,
            StatusCode = statusCode,
            Message = message
        };

    public static ApiResponse<T> Fail(Error error, int statusCode = StatusCodes.Status400BadRequest)
        => new()
        {
            Success = false,
            StatusCode = statusCode,
            Message = error.errorName,
            ErrorMessage = error.errorDescription
        };
}

public class ApiResponse : ApiResponse<object>
{
    public static ApiResponse Ok(string message = "", int statusCode = StatusCodes.Status200OK)
        => new()
        {
            Success = true,
            StatusCode = statusCode,
            Message = message
        };
}