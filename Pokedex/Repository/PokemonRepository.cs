using Microsoft.Extensions.Options;
using Pokedex.Model;

namespace Pokedex.Repository;

public class PokemonRepository:IPokemonRepository
{
    private readonly DatabaseSettings _databaseSettings;

    public PokemonRepository(IOptions<DatabaseSettings> databaseSettings)
    {
        _databaseSettings = databaseSettings.Value;
    }
    public void AddPokemon(Pokemon? pokemon)
    {
        using var context = new PokemonDbContext(_databaseSettings.ConnectionString);
        pokemon.LastTranslated = DateTime.Now;
        context.Pokemons.Add(pokemon);
        context.SaveChanges();
    }

    public void UpdatePokemon(string pokemonName)
    {
        using var context = new PokemonDbContext(_databaseSettings.ConnectionString);
        var pokemonToUpdate = context.Pokemons.FirstOrDefault(p => p.Name == pokemonName);
        if (pokemonToUpdate != null)
        {
            pokemonToUpdate.LastTranslated = DateTime.Now;
        }
        context.SaveChanges();
    }

    public Pokemon? GetPokemon(string pokemonName)
    {
        using var context = new PokemonDbContext(_databaseSettings.ConnectionString);
        return context.Pokemons.FirstOrDefault(p => p.Name == pokemonName);
    }
}