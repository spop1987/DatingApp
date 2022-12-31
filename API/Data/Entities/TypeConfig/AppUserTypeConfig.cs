using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Entities.TypeConfig
{
    public class AppUserTypeConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("User");
            builder.HasKey(u => u.UserId);
            builder.Property(u => u.UserName).IsRequired().HasColumnType("varchar(50)");
            builder.Property(u => u.PasswordHash);
            builder.Property(u => u.PasswordSalt);
            builder.Property(u => u.DateOfBirth);
            builder.Property(u => u.KnownAs).HasColumnType("varchar(50)");
            builder.Property(u => u.Created);
            builder.Property(u => u.LastActive);
            builder.Property(u => u.Gender).HasColumnType("varchar(10)");
            builder.Property(u => u.Introduction);
            builder.Property(u => u.LookingFor);
            builder.Property(u => u.Interests);
            builder.Property(u => u.City);
            builder.Property(u => u.Country);
            builder.HasMany(u => u.Photos)
                .WithOne(p => p.AppUser)
                .HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
            // builder.Property(u => u.PasswordHash).HasColumnType("varbinary(1024)");
            // builder.Property(u => u.PasswordSalt).HasColumnType("varbinary(1024)");
        }
    }
}