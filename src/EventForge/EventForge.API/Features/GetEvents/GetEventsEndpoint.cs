namespace EventForge.API.Features.GetEvents;

public sealed record GetEventsResponse(IEnumerable<EventFullDto> Events);

public class GetEventsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/events", async (ISender sender) =>
        {
            var result = await sender.Send(new GetEventsQuery());

            var response = result.Adapt<GetEventsResponse>();

            return Results.Ok(response);
        })
        .WithName("GetEvents")
        .Produces<GetEventsResponse>(StatusCodes.Status200OK)
        .WithSummary("Get all events")
        .WithDescription("Retrieve a list of all events.");
    }
}
