using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class FileItemEntityConfiguration : IEntityTypeConfiguration<FileItemEntity>
    {
        public void Configure(EntityTypeBuilder<FileItemEntity> builder)
        {
            builder.ToTable("FileItem");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
            builder.Property(x => x.FileName).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Language).IsRequired().HasMaxLength(20);
            builder.Property(x => x.RecognitionState).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired();

            builder.HasMany(x => x.TranscribeItems).WithOne(x => x.FileItem).HasForeignKey(x => x.FileItemId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.AudioSource).WithOne(x => x.FileItem).HasForeignKey<AudioSourceEntity>(x => x.FileItemId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
