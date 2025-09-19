namespace EventForge.API.ValueObjects;

public sealed record EventSchedule
{
    public DateOnly Date
    {
        get;
    }
    public TimeOnly Time
    {
        get;
    }
    public DateTimeOffset StartAt =>
        new DateTimeOffset(Date.ToDateTime(Time), TimeSpan.Zero);

    private EventSchedule(DateOnly date, TimeOnly time)
    {
        Date = date;
        Time = time;
    }

    public static EventSchedule Of(DateOnly date, TimeOnly time)
    {
        if (date == default)
            throw new InvalidEventScheduleException("date is required");

        if (time == default)
            throw new InvalidEventScheduleException("time is required");

        return new EventSchedule(date, time);
    }
}

