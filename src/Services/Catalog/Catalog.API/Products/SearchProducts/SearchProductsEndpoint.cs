namespace Catalog.API.Products.SearchProducts;

public record SearchProductsRequest(string? Name = null, decimal? MinPrice = null, decimal? MaxPrice = null, int? PageNumber = 1, int? PageSize = 10);
public record SearchProductsResponse(IEnumerable<Product> Products);

public class SearchProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/search", async ([AsParameters] SearchProductsRequest request, ISender sender) =>
        {
            var query = request.Adapt<SearchProductsQuery>();

            var result = await sender.Send(query);

            var response = result.Adapt<SearchProductsResponse>();

            return Results.Ok(response);
        })
        .WithName("SearchProducts")
        .Produces<SearchProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Search Products")
        .WithDescription("Search Products by name and price range");
    }
}
