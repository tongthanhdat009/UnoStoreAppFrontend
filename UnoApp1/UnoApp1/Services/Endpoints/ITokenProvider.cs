namespace UnoApp1.Services.Endpoints;

public interface ITokenProvider
{
    Task<string?> GetTokenAsync(CancellationToken cancellationToken = default);
    Task SetTokenAsync(string? token, CancellationToken cancellationToken = default);
    Task ClearTokenAsync(CancellationToken cancellationToken = default);
}
