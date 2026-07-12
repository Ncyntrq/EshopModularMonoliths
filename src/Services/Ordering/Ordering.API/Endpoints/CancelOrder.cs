using Ordering.Application.Orders.Commands.CancelOrder;

namespace Ordering.API.Endpoints;

public record CancelOrderResponse(bool IsSuccess);

public class CancelOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/orders/{id}/cancel", async (Guid id, ISender sender) =>
        {
            var command = new CancelOrderCommand(id);

            var result = await sender.Send(command);

            var response = result.Adapt<CancelOrderResponse>();

            return Results.Ok(response);
        })
        .WithName("CancelOrder")
        .Produces<CancelOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Cancel Order")
        .WithDescription("Cancel Order");
    }
}
