namespace EventForge.API.ValueObjects;

public sealed class ImageUrl
{
    public string Value
    {
        get;
    }
    private ImageUrl(string value) => Value = value;

    public static ImageUrl Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidUrlException("Image", "empty");
        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            throw new InvalidUrlException("Image", "must be absolute http/https");
        return new ImageUrl(uri.ToString());
    }
}
