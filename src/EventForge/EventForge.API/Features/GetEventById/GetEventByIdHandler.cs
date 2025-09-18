namespace EventForge.API.Features.GetEventById;

public sealed record GetEventByIdQuery(Guid Id) : IQuery<GetEventByIdResult>;
public sealed record GetEventByIdResult(EventDto Event);


public class GetEventByIdQueryHandler(IDocumentSession session)
    : IQueryHandler<GetEventByIdQuery, GetEventByIdResult>
{
    public async Task<GetEventByIdResult> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await session.LoadAsync<Event>(request.Id, cancellationToken);

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

