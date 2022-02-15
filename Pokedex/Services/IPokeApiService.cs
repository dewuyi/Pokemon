using Pokedex.Model;

namespace Pokedex.Services;

public interface IPokeApiService
{
    Task<PokemonResponse> GetTranslatedPokemon(string pokemonName,
        CancellationToken cancellationToken);

    Task<PokemonResponse> GetBasicPokemon(
        string pokemonName,
        CancellationToken cancellationToken);
}