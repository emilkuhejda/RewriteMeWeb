using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class InformationMessageEntityConfiguration : IEntityTypeConfiguration<InformationMessageEntity>
    {
        public void Configure(EntityTypeBuilder<InformationMessageEntity> builder)
        {
            builder.ToTable("InformationMessage");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.CampaignName).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Message).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Language).IsRequired();
            builder.Property(x => x.SentOnOsx).IsRequired();
            builder.Property(x => x.SentOnAndroid).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired();
        }
    }
}
