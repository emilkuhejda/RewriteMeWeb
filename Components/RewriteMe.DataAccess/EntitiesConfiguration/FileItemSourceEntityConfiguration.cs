using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class FileItemSourceEntityConfiguration : IEntityTypeConfiguration<FileItemSourceEntity>
    {
        public void Configure(EntityTypeBuilder<FileItemSourceEntity> builder)
        {
            builder.ToTable("FileItemSource");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.FileItemId).IsRequired();
            builder.Property(x => x.Source).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired();
        }
    }
}
