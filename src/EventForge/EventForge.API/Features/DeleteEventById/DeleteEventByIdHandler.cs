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

public class DeleteEventByIdCommandHandler(IDocumentSession session) : ICommandHandler<DeleteEventCommand, DeleteEventResult>
{
    public async Task<DeleteEventResult> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var entity = await session.LoadAsync<Event>(request.Id, cancellationToken);

        if (entity is null)
        {
            throw new EventNotFoundException(request.Id);
        }

        session.Delete(entity);
        await session.SaveChangesAsync(cancellationToken);

        return new DeleteEventResult(true);
    }
}
