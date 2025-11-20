using System.Net.Http.Headers;

namespace UnoApp1.Services.Endpoints;

/// <summary>
/// DelegatingHandler tự động gắn Header Authorization: Bearer {token} cho mọi request nếu token có sẵn.
/// </summary>
internal sealed class AuthorizationHandler(ITokenProvider tokenProvider) : DelegatingHandler
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenProvider.GetTokenAsync(cancellationToken);
        if (!string.IsNullOrWhiteSpace(token) && request.Headers.Authorization is null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        return await base.SendAsync(request, cancellationToken);
    }
}
