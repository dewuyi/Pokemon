using Pokedex;
using Pokedex.Middleware;
using Pokedex.Repository;
using Pokedex.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<IApiClient, ApiClient>();
builder.Services.AddSingleton<IPokeApiService, PokeApiService>();
builder.Services.AddSingleton<IPokemonRepository, PokemonRepository>();
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();