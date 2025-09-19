namespace EventForge.API.Exceptions;

public sealed class EventNotFoundException : NotFoundException
{
    public EventNotFoundException(Guid id)
        : base($"Event with id '{id}' was not found", "event.notfound")
    {
    }
}
