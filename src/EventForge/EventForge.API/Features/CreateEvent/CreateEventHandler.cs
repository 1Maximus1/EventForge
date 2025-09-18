namespace EventForge.API.Features.CreateEvent;

public record CreateEventCommand(EventDto Event) : ICommand<CreateEventResult>;
public record CreateEventResult(Guid Id);

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Event)
            .NotNull().WithMessage("Event payload is required.")
            .SetValidator(new EventDtoValidator());
    }
}

public class CreateEventCommandHandler(IDocumentSession session) : ICommandHandler<CreateEventCommand, CreateEventResult>
{
    public async Task<CreateEventResult> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var eventRequest = request.Event;

        var id = ValueObjects.EventId.Of(Guid.NewGuid());
        var name = EventName.Of(eventRequest.Name);
        var place = Place.Of(eventRequest.Place);
        var schedule = EventSchedule.Of(eventRequest.Date, eventRequest.Time);
        var imageUrl = string.IsNullOrWhiteSpace(eventRequest.ImageUrl) ? null : ImageUrl.Of(eventRequest.ImageUrl);

        var createdEvent = Event.Create(
            id,
            name,
            eventRequest.Category,
            place,
            schedule,
            eventRequest.Description,
            eventRequest.AdditionalInfo,
            imageUrl
        );

        session.Store(createdEvent);
        await session.SaveChangesAsync(cancellationToken);

        return new CreateEventResult(id.Value);
    }
}
