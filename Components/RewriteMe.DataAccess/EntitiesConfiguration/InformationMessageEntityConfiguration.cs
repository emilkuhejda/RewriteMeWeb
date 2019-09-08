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
            builder.Property(x => x.DateCreated).IsRequired();

            builder.HasMany(x => x.LanguageVersions).WithOne(x => x.InformationMessage).HasForeignKey(x => x.InformationMessageId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.OpenedMessages).WithOne(x => x.InformationMessage).HasForeignKey(x => x.InformationMessageId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
