using System.Net.Http.Headers;
using Windows.Storage;

namespace UnoApp1.Services.Endpoints;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly ILogger<AuthHeaderHandler> _logger;

    public AuthHeaderHandler(ILogger<AuthHeaderHandler> logger, HttpMessageHandler? innerHandler = null)
        : base(innerHandler ?? new HttpClientHandler())
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Retrieve JWT token from ApplicationData LocalSettings
            var localSettings = ApplicationData.Current.LocalSettings;
            var token = localSettings.Values["AuthToken"] as string;

            if (!string.IsNullOrEmpty(token))
            {
                // Attach the token to the Authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                _logger.LogInformation("JWT token attached to request");
            }
            else
            {
                _logger.LogWarning("No JWT token found in LocalSettings");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving JWT token from LocalSettings");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}

