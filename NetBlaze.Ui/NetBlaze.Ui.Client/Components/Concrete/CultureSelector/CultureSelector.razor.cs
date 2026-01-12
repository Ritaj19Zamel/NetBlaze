using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.Ui.Client.Services.CommonServices;
using System.Globalization;
using Blazored.LocalStorage;

namespace NetBlaze.Ui.Client.Components.Concrete.CultureSelector
{
    public partial class CultureSelector
    {
        [Inject] private CookieService CookieService { get; set; } = null!;

        private bool IsRequestedLanguageEnglish { get; set; } =
            CultureInfo.CurrentCulture.Name == LanguageCode.ENGLISH_CODE;

        private async Task ToggleLanguageAsync()
        {
            try
            {
                IsRequestedLanguageEnglish = !IsRequestedLanguageEnglish;

                var newCulture = IsRequestedLanguageEnglish
                    ? LanguageCode.ENGLISH_CODE
                    : LanguageCode.ARABIC_CODE;

                if (CultureInfo.CurrentCulture.Name != newCulture)
                {
                    // Set the culture cookie
                    await CookieService.SetCookieAsync(
                        MiscConstants.currentCultureCode,
                        newCulture,
                        365);

                    // Use JavaScript to reload the page - this preserves authentication
                    await JSRuntime.InvokeVoidAsync("location.reload", true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error toggling language: {ex.Message}");
            }
        }
    }
}