using System.Text.Json.Serialization;

namespace Pokedex.Model;

public class Pokemon
{
    public string Name { get; set; }
    public ResponseItem Habitat { get; set; }
    public bool IsLegendary { get; set; }
    
    [JsonPropertyName("flavor_text_entries")]  
    public PokemonDescription[] PokemonDescriptions { get; set; }
}

public class PokemonTranslated : Pokemon
{
    public PokemonDescription TranslatedDescription { get; set; }
}

public class PokemonBasic : Pokemon
{
    public PokemonDescription StandardDescription { get; set; }
}