using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace NetBlaze.Ui.Client.Components.Generic
{
    public partial class DashboardCard
    {
        [Inject] NavigationManager Navigation { get; set; } = default!;

        [Parameter] public string Title { get; set; } = string.Empty;
        [Parameter] public string Description { get; set; } = string.Empty;
        [Parameter] public string NavigateTo { get; set; } = "/";
        [Parameter] public string Icon { get; set; } = Icons.Material.Filled.Home;
        [Parameter] public Color Color { get; set; } = Color.Primary;
        [Parameter] public string? Policy { get; set; } // Add authorization policy parameter

        private void HandleClick()
        {
            Navigation.NavigateTo(NavigateTo);
        }
    }
}