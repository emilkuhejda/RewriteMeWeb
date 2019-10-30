using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class TranscribeItemEntityConfiguration : IEntityTypeConfiguration<TranscribeItemEntity>
    {
        public void Configure(EntityTypeBuilder<TranscribeItemEntity> builder)
        {
            builder.ToTable("TranscribeItem");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.FileItemId).IsRequired();
            builder.Property(x => x.ApplicationId).IsRequired();
            builder.Property(x => x.Alternatives).IsRequired();
            builder.Property(x => x.SourceFileName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.StartTime).IsRequired();
            builder.Property(x => x.EndTime).IsRequired();
            builder.Property(x => x.TotalTime).IsRequired();
            builder.Property(x => x.DateCreatedUtc).IsRequired();
            builder.Property(x => x.DateUpdatedUtc).IsRequired();
        }
    }
}
