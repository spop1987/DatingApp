using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Entities.TypeConfig
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
            // builder.Property(u => u.PasswordHash).HasColumnType("varbinary(1024)");
            // builder.Property(u => u.PasswordSalt).HasColumnType("varbinary(1024)");
        }
    }
}