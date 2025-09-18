using System.Net;

namespace BuildingBlocks.Exceptions;

public abstract class AppException : Exception
{
    public string? ErrorCode
    {
        get;
    }
    public HttpStatusCode StatusCode
    {
        get;
    }
    protected AppException(string message, HttpStatusCode statusCode, string? errorCode = null, Exception? inner = null)
        : base(message, inner)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }
}

