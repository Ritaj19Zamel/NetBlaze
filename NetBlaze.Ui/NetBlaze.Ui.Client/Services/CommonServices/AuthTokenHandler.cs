using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http.Headers;

namespace NetBlaze.Ui.Client.Services.CommonServices
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;

        public AuthTokenHandler(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Get token from localStorage (WebAssembly)
            try
            {
                var token = await _localStorage.GetItemAsync<string>("auth_token", cancellationToken);
                if (!string.IsNullOrWhiteSpace(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            catch (InvalidOperationException)
            {
                // JS interop not available (prerendering) - continue without token
            }

            // For WebAssembly, include credentials with the request
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}