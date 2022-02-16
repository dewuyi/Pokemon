using Pokedex.Model;

namespace Pokedex.Services;

public class PokeApiService:IPokeApiService
{
    private IApiClient _apiClient;
    public PokeApiService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    
    private readonly Uri _basePokeApiUri = new Uri("https://pokeapi.co/api/v2/pokemon-species/");
    private readonly Uri _baseShakespeareTranslationUri = new Uri("https://api.funtranslations.com/translate/shakespeare.json");
    private readonly Uri _baseYodaTranslationUri = new Uri("https://api.funtranslations.com/translate/yoda.json");
    
    public async Task<PokemonResponse> GetTranslatedPokemon(string pokemonName, CancellationToken cancellationToken)
    {
        var pokemon = await _apiClient.GetResourceAsync<Pokemon>(_basePokeApiUri, pokemonName, cancellationToken);
        if (pokemon == null)
        {
            return null;
        }
        
        var response = new PokemonResponse
        {
            Name = pokemon.Name,
            Description = pokemon.PokemonDescriptions.First(d => d.Language.Name == "en").Description,
            Habitat = pokemon.Habitat.Name,
            IsLegendary = pokemon.IsLegendary
        };
        
        var sanitizedDescription = response.Description.Replace("\n", " ").Replace("\f", " ");
        var result = await _apiClient.GetResourceAsync<PokemonTranslatedDescription>(GetPokemonTranslationUri(response),
            $"?text={sanitizedDescription}", cancellationToken);
        response.Description = result.Contents.Translated;
        
        return response;
    }

    public async Task<PokemonResponse> GetBasicPokemon(string pokemonName, CancellationToken cancellationToken)
    {
        var pokemon = await _apiClient.GetResourceAsync<Pokemon>(_basePokeApiUri, pokemonName, cancellationToken);
        if (pokemon == null)
        {
            return null;
        }
        
        var response = new PokemonResponse
        {
            Name = pokemon.Name,
            Description = pokemon.PokemonDescriptions.First(d => d.Language.Name == "en").Description,
            Habitat = pokemon.Habitat.Name,
            IsLegendary = pokemon.IsLegendary
        };
        
        return response;
    }

    private Uri GetPokemonTranslationUri(PokemonResponse pokemon)
    {
        if (pokemon.Habitat == "cave" || pokemon.IsLegendary)
        {
            return _baseYodaTranslationUri;
        }

        return _baseShakespeareTranslationUri;
    }
}