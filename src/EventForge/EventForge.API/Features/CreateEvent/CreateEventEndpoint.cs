namespace EventForge.API.Features.CreateEvent;

public record CreateEventRequest(EventDto Event);
public record CreateEventResponse(Guid Id);

public class CreateEventEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/events", async (CreateEventRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateEventCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<CreateEventResponse>();

            return Results.Created($"/api/v1/events/{response.Id}", response);
        })
        .WithName("CreateEvent")
        .Produces<CreateEventResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create a new event")
        .WithDescription("Creates an event with required fields and returns its identifier.");
    }
}
