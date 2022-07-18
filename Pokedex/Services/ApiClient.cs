using System.Net;
using System.Text.Json;
using Pokedex.Model;

namespace Pokedex.Services;

public class ApiClient:IApiClient
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    /// <summary>
    /// Gets a resource by name; resource is retrieved from cache if possible. This lookup
    /// is case insensitive.
    /// </summary>
    /// <typeparam name="T">The type of resource</typeparam>
    /// <param name="name">Name of resource</param>
    /// <param name="cancellationToken">Cancellation token for the request; not utilitized if data has been cached</param>
    /// <returns>The object of the resource</returns>
    public async Task<T> GetResourceAsync<T>(Uri baseApiUri, string name, CancellationToken cancellationToken)
    {
        var resource = await GetAsync<T>($"{baseApiUri}{name.ToLower()}", cancellationToken);

        return resource;
    }
    
    /// <summary>
    /// Handles all outbound API requests to the PokeAPI server and deserializes the response
    /// </summary>
    private async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return default;
        }

        response.EnsureSuccessStatusCode();
        var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
       return await JsonSerializer.DeserializeAsync<T>(
            responseStream, SerializerOptions, cancellationToken);
    }
    
}