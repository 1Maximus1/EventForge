namespace EventForge.API.Dtos;

public sealed record EventDto
(
    string Name,
    EventCategory Category,
    string Place,
    DateOnly Date,
    TimeOnly Time,
    string Description,
    string? AdditionalInfo,
    string? ImageUrl
);
