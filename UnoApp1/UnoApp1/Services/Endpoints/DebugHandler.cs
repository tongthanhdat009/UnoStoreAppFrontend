namespace UnoApp1.Services.Endpoints;

internal class DebugHttpHandler : DelegatingHandler
{
    private readonly ILogger _logger;

    public DebugHttpHandler(ILogger<DebugHttpHandler> logger, HttpMessageHandler? innerHandler = null)
        : base(innerHandler ?? new HttpClientHandler())
    {
        _logger = logger;
    }

    protected async override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
#if DEBUG
        // Log request
        _logger.LogDebugMessage("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        _logger.LogDebugMessage($"🌐 [HTTP REQUEST] {request.Method} {request.RequestUri}");
        System.Diagnostics.Debug.WriteLine($"🌐 [HTTP REQUEST] {request.Method} {request.RequestUri}");
        
        // Log request headers
        if (request.Headers.Any())
        {
            _logger.LogDebugMessage("📋 Request Headers:");
            foreach ((var key, var values) in request.Headers.ToDictionary(x => x.Key, x => string.Join(", ", x.Value)))
            {
                var maskedValue = key.ToLower() == "authorization" ? "Bearer ***" : values;
                _logger.LogDebugMessage($"   {key}: {maskedValue}");
                System.Diagnostics.Debug.WriteLine($"   {key}: {maskedValue}");
            }
        }
        
        // Log request body
        if (request.Content is not null)
        {
            var requestBody = await request.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(requestBody))
            {
                _logger.LogDebugMessage($"📤 Request Body: {requestBody}");
                System.Diagnostics.Debug.WriteLine($"📤 Request Body: {requestBody}");
            }
        }
#endif
        
        var response = await base.SendAsync(request, cancellationToken);
        stopwatch.Stop();
        
#if DEBUG
        // Log response
        if (response.IsSuccessStatusCode)
        {
            _logger.LogDebugMessage($"✅ [HTTP SUCCESS] {(int)response.StatusCode} {response.StatusCode} ({stopwatch.ElapsedMilliseconds}ms)");
            System.Diagnostics.Debug.WriteLine($"✅ [HTTP SUCCESS] {(int)response.StatusCode} {response.StatusCode} ({stopwatch.ElapsedMilliseconds}ms)");
            
            // Optionally log response body for successful calls
            if (response.Content is not null)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseBody))
                {
                    var preview = responseBody.Length > 200 ? responseBody.Substring(0, 200) + "..." : responseBody;
                    _logger.LogDebugMessage($"📥 Response Preview: {preview}");
                    System.Diagnostics.Debug.WriteLine($"📥 Response: {responseBody.Length} characters");
                }
            }
        }
        else
        {
            _logger.LogDebugMessage($"❌ [HTTP FAILED] {(int)response.StatusCode} {response.StatusCode} ({stopwatch.ElapsedMilliseconds}ms)");
            System.Diagnostics.Debug.WriteLine($"❌ [HTTP FAILED] {(int)response.StatusCode} {response.StatusCode} ({stopwatch.ElapsedMilliseconds}ms)");
            
            if (request.RequestUri is not null)
            {
                _logger.LogDebugMessage($"🔗 URI: {request.RequestUri}");
                System.Diagnostics.Debug.WriteLine($"🔗 URI: {request.RequestUri}");
            }

            // Log response headers
            if (response.Headers.Any())
            {
                _logger.LogDebugMessage("📋 Response Headers:");
                foreach ((var key, var values) in response.Headers.ToDictionary(x => x.Key, x => string.Join(", ", x.Value)))
                {
                    _logger.LogDebugMessage($"   {key}: {values}");
                    System.Diagnostics.Debug.WriteLine($"   {key}: {values}");
                }
            }

            // Log error response body
            var errorContent = response.Content is not null ? await response.Content.ReadAsStringAsync() : null;
            if (!string.IsNullOrEmpty(errorContent))
            {
                _logger.LogDebugMessage($"📥 Error Response: {errorContent}");
                System.Diagnostics.Debug.WriteLine($"📥 Error Response: {errorContent}");
            }

            // Uncomment to automatically break when an API call fails while debugging
            // System.Diagnostics.Debugger.Break();
        }
        
        _logger.LogDebugMessage("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
#endif
        return response;
    }
}
