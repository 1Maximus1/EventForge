namespace EventForge.API.Exceptions;

public sealed class InvalidEventScheduleException : DomainValidationException
{
    public InvalidEventScheduleException(string reason)
        : base($"Invalid event schedule: {reason}", "event.schedule.invalid") { }
}
