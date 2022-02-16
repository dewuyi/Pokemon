using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;
using Pokedex.Model;
using Pokedex.Services;

namespace PokedexTests;

[TestFixture]
public class PokeApiServiceTests
{
    private Fixture _fixture;
    private Mock<IApiClient> _mockApiClient;
    private readonly Uri _basePokeApiUri = new Uri("https://pokeapi.co/api/v2/pokemon-species/");
    private readonly Uri _baseShakespeareTranslationUri = new Uri("https://api.funtranslations.com/translate/shakespeare.json");
    private readonly Uri _baseYodaTranslationUri = new Uri("https://api.funtranslations.com/translate/yoda.json");
    
    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoMoqCustomization());
        _mockApiClient = _fixture.Freeze<Mock<IApiClient>>();
      
        _mockApiClient.Setup(api => api.GetResourceAsync<PokemonTranslatedDescription>(_baseShakespeareTranslationUri,
            It.IsAny<string>(),
            It.IsAny<CancellationToken>())).Returns(Task.FromResult(GetPokemonTranslation(isYodaTranslation:false)));
        
        _mockApiClient.Setup(api => api.GetResourceAsync<PokemonTranslatedDescription>(_baseYodaTranslationUri,
            It.IsAny<string>(),
            It.IsAny<CancellationToken>())).Returns(Task.FromResult(GetPokemonTranslation(isYodaTranslation:true)));
    }
    [Test]
    public async Task GetBasicPokemon_ReturnsBasicPokemon()
    {
        var pokeApiService = _fixture.Create<PokeApiService>();
        _mockApiClient.Setup(api => api.GetResourceAsync<Pokemon>(_basePokeApiUri,
            "caterpie",
            It.IsAny<CancellationToken>())).Returns(Task.FromResult(GetPokemons().First(p=>p.Name=="caterpie")));

        var description =
            "Forms colonies in\nperpetually dark\nplaces. Uses\fultrasonic waves\nto identify and\napproach targets.";
       
        var result = await pokeApiService.GetBasicPokemon("caterpie", CancellationToken.None);
        
        Assert.AreEqual(result.Description, description);
    }
    
    [Test]
    public async Task GetTranslatedPokemon_ReturnsTranslatedPokemonYoda()
    {
        var pokeApiService = _fixture.Create<PokeApiService>();
        _mockApiClient.Setup(api => api.GetResourceAsync<Pokemon>(_basePokeApiUri,
            It.IsAny<string>(),
            It.IsAny<CancellationToken>())).Returns(Task.FromResult(GetPokemons().First(p=>p.Name=="zubat")));
        
        var description =
            "Lost a planet,  master obiwan has.";
        
        var result = await pokeApiService.GetTranslatedPokemon("zubat", CancellationToken.None);
        
        Assert.AreEqual(result.Description, description);
    }

    [Test]
    public async Task GetTranslatedPokemon_LegendaryPokemonReturnsTranslatedPokemonYoda()
    {
        var pokeApiService = _fixture.Create<PokeApiService>();
        _mockApiClient.Setup(api => api.GetResourceAsync<Pokemon>(_basePokeApiUri,
            It.IsAny<string>(),
            It.IsAny<CancellationToken>())).Returns(Task.FromResult(GetPokemons().First(p => p.Name== "squirtle")));
        var description =
            "Lost a planet,  master obiwan has.";
        
        var result = await pokeApiService.GetTranslatedPokemon("squirtle", CancellationToken.None);
        
        Assert.AreEqual(result.Description, description);
    }
    
   
    
    
    [Test]
    public async Task GetTranslatedPokemon_ReturnsTranslatedPokemonShakesphere()
    {
        var pokeApiService = _fixture.Create<PokeApiService>();
        _mockApiClient.Setup(api => api.GetResourceAsync<Pokemon>(_basePokeApiUri,
            It.IsAny<string>(),
            It.IsAny<CancellationToken>())).Returns(Task.FromResult(GetPokemons().First(p => p.Name == "charmander")));
        var description =
            "Master obiwan hath did lose a planet.";
        
        var result = await pokeApiService.GetTranslatedPokemon("charmander", CancellationToken.None);
        
        Assert.AreEqual(result.Description, description);
    }
    
    
    
    
    private  IEnumerable<Pokemon> GetPokemons()
    {
        var pokemons = new List<Pokemon>
        {
            new()
            {
                Name = "caterpie",
                Habitat = new ResponseItem()
                {
                    Name = "forest"
                },
                IsLegendary = false,
                PokemonDescriptions = new PokemonDescription[]
                {
                    new()
                    {
                        Language = new ResponseItem()
                        {
                            Name = "fr"
                        },
                        Description =
                            "Il se repère dans l’espace grâce\naux ultrasons émis par sa gueule."

                    },
                    new()
                    {
                        Language = new ResponseItem()
                        {
                            Name = "en"
                        },
                        Description =
                            "Forms colonies in\nperpetually dark\nplaces. Uses\fultrasonic waves\nto identify and\napproach targets."

                    }
                }
            },
            new()
            {
                Name = "zubat",
                Habitat = new ResponseItem()
                {
                    Name = "cave"
                },
                IsLegendary = false,
                PokemonDescriptions = new PokemonDescription[]
                {
                    new()
                    {
                        Language = new ResponseItem()
                        {
                            Name = "fr"
                        },
                        Description =
                            "Maître Obiwan a perdu une planète."

                    },
                    new()
                    {
                        Language = new ResponseItem()
                        {
                            Name = "en"
                        },
                        Description =
                            "Master Obiwan has lost a planet."

                    }
                }
            },
            new()
            {
                Name = "charmander",
                Habitat = new ResponseItem()
                {
                    Name = "mountain"
                },
                IsLegendary = false,
                PokemonDescriptions = new PokemonDescription[]
                {
                    new()
                    {
                        Language = new ResponseItem()
                        {
                            Name = "fr"
                        },
                        Description =
                            "Maître Obiwan a perdu une planète."

                    },
                    new()
                    {
                        Language = new ResponseItem()
                        {
                            Name = "en"
                        },
                        Description =
                            "Master Obiwan has lost a planet."

                    }
                }
            },
            new()
            {
                Name = "squirtle",
                Habitat = new ResponseItem()
                {
                    Name = "waters-edge"
                },
                IsLegendary = true,
                PokemonDescriptions = new PokemonDescription[]
                {
                    new()
                    {
                        Language = new ResponseItem()
                        {
                            Name = "fr"
                        },
                        Description =
                            "Maître Obiwan a perdu une planète."

                    },
                    new()
                    {
                        Language = new ResponseItem()
                        {
                            Name = "en"
                        },
                        Description =
                            "Master Obiwan has lost a planet."

                    }
                }
            }
        };
       
        
        return pokemons;
    }

    private PokemonTranslatedDescription GetPokemonTranslation(bool isYodaTranslation)
    {
        return new PokemonTranslatedDescription
        {
            Contents = new Content()
            {
                Translated = isYodaTranslation
                    ? "Lost a planet,  master obiwan has."
                    : "Master obiwan hath did lose a planet."
            }
        };
    }
}