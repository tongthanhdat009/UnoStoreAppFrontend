using Refit;
using UnoApp1.Models;

namespace UnoApp1.Services.Endpoints;

public interface IRetailApiService
{
    [Get("/api/products")]
    Task<List<Product>> GetProductsAsync();

    [Post("/api/Order")]
    Task<ApiResponse<object>> CreateOrderAsync([Body] OrderModel order);
}

