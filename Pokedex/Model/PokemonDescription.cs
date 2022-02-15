using System.Text.Json.Serialization;

namespace Pokedex.Model;

public class PokemonDescription
{
    [JsonPropertyName("flavor_text")]  
    public string Description { get; set; }
    public ResponseItem Language { get; set; }
}