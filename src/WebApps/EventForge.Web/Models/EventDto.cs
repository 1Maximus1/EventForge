using System.Text.Json.Serialization;

namespace EventForge.Web.Models;

public sealed record EventDto(
    string Name,
    EventCategory Category,
    string Place,
    DateOnly Date,
    TimeOnly Time,
    string Description,
    string? AdditionalInfo,
    string? ImageUrl
);

public sealed record EventFullDto(
    Guid Id,
    string Name,
    EventCategory Category,
    string Place,
    DateOnly Date,
    TimeOnly Time,
    string Description,
    string? AdditionalInfo,
    string? ImageUrl
);

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EventCategory
{
    Other = 0,
    Dancing = 1,
    Sport = 2,
    Study = 3,
}

public class GetEventsResponse
{
    public List<EventFullDto> Events { get; set; } = [];
}

public sealed record CreateEventRequest(EventDto Event);
public record UpdateEventRequest(EventDto Event);
