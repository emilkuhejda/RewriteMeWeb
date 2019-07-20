using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class SpeechResultEntityConfiguration : IEntityTypeConfiguration<SpeechResultEntity>
    {
        public void Configure(EntityTypeBuilder<SpeechResultEntity> builder)
        {
            builder.ToTable("SpeechResult");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.RecognizedAudioSampleId).IsRequired();
        }
    }
}
