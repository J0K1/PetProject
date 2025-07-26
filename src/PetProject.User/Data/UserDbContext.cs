using Microsoft.EntityFrameworkCore;
using PetProject.Shared.Entities;
using PetProject.Shared.Configurations;

namespace PetProject.User.Data
{
    public class UserDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(modelBuilder);
        }

    }
}
