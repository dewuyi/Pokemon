using Microsoft.AspNetCore.Mvc;
using Pokedex.Model;
using Pokedex.Services;

namespace Pokedex.Controllers;

[ApiController]
[Route("pokemon")]
public class PokemonController : ControllerBase
{
    private IPokeApiService _pokeApiService;
    
    public PokemonController(IPokeApiService pokeApiService)
    {
        _pokeApiService = pokeApiService;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pokemonName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{pokemonName}")]
    
    public async Task<ActionResult<IEnumerable<PokemonResponse>>> GetBasicPokemon(
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
        
            return Ok(pokemon);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
       
    }
   
    [HttpGet("{pokemonName}/translated")]
    
    public async Task<ActionResult<IEnumerable<PokemonResponse>>> GetTranslatedPokemon(string pokemonName,
        CancellationToken cancellationToken)
    {
        var pokemon = await _pokeApiService.GetTranslatedPokemon(pokemonName, cancellationToken);
    
        return Ok(pokemon);
    }
}