namespace EventForge.API.ValueObjects;

public sealed record EventId
{
    public Guid Value
    {
        get;
    }

    private EventId(Guid value) => Value = value;

    public static EventId Of(Guid? value)
    {
        if (value is null)
            throw new InvalidEntityIdException("Event", "null");

        if (value == Guid.Empty)
            throw new InvalidEntityIdException("Event", "empty");

        return new EventId(value.Value);
    }
}

