using EventForge.API.Data;

namespace EventForge.API.Features.GetEventById;

public sealed record GetEventByIdQuery(Guid Id) : IQuery<GetEventByIdResult>;
public sealed record GetEventByIdResult(EventDto Event);


public class GetEventByIdQueryHandler(ApplicationDbContext dbContext)
    : IQueryHandler<GetEventByIdQuery, GetEventByIdResult>
{
    public async Task<GetEventByIdResult> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var id = ValueObjects.EventId.Of(request.Id);

        var entity = await dbContext.Events
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (entity is null)
            throw new EventNotFoundException(request.Id);

        var dto = new EventDto(
            entity.Name.Value,
            entity.Category,
            entity.Place.Value,
            entity.Schedule.Date,
            entity.Schedule.Time,
            entity.Description,
            entity.AdditionalInfo,
            entity.ImageUrl?.Value
        );

        return new GetEventByIdResult(dto);
    }
}

