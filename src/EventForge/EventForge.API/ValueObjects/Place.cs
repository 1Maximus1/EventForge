namespace EventForge.API.ValueObjects;

public sealed record Place
{
    public string Value
    {
        get;
    }

    private Place(string value) => Value = value;

    public static Place Of(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidPlaceException("empty or whitespace");

        var trimmed = value.Trim();

        if (trimmed.Length < 3)
            throw new InvalidPlaceException("must be at least 3 characters long");

        if (trimmed.Length > 200)
            throw new InvalidPlaceException("must not exceed 200 characters");

        return new Place(trimmed);
    }
}
