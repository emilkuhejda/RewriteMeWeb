using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class WavPartialFileEntityConfiguration : IEntityTypeConfiguration<WavPartialFileEntity>
    {
        public void Configure(EntityTypeBuilder<WavPartialFileEntity> builder)
        {
            builder.ToTable("WavPartialFile");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.FileItemId).IsRequired();
            builder.Property(x => x.Path).IsRequired();
            builder.Property(x => x.AudioChannels).IsRequired();
            builder.Property(x => x.StartTime).IsRequired();
            builder.Property(x => x.EndTime).IsRequired();
            builder.Property(x => x.TotalTime).IsRequired();
        }
    }
}
