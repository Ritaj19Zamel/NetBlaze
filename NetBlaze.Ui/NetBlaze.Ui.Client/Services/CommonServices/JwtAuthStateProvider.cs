using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace NetBlaze.Ui.Client.Services.CommonServices
{
    public class JwtAuthStateProvider : AuthenticationStateProvider, IJwtAuthService
    {
        private readonly ILocalStorageService _localStorage;
        private string? _cachedToken;
        private bool _isInitialized;

        public JwtAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = Convert.FromBase64String(
                payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=')
            );

            var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonBytes)!;
            var claims = new List<Claim>();

            foreach (var kvp in data)
            {
                if (kvp.Key.Equals("role", StringComparison.OrdinalIgnoreCase) ||
                    kvp.Key.Contains("identity/claims/role", StringComparison.OrdinalIgnoreCase))
                {
                    if (kvp.Value.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var r in kvp.Value.EnumerateArray())
                            claims.Add(new Claim(ClaimTypes.Role, r.GetString()!));
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, kvp.Value.GetString()!));
                    }
                }
                else if (kvp.Key.Equals("sub", StringComparison.OrdinalIgnoreCase))
                {
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, kvp.Value.ToString()));
                }
                else if (kvp.Key.Equals("email", StringComparison.OrdinalIgnoreCase))
                {
                    claims.Add(new Claim(ClaimTypes.Email, kvp.Value.ToString()));
                }
                else if (kvp.Key.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    claims.Add(new Claim(ClaimTypes.Name, kvp.Value.ToString()));
                }
                else
                {
                    claims.Add(new Claim(kvp.Key, kvp.Value.ToString()));
                }
            }

            return claims;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Try to use cached token first (avoids JS interop during prerendering)
            if (_isInitialized && string.IsNullOrWhiteSpace(_cachedToken))
                return Anonymous();

            // Only call localStorage if not initialized yet
            if (!_isInitialized)
            {
                try
                {
                    _cachedToken = await _localStorage.GetItemAsync<string>("auth_token");
                    _isInitialized = true;
                }
                catch (InvalidOperationException)
                {
                    // JS interop not available yet (prerendering)
                    return Anonymous();
                }
            }

            if (string.IsNullOrWhiteSpace(_cachedToken))
                return Anonymous();

            try
            {
                var claims = ParseClaimsFromJwt(_cachedToken);
                var identity = new ClaimsIdentity(claims, "jwt", ClaimTypes.Name, ClaimTypes.Role);
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
            catch
            {
                return Anonymous();
            }
        }

        public async Task SetTokenAsync(string token)
        {
            _cachedToken = token;
            _isInitialized = true;

            try
            {
                await _localStorage.SetItemAsync("auth_token", token);
            }
            catch (InvalidOperationException)
            {
                // JS interop not available - token is cached in memory
            }

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task ClearAsync()
        {
            _cachedToken = null;
            _isInitialized = true;

            try
            {
                await _localStorage.RemoveItemAsync("auth_token");
            }
            catch (InvalidOperationException)
            {
                // JS interop not available
            }

            NotifyAuthenticationStateChanged(Task.FromResult(Anonymous()));
        }

        private static AuthenticationState Anonymous() =>
            new(new ClaimsPrincipal(new ClaimsIdentity()));
    }
}