using Pokedex.Model;

namespace Pokedex.Repository;

public interface IPokemonRepository
{
    void AddPokemon(Pokemon? pokemon);
    void UpdatePokemon(string pokemonName);
    Pokemon? GetPokemon(string pokemonName);
}