namespace EventForge.API.ValueObjects;

public sealed record EventName
{
    // Consider adding default length constant if needed in future
    //private const int DefaultLength = 5;
    public string Value
    {
        get;
    }

    private EventName(string value) => Value = value;

    public static EventName Of(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidEventNameException("empty or whitespace");

        var trimmed = value.Trim();
        if (trimmed.Length < 3)
            throw new InvalidEventNameException("min length is 3");
        if (trimmed.Length > 100)
            throw new InvalidEventNameException("max length is 100");

        return new EventName(trimmed);
    }
}

