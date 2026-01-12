using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace NetBlaze.Ui.Client.Services.CommonServices
{
    public class AuthGuard
    {
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _nav;

        public AuthGuard(
            ILocalStorageService localStorage,
            NavigationManager nav)
        {
            _localStorage = localStorage;
            _nav = nav;
        }

        public async Task<bool> EnsureAuthenticatedAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("auth_token");

            if (string.IsNullOrWhiteSpace(token))
            {
                var returnUrl = _nav.ToBaseRelativePath(_nav.Uri);
                _nav.NavigateTo($"/?returnUrl=/{returnUrl}", forceLoad: true);
                return false;
            }

            return true;
        }
    }
}
