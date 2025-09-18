namespace EventForge.API.Features.UpdateEventById;

public sealed record UpdateEventRequest(EventDto Event);
public sealed record UpdateEventResponse(bool IsSuccess);

public class UpdateEventByIdEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/events/{id:guid}",
            async (Guid id, UpdateEventRequest request, ISender sender) =>
            {
                var command = new UpdateEventCommand(request.Event, id);

                var result = await sender.Send(command);

                var response = result.Adapt<UpdateEventResponse>();

                return Results.Ok(response);
            })
            .WithName("UpdateEventById")
            .Produces<UpdateEventResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Event")
            .WithDescription("Update an event by its unique identifier.");
    }
}
