namespace EventForge.API.Features.UpdateEventById;

public sealed record UpdateEventCommand(EventDto Event, Guid Id)
    : ICommand<UpdateEventResult>;
public sealed record UpdateEventResult(bool IsSuccess);


public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
{
    public UpdateEventCommandValidator()
    {
        RuleFor(x => x.Event)
            .NotNull().WithMessage("Event payload is required.")
            .SetValidator(new EventDtoValidator());

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Event Id is required.");
    }
}


public class UpdateEventByIdCommandHandler(IDocumentSession session) : ICommandHandler<UpdateEventCommand, UpdateEventResult>
{
    public async Task<UpdateEventResult> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var eventRequestId = request.Id;
        var evenRequest = request.Event;

        var entity = await session.LoadAsync<Event>(eventRequestId, cancellationToken);
        if (entity is null)
            throw new EventNotFoundException(eventRequestId);

        entity.Update(
            EventName.Of(evenRequest.Name),
            evenRequest.Category,
            Place.Of(evenRequest.Place),
            EventSchedule.Of(evenRequest.Date, evenRequest.Time),
            evenRequest.Description,
            evenRequest.AdditionalInfo,
            string.IsNullOrWhiteSpace(evenRequest.ImageUrl) ? null : ImageUrl.Of(evenRequest.ImageUrl)
        );

        session.Store(entity);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdateEventResult(true);
    }
}
