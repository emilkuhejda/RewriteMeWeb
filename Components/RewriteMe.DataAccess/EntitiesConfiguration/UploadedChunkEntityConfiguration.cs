using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class UploadedChunkEntityConfiguration : IEntityTypeConfiguration<UploadedChunkEntity>
    {
        public void Configure(EntityTypeBuilder<UploadedChunkEntity> builder)
        {
            builder.ToTable("UploadedChunk");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.FileItemId).IsRequired();
            builder.Property(x => x.Source).IsRequired();
            builder.Property(x => x.Order).IsRequired();
            builder.Property(x => x.DateCreatedUtc).IsRequired();
        }
    }
}
