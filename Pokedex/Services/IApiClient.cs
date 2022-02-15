namespace Pokedex.Services;

public interface IApiClient:IDisposable
{
    public Task<T> GetResourceAsync<T>(Uri baseApiUri, string body, CancellationToken cancellationToken);
}