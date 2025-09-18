using System.Net;

namespace BuildingBlocks.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string message, string? code = null)
        : base(message, statusCode: HttpStatusCode.NotFound, errorCode: code) { }
}

