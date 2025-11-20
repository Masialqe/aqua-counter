using Common.Abstractions;

namespace Common.Responses;

public class ApiResponse
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors = [];
    public string TraceId { get; set; } = Guid.NewGuid().ToString();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ApiResponse Fail(string message, List<string>? errors = null, int statusCode = StatusCodes.Status400BadRequest)
        => new()
        {
            Success = false,
            StatusCode = statusCode,
            Message = message,
            Errors = errors ?? new()
        };

    public static ApiResponse ServerError(string message = "Internal server error", List<string>? errors = null)
        => new()
        {
            Success = false,
            StatusCode = StatusCodes.Status500InternalServerError,
            Message = message,
            Errors = errors
        };

    public static ApiResponse Ok(string message = "Success", int statusCode = StatusCodes.Status200OK)
       => new()
       {
           Success = true,
           StatusCode = statusCode,
           Message = message
       };
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }

    public static ApiResponse<T> Ok(T data, string message = "Success", int statusCode = StatusCodes.Status200OK)
        => new()
        {
            Success = true,
            StatusCode = statusCode,
            Message = message,
            Data = data
        };

    public static ApiResponse<T> Created(T data, string message = "Created successfully", int statusCode = StatusCodes.Status201Created)
        => new()
        {
            Success = true,
            StatusCode = statusCode,
            Message = message,
            Data = data
        };
}


public static class ApiResult
{
    public static IResult Ok<T>(T data, string? message = null)
        => Results.Json(ApiResponse<T>.Ok(data, message ?? "Success"));

    public static IResult Ok(string? message = null)
        => Results.Json(ApiResponse.Ok(message ?? "Success"));

    public static IResult NoContent(string? message = null)
        => Results.Json(ApiResponse.Ok(message ?? "Operation has been procceded.", StatusCodes.Status204NoContent));

    public static IResult Created<T>(string uri, T data, string? message = null)
        => Results.Json(ApiResponse<T>.Created(data, message ?? "Created successfully"), statusCode: StatusCodes.Status201Created);

    public static IResult Unauthorized(List<string> errors, string? message = null)
        => Results.Json(ApiResponse.Fail(message ?? "User not authorized.", errors, StatusCodes.Status401Unauthorized), statusCode: StatusCodes.Status401Unauthorized);

    public static IResult BadRequest(List<string> errors, string? message = null)
        => Results.Json(ApiResponse.Fail(message ?? "Bad request.", errors, StatusCodes.Status400BadRequest), statusCode: StatusCodes.Status400BadRequest);

    public static List<string> ToErrorList(this Error error)
    => new() { error.errorDescription! };

    public static List<string> ToErrorList(this List<Error> errors)
        => errors.Select(e => e.errorDescription!).ToList();

}