using Windows.Storage;

namespace UnoApp1.Services;

/// <summary>
/// Helper service for managing JWT tokens in storage
/// Use this for testing or integrate with your authentication flow
/// </summary>
public class TokenService
{
    private readonly ILogger<TokenService> _logger;
    private const string TokenKey = "AuthToken";

    public TokenService(ILogger<TokenService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Save JWT token to local storage
    /// </summary>
    public void SaveToken(string token)
    {
        try
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[TokenKey] = token;
            _logger.LogInformation("JWT token saved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save JWT token");
            throw;
        }
    }

    /// <summary>
    /// Get JWT token from local storage
    /// </summary>
    public string? GetToken()
    {
        try
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            var token = localSettings.Values[TokenKey] as string;
            
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("No JWT token found in storage");
            }
            
            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve JWT token");
            return null;
        }
    }

    /// <summary>
    /// Remove JWT token from local storage (for logout)
    /// </summary>
    public void ClearToken()
    {
        try
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values.Remove(TokenKey);
            _logger.LogInformation("JWT token cleared successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to clear JWT token");
            throw;
        }
    }

    /// <summary>
    /// Check if a token exists in storage
    /// </summary>
    public bool HasToken()
    {
        var token = GetToken();
        return !string.IsNullOrEmpty(token);
    }
}

