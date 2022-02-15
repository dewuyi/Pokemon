namespace Pokedex.Model;

/// <summary>
/// The base class for all pokemon api response item
/// </summary>
public class ResponseItem
{
    /// <summary>
    /// The name of the pokemon property
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The api url of the pokemon property
    /// </summary>
    public string Url { get; set; }
}

