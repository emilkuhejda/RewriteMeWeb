using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class TranscribeItemSourceEntityConfiguration : IEntityTypeConfiguration<TranscribeItemSourceEntity>
    {
        public void Configure(EntityTypeBuilder<TranscribeItemSourceEntity> builder)
        {
            builder.ToTable("TranscribeItemSource");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.FileItemId).IsRequired();
            builder.Property(x => x.Source).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired();
        }
    }
}
