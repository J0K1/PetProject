using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetProject.Models;

namespace PetProject.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder
                .Property(u => u.Login)
                .IsRequired()
                .HasMaxLength(64);

            builder
                .Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(128);

            builder
                .Property(u => u.Nick)
                .IsRequired()
                .HasMaxLength(32);

            builder
                .Property(u => u.Role)
                .HasConversion<string>();

            builder
                .HasMany(u => u.Friends)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "UserFriends",
                    j => j.HasOne<UserEntity>().WithMany().HasForeignKey("FriendId").OnDelete(DeleteBehavior.NoAction),
                    j => j.HasOne<UserEntity>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("UserId", "FriendId");
                    });


            builder
                .HasMany(u => u.Games)
                .WithMany()
                .UsingEntity(j => j.ToTable("UserGames"));

        }
    }

}
