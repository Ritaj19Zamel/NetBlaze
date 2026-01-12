namespace NetBlaze.Ui.Client.Services.CommonServices
{
    public interface IJwtAuthService
    {
        Task SetTokenAsync(string token);
        Task ClearAsync();
    }
}
