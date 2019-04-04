using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class AudioSourceEntityConfiguration : IEntityTypeConfiguration<AudioSourceEntity>
    {
        public void Configure(EntityTypeBuilder<AudioSourceEntity> builder)
        {
            builder.ToTable("AudioSource");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.FileItemId).IsRequired();
            builder.Property(x => x.OriginalSource).IsRequired();
            builder.Property(x => x.ContentType).HasMaxLength(50).IsRequired();
        }
    }
}
