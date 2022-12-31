using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Entities.TypeConfig
{
    public class PhotoTypeConfig : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.ToTable("Photo");
            builder.HasKey(p => p.PhotoId);
            builder.Property(p => p.Url);
            builder.Property(p => p.IsMain);
            builder.Property(p => p.PublicPhotoId);
            builder.HasOne(p => p.AppUser)
                .WithMany(au => au.Photos)
                .HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}