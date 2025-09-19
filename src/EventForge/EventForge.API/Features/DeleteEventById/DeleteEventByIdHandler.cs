using EventForge.API.Data;

namespace EventForge.API.Features.DeleteEventById;

public record DeleteEventCommand(Guid Id) : ICommand<DeleteEventResult>;
public record DeleteEventResult(bool IsSuccess);

public class DeleteEventCommandValidator : AbstractValidator<DeleteEventCommand>
{
    public DeleteEventCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("EventId is required");
    }
}

public class DeleteEventByIdCommandHandler(ApplicationDbContext dbContext) : ICommandHandler<DeleteEventCommand, DeleteEventResult>
{
    public async Task<DeleteEventResult> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var id = ValueObjects.EventId.Of(request.Id);

        var entity = await dbContext.Events
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (entity is null)
        {
            throw new EventNotFoundException(request.Id);
        }

        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteEventResult(true);
    }
}
