using Microsoft.EntityFrameworkCore;
using PetProject.Shared.Configurations;
using PetProject.Shared.Entities;

namespace PetProject.Game.Data
{
    public class GameDbContext : DbContext
    {
        public DbSet<GameEntity> Games { get; set; }
        public GameDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GameConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
