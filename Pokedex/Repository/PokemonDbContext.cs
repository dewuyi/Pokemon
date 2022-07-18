using Microsoft.EntityFrameworkCore;
using Pokedex.Model;

namespace Pokedex.Repository
{
    public partial class PokemonDbContext : DbContext
    {
        private string _connectionString;
        public PokemonDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public PokemonDbContext(DbContextOptions<PokemonDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Pokemon?> Pokemons { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pokemon>(entity =>
            {
                entity.ToTable("Pokemon");
                
                entity.Property(e => e.PokemonId)
                    .ValueGeneratedNever()
                    .HasColumnName("PokemonID");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastTranslated).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
