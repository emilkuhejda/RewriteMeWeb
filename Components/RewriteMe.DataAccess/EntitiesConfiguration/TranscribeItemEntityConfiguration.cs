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
            builder.Property(x => x.Alternatives).IsRequired();
            builder.Property(x => x.Source).IsRequired();
            builder.Property(x => x.TotalTime).IsRequired();
        }
    }
}
