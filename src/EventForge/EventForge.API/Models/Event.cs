namespace EventForge.API.Models;

public class Event : Entity<ValueObjects.EventId>
{
    public EventName Name
    {
        get; private set;
    } = default!;
    public EventCategory Category
    {
        get; private set;
    } = EventCategory.Other!;
    public Place Place
    {
        get; private set;
    } = default!;
    public EventSchedule Schedule
    {
        get; private set;
    } = default!;
    public string Description
    {
        get; private set;
    } = default!;
    public string? AdditionalInfo
    {
        get; private set;
    } = default!;
    public ImageUrl? ImageUrl
    {
        get; private set;
    } = default!;

    public Event()
    {
    }

    public static Event Create(
        ValueObjects.EventId id,
        EventName name,
        EventCategory category,
        Place place,
        EventSchedule schedule,
        string description,
        string? additionalInfo,
        ImageUrl? imageUrl)
    {
        var ev = new Event
        {
            Id = id,
            Name = name,
            Category = category,
            Place = place,
            Schedule = schedule,
            ImageUrl = imageUrl
        };

        ev.SetDescription(description);
        ev.SetAdditionalInfo(additionalInfo);

        return ev;
    }

    public void Update(
        EventName name,
        EventCategory category,
        Place place,
        EventSchedule schedule,
        string description,
        string? additionalInfo,
        ImageUrl? imageUrl)
    {
        Name = name;
        Category = category;
        Place = place;
        Schedule = schedule;
        SetDescription(description);
        SetAdditionalInfo(additionalInfo);
        ImageUrl = imageUrl;
    }
    private void SetDescription(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainValidationException("Description is required", "event.description.required");

        var trimmed = value.Trim();
        if (trimmed.Length > 1000)
            throw new DomainValidationException("Description must be ≤ 1000 chars", "event.description.length");

        Description = trimmed;
    }

    private void SetAdditionalInfo(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            AdditionalInfo = null;
            return;
        }

        var trimmed = value.Trim();
        if (trimmed.Length > 500)
            throw new DomainValidationException("AdditionalInfo must be ≤ 500 chars", "event.additionalinfo.length");

        AdditionalInfo = trimmed;
    }
}

