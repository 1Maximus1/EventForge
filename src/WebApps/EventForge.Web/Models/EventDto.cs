namespace EventForge.Web.Models;

public sealed record EventDto(
    Guid? Id,
    string Name,
    EventCategory Category,
    string Place,
    DateOnly Date,
    TimeOnly Time,
    string Description,
    string? AdditionalInfo,
    string? ImageUrl
);

public enum EventCategory
{
    Other = 0,
    Dancing = 1,
    Sport = 2,
    Study = 3,
}
