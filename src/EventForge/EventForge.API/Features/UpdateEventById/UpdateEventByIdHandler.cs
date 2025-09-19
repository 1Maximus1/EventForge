using EventForge.API.Data;

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


public class UpdateEventByIdCommandHandler(ApplicationDbContext dbContext) : ICommandHandler<UpdateEventCommand, UpdateEventResult>
{
    public async Task<UpdateEventResult> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var evenRequest = request.Event;
        var id = ValueObjects.EventId.Of(request.Id);

        var entity = await dbContext.Events
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (entity is null)
            throw new EventNotFoundException(id.Value);

        entity.Update(
            EventName.Of(evenRequest.Name),
            evenRequest.Category,
            Place.Of(evenRequest.Place),
            EventSchedule.Of(evenRequest.Date, evenRequest.Time),
            evenRequest.Description,
            evenRequest.AdditionalInfo,
            string.IsNullOrWhiteSpace(evenRequest.ImageUrl) ? null : ImageUrl.Of(evenRequest.ImageUrl)
        );

        dbContext.Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateEventResult(true);
    }
}
