using System.Net;

namespace BuildingBlocks.Exceptions;

public class DomainValidationException : AppException
{
    public DomainValidationException(string message, string? code = null)
        : base(message, statusCode: HttpStatusCode.UnprocessableContent, errorCode: code) { }
}
