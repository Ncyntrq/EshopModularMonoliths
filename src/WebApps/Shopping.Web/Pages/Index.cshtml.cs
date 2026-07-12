namespace Shopping.Web.Pages;
public class IndexModel
    (ICatalogService catalogService, IBasketService basketService, ILogger<IndexModel> logger)
    : PageModel
{    
    public IEnumerable<ProductModel> ProductList { get; set; } = new List<ProductModel>();    

    public async Task<IActionResult> OnGetAsync(string? searchName, decimal? minPrice, decimal? maxPrice)
    {
        logger.LogInformation("Index page visited");
        
        if (!string.IsNullOrWhiteSpace(searchName) || minPrice.HasValue || maxPrice.HasValue)
        {
            var result = await catalogService.SearchProducts(searchName, minPrice, maxPrice);
            ProductList = result.Products;
        }
        else
        {
            var result = await catalogService.GetProducts();
            ProductList = result.Products;
        }
        
        return Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
    {
        logger.LogInformation("Add to cart button clicked");

        var productResponse = await catalogService.GetProduct(productId);

        var basket = await basketService.LoadUserBasket();

        basket.Items.Add(new ShoppingCartItemModel
        {
            ProductId = productId,
            ProductName = productResponse.Product.Name,
            Price = productResponse.Product.Price,
            Quantity = 1,
            Color = "Black"
        });

        await basketService.StoreBasket(new StoreBasketRequest(basket));
        
        return RedirectToPage("Cart");
    }    
}
