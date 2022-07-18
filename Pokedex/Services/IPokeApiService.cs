using Pokedex.Model;

namespace Pokedex.Services;

public interface IPokeApiService
{
    Task<Pokemon> GetTranslatedPokemon(string pokemonName,
        CancellationToken cancellationToken);

    Task<Pokemon> GetBasicPokemon(
        string pokemonName,
        CancellationToken cancellationToken);
}