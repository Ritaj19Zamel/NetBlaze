using NetBlaze.SharedKernel.HelperUtilities.General;
using System.Net.Http.Json;
using System.Text.Json;

namespace NetBlaze.Ui.Client.Services.CommonServices
{
    public class ExternalHttpClientWrapper
    {
        public HttpClient NativeHttpClient { get; }

        public ExternalHttpClientWrapper(HttpClient httpClient)
        {
            NativeHttpClient = httpClient;
        }

        public async Task<TResponse> GetFromJsonAsync<TResponse>(
            string url,
            CancellationToken cancellationToken = default)
        {
            var response = await NativeHttpClient.GetAsync(url, cancellationToken);
            return await ReadResponseAsync<TResponse>(response);
        }

        public async Task<TResponse> PostAsJsonAsync<TRequest, TResponse>(
            string url,
            TRequest data,
            CancellationToken cancellationToken = default)
        {
            var response = await NativeHttpClient.PostAsJsonAsync(url, data, cancellationToken);
            return await ReadResponseAsync<TResponse>(response);
        }

        public async Task<TResponse> PutAsJsonAsync<TRequest, TResponse>(
            string url,
            TRequest data,
            CancellationToken cancellationToken = default)
        {
            var response = await NativeHttpClient.PutAsJsonAsync(url, data, cancellationToken);
            return await ReadResponseAsync<TResponse>(response);
        }

        public async Task<TResponse> DeleteFromJsonAsync<TResponse>(
            string url,
            CancellationToken cancellationToken = default)
        {
            var response = await NativeHttpClient.DeleteAsync(url, cancellationToken);
            return await ReadResponseAsync<TResponse>(response);
        }

        private static async Task<TResponse> ReadResponseAsync<TResponse>(
            HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
                return default!;

            return JsonSerializer.Deserialize<TResponse>(
                content,
                CustomJsonSerializerOptions._jsonSerializerOptions
            )!;
        }
    }
}
