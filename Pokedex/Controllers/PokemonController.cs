using Microsoft.AspNetCore.Mvc;
using Pokedex.Model;
using Pokedex.Repository;
using Pokedex.Services;

namespace Pokedex.Controllers;

[ApiController]
[Route("pokemon")]
public class PokemonController : ControllerBase
{
    private IPokeApiService _pokeApiService;
    private IPokemonRepository _pokemonRepository;
    
    public PokemonController(IPokeApiService pokeApiService, IPokemonRepository pokemonRepository)
    {
        _pokeApiService = pokeApiService;
        _pokemonRepository = pokemonRepository;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pokemonName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{pokemonName}")]
    
    public async Task<ActionResult<IEnumerable<Pokemon>>> GetBasicPokemon(
        string pokemonName,
        CancellationToken cancellationToken)
    {
        try
        {
            var pokemon = await _pokeApiService.GetBasicPokemon(pokemonName, cancellationToken);
            if (pokemon == null)
            {
                return NotFound();
            }
            AddOrUpdatePokemon(pokemon);
            return Ok(pokemon);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
       
    }
   
    [HttpGet("{pokemonName}/translated")]
    
    public async Task<ActionResult<IEnumerable<Pokemon>>> GetTranslatedPokemon(string pokemonName,
        CancellationToken cancellationToken)
    {
        try
        {
            var pokemon = await _pokeApiService.GetTranslatedPokemon(pokemonName, cancellationToken);
            if (pokemon == null)
            {
                return NotFound();
            }
            
            AddOrUpdatePokemon(pokemon);
            return Ok(pokemon);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    private void AddOrUpdatePokemon(Pokemon pokemon)
    {
        var pokemonLookup = _pokemonRepository.GetPokemon(pokemon.Name);
        if (pokemonLookup != null)
        {
            _pokemonRepository.UpdatePokemon(pokemon.Name);
        }
        else
        {
            _pokemonRepository.AddPokemon(pokemon);
        }
    }
}