using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class RecognizedAudioSampleEntityConfiguration : IEntityTypeConfiguration<RecognizedAudioSampleEntity>
    {
        public void Configure(EntityTypeBuilder<RecognizedAudioSampleEntity> builder)
        {
            builder.ToTable("RecognizedAudioSample");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.DateCreated).IsRequired();

            builder.HasMany(x => x.SpeechResults).WithOne(x => x.RecognizedAudioSample).HasForeignKey(x => x.RecognizedAudioSampleId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
