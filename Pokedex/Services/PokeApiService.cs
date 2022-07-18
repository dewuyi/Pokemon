using System.Collections.Concurrent;
using Pokedex.Model;

namespace Pokedex.Services;

public class PokeApiService:IPokeApiService
{
    private IApiClient _apiClient;
    private readonly ConcurrentDictionary<string, PokemonResponse> _cachedPokemon = new();
    public PokeApiService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    
    private readonly Uri _basePokeApiUri = new Uri("https://pokeapi.co/api/v2/pokemon-species/");
    private readonly Uri _baseShakespeareTranslationUri = new Uri("https://api.funtranslations.com/translate/shakespeare.json");
    private readonly Uri _baseYodaTranslationUri = new Uri("https://api.funtranslations.com/translate/yoda.json");
    
    public async Task<Pokemon> GetTranslatedPokemon(string pokemonName, CancellationToken cancellationToken)
    {
        PokemonResponse pokemon;

        if (_cachedPokemon.TryGetValue(pokemonName, out var cachedPokemon))
        {
            pokemon = cachedPokemon;
        }
        else
        {
            pokemon = await _apiClient.GetResourceAsync<PokemonResponse>(_basePokeApiUri, pokemonName, cancellationToken);
            _cachedPokemon[pokemon.Name] = pokemon;
        }
        
        if (pokemon == null)
        {
            return null;
        }
        
        var response = new Pokemon
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

    public async Task<Pokemon> GetBasicPokemon(string pokemonName, CancellationToken cancellationToken)
    {
        PokemonResponse pokemon;
        if (_cachedPokemon.TryGetValue(pokemonName, out var cachedPokemon))
        {
            pokemon = cachedPokemon;
        }
        else
        {
            pokemon = await _apiClient.GetResourceAsync<PokemonResponse>(_basePokeApiUri, pokemonName, cancellationToken);
        }
       
       
        if (pokemon == null)
        {
            return null;
        }
        
        var response = new Pokemon
        {
            Name = pokemon.Name,
            Description = pokemon.PokemonDescriptions.First(d => d.Language.Name == "en").Description,
            Habitat = pokemon.Habitat.Name,
            IsLegendary = pokemon.IsLegendary
        };
        
        return response;
    }

    private Uri GetPokemonTranslationUri(Pokemon pokemon)
    {
        if (pokemon.Habitat == "cave" || pokemon.IsLegendary)
        {
            return _baseYodaTranslationUri;
        }

        return _baseShakespeareTranslationUri;
    }
}