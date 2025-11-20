namespace UnoApp1.Services.Endpoints;

/// <summary>
/// Lưu trữ token JWT đơn giản trong bộ nhớ tiến trình. Có thể thay thế bằng secure storage nếu cần.
/// </summary>
public sealed class JwtTokenProvider : ITokenProvider
{
    private string? _token;

    public Task<string?> GetTokenAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(_token);

    public Task SetTokenAsync(string? token, CancellationToken cancellationToken = default)
    {
        _token = token;
        return Task.CompletedTask;
    }

    public Task ClearTokenAsync(CancellationToken cancellationToken = default)
    {
        _token = null;
        return Task.CompletedTask;
    }
}
