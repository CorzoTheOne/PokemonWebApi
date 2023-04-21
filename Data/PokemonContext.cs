using Microsoft.EntityFrameworkCore;
using PokemonWebApi.Model;

namespace PokemonWebApi.Data
{
    public class PokemonContext : DbContext 
    {
        //public PokemonContext(DbContextOptions<PokemonContext> options) : base(options) { }
        public DbSet<Pokemon> Pokemons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=(LocalDb)\\LocalDBDemo;Database=PokemonDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=True");
        }
    }
}
