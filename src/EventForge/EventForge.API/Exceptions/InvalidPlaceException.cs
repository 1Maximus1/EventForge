namespace EventForge.API.Exceptions;

public sealed class InvalidPlaceException : DomainValidationException
{
    public InvalidPlaceException(string reason)
        : base($"Invalid place: {reason}", "event.place.invalid") { }
}
