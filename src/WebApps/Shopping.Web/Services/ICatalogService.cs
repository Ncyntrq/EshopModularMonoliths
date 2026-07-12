namespace Shopping.Web.Services;

public interface ICatalogService
{
    [Get("/catalog-service/products?pageNumber={pageNumber}&pageSize={pageSize}")]
    Task<GetProductsResponse> GetProducts(int? pageNumber = 1, int? pageSize = 10);
    
    [Get("/catalog-service/products/{id}")]
    Task<GetProductByIdResponse> GetProduct(Guid id);
    
    [Get("/catalog-service/products/category/{category}")]
    Task<GetProductByCategoryResponse> GetProductsByCategory(string category);

    [Get("/catalog-service/products/search")]
    Task<SearchProductsResponse> SearchProducts(string? name = null, decimal? minPrice = null, decimal? maxPrice = null, int? pageNumber = 1, int? pageSize = 10);
}
