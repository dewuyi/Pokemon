namespace Pokedex.Model;

public class Pokemon
{
    public int PokemonId { get; set; }
    public string Name { get; set; }
    public string Habitat { get; set; }
    public bool IsLegendary { get; set; }
    public string Description { get; set; }
    public DateTime? LastTranslated { get; set; }
}