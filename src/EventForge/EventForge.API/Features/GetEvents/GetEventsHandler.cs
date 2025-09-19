namespace EventForge.API.Features.GetEvents;

public sealed record GetEventsQuery() : IQuery<GetEventsResult>;
public sealed record GetEventsResult(IEnumerable<EventFullDto> Events);

public class GetEventsQueryHandler(ApplicationDbContext dbContext)
    : IQueryHandler<GetEventsQuery, GetEventsResult>
{
    public async Task<GetEventsResult> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        var items = await dbContext.Events
            .Select(e => new EventFullDto(
                e.Id.Value,
                e.Name.Value,
                e.Category,
                e.Place.Value,
                e.Schedule.Date,
                e.Schedule.Time,
                e.Description,
                e.AdditionalInfo,
                e.ImageUrl != null ? e.ImageUrl.Value : null))
            .ToListAsync(cancellationToken);

        return new GetEventsResult(items);
    }
}