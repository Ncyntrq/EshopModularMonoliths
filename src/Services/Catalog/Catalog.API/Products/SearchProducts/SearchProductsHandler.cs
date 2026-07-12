namespace Catalog.API.Products.SearchProducts;

public record SearchProductsQuery(string? Name = null, decimal? MinPrice = null, decimal? MaxPrice = null, int? PageNumber = 1, int? PageSize = 10) : IQuery<SearchProductsResult>;
public record SearchProductsResult(IEnumerable<Product> Products);

internal class SearchProductsQueryHandler
    (IDocumentSession session)
    : IQueryHandler<SearchProductsQuery, SearchProductsResult>
{
    public async Task<SearchProductsResult> Handle(SearchProductsQuery query, CancellationToken cancellationToken)
    {
        var dbQuery = session.Query<Product>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            // Use case-insensitive search
            dbQuery = dbQuery.Where(p => p.Name.Contains(query.Name, StringComparison.OrdinalIgnoreCase));
        }

        if (query.MinPrice.HasValue)
        {
            dbQuery = dbQuery.Where(p => p.Price >= query.MinPrice.Value);
        }

        if (query.MaxPrice.HasValue)
        {
            dbQuery = dbQuery.Where(p => p.Price <= query.MaxPrice.Value);
        }

        var products = await dbQuery
            .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

        return new SearchProductsResult(products);
    }
}
