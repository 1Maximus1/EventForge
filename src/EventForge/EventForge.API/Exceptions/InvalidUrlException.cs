namespace EventForge.API.Exceptions;

public sealed class InvalidUrlException : DomainValidationException
{
    public InvalidUrlException(string target, string reason)
        : base($"{target} URL invalid: {reason}", code: $"{target.ToLower()}.url.invalid") { }
}
