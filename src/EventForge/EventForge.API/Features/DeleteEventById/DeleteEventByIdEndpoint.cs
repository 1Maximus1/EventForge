namespace EventForge.API.Features.DeleteEventById;
public record DeleteEventResponse(bool IsSuccess);

public class DeleteEventByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/events/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteEventCommand(id));

            var response = result.Adapt<DeleteEventResponse>();

            return Results.Ok(response);
        })
        .WithName("DeleteEvent")
        .Produces<DeleteEventResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Event")
        .WithDescription("Delete event by its identifier.");
    }
}
