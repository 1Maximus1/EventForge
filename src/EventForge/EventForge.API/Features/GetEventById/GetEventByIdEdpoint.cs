namespace EventForge.API.Features.GetEventById;

public sealed record GetEventByIdResponse(EventDto Event);

public class GetEventByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/events/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetEventByIdQuery(id));

            var response = result.Adapt<GetEventByIdResponse>();

            return Results.Ok(response);
        })
        .WithName("GetEventById")
        .Produces<GetEventByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Event By Id")
        .WithDescription("Get an event by its unique identifier.");
    }
}

