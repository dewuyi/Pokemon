namespace Pokedex.Services;

public interface IApiClient
{
    public Task<T> GetResourceAsync<T>(Uri baseApiUri, string body, CancellationToken cancellationToken);
}