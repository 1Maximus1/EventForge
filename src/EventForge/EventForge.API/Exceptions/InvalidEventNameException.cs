namespace EventForge.API.Exceptions;

public sealed class InvalidEventNameException : DomainValidationException
{
    public InvalidEventNameException(string reason)
        : base($"Invalid event name: {reason}", "event.name.invalid") { }
}

