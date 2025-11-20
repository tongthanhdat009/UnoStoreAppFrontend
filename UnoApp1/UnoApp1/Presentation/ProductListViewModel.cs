﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UnoApp1.Models;
using UnoApp1.Services.Endpoints;

namespace UnoApp1.Presentation;

public partial class ProductListViewModel : ObservableObject
{
    private readonly IRetailApiService _apiService;
    private readonly INavigator _navigator;
    private readonly ILogger<ProductListViewModel> _logger;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FilteredProducts), nameof(IsEmpty))]
    private string searchQuery = string.Empty;

    public ObservableCollection<Product> Products { get; } = new();

    /// <summary>
    /// Danh sách sản phẩm đã được filter theo SearchQuery
    /// </summary>
    public IEnumerable<Product> FilteredProducts
    {
        get
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
                return Products;

            var query = SearchQuery.ToLowerInvariant();
            return Products.Where(p =>
                p.ProductName.ToLowerInvariant().Contains(query) ||
                p.Barcode.ToLowerInvariant().Contains(query) ||
                (p.Category?.CategoryName?.ToLowerInvariant().Contains(query) ?? false));
        }
    }

    /// <summary>
    /// Kiểm tra xem có sản phẩm nào không (để hiển thị empty state)
    /// </summary>
    public bool IsEmpty => !IsBusy && !Products.Any() && string.IsNullOrEmpty(ErrorMessage);

    public ProductListViewModel(
        IRetailApiService apiService,
        INavigator navigator,
        ILogger<ProductListViewModel> logger)
    {
        _apiService = apiService;
        _navigator = navigator;
        _logger = logger;
        
        _logger.LogDebug("ProductListViewModel initialized");
        Debug.WriteLine("🔧 [ProductListViewModel] Constructor called");
        
        // Auto-load products khi khởi tạo
        _ = LoadProductsAsync();
    }

    partial void OnSearchQueryChanged(string value)
    {
        _logger.LogDebug("Search query changed to: {Query}", value);
        // FilteredProducts sẽ tự động update qua NotifyPropertyChangedFor
    }

    [RelayCommand]
    private async Task LoadProductsAsync()
    {
        _logger.LogDebug("LoadProductsAsync called, IsBusy={IsBusy}", IsBusy);
        Debug.WriteLine($"🔄 [ProductListViewModel] LoadProductsAsync started, IsBusy={IsBusy}");
        
        if (IsBusy)
        {
            _logger.LogDebug("LoadProductsAsync skipped - already busy");
            Debug.WriteLine("⏭️ [ProductListViewModel] Load skipped - already busy");
            return;
        }

        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            
            _logger.LogDebug("Setting IsBusy=true, clearing previous errors");
            Debug.WriteLine("🚀 [ProductListViewModel] Starting API call...");

            _logger.LogInformation("Loading products from API...");
            Debug.WriteLine($"📡 [ProductListViewModel] Calling GetProductsAsync() at {DateTime.Now:HH:mm:ss.fff}");

            var products = await _apiService.GetProductsAsync();
            
            _logger.LogDebug("API response received, product count={Count}", products?.Count ?? 0);
            Debug.WriteLine($"✅ [ProductListViewModel] API response received: {products?.Count ?? 0} products");

            if (products == null)
            {
                _logger.LogWarning("API returned null products list");
                Debug.WriteLine("⚠️ [ProductListViewModel] Products list is NULL!");
                products = new List<Product>();
            }

            Products.Clear();
            _logger.LogDebug("Cleared existing products collection");
            Debug.WriteLine($"🗑️ [ProductListViewModel] Cleared existing products");
            
            var addedCount = 0;
            foreach (var product in products)
            {
                Products.Add(product);
                addedCount++;
                
                if (addedCount <= 3) // Log first 3 products for debugging
                {
                    _logger.LogDebug("Added product: ID={ProductId}, Name={ProductName}, Price={Price}", 
                        product.ProductId, product.ProductName, product.Price);
                    Debug.WriteLine($"  ➕ Product #{addedCount}: {product.ProductName} (ID: {product.ProductId}, Price: {product.Price:N0} VNĐ)");
                }
            }

            // Notify UI về thay đổi FilteredProducts và IsEmpty
            OnPropertyChanged(nameof(FilteredProducts));
            OnPropertyChanged(nameof(IsEmpty));

            stopwatch.Stop();
            _logger.LogInformation("Loaded {Count} products successfully in {ElapsedMs}ms", 
                products.Count, stopwatch.ElapsedMilliseconds);
            Debug.WriteLine($"✨ [ProductListViewModel] SUCCESS! Loaded {products.Count} products in {stopwatch.ElapsedMilliseconds}ms");
        }
        catch (Refit.ApiException apiEx) when (apiEx.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            stopwatch.Stop();
            _logger.LogWarning("Unauthorized access (401) - JWT token may be expired or invalid. Time: {ElapsedMs}ms", 
                stopwatch.ElapsedMilliseconds);
            Debug.WriteLine($"🔒 [ProductListViewModel] 401 UNAUTHORIZED - Token expired/invalid");
            Debug.WriteLine($"   Status: {apiEx.StatusCode}, Message: {apiEx.Message}");
            
            ErrorMessage = "Session expired. Please login again.";
            
            // Navigate to Login page (assuming you have a Login route)
            // await _navigator.NavigateViewModelAsync<LoginViewModel>(this);
        }
        catch (HttpRequestException httpEx)
        {
            stopwatch.Stop();
            _logger.LogError(httpEx, "Network error occurred while loading products. Time: {ElapsedMs}ms", 
                stopwatch.ElapsedMilliseconds);
            Debug.WriteLine($"🌐 [ProductListViewModel] NETWORK ERROR after {stopwatch.ElapsedMilliseconds}ms");
            Debug.WriteLine($"   Exception: {httpEx.GetType().Name}");
            Debug.WriteLine($"   Message: {httpEx.Message}");
            Debug.WriteLine($"   InnerException: {httpEx.InnerException?.Message}");
            
            ErrorMessage = "Network error. Please check your connection and try again.";
        }
        catch (Refit.ApiException apiEx)
        {
            stopwatch.Stop();
            _logger.LogError(apiEx, "API error: Status={StatusCode}, Content={Content}", 
                apiEx.StatusCode, apiEx.Content);
            Debug.WriteLine($"❌ [ProductListViewModel] API ERROR: {apiEx.StatusCode}");
            Debug.WriteLine($"   Content: {apiEx.Content}");
            Debug.WriteLine($"   RequestUri: {apiEx.Uri}");
            
            ErrorMessage = $"API Error ({apiEx.StatusCode}): {apiEx.Message}";
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Unexpected error loading products. Time: {ElapsedMs}ms", 
                stopwatch.ElapsedMilliseconds);
            Debug.WriteLine($"💥 [ProductListViewModel] UNEXPECTED ERROR after {stopwatch.ElapsedMilliseconds}ms");
            Debug.WriteLine($"   Type: {ex.GetType().FullName}");
            Debug.WriteLine($"   Message: {ex.Message}");
            Debug.WriteLine($"   StackTrace: {ex.StackTrace}");
            
            ErrorMessage = $"An error occurred: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
            _logger.LogDebug("LoadProductsAsync completed, IsBusy=false, Products.Count={Count}", Products.Count);
            Debug.WriteLine($"🏁 [ProductListViewModel] LoadProductsAsync finished. Total products: {Products.Count}, IsBusy=false");
        }
    }
}

