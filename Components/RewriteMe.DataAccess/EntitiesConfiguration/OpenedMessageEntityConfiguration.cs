using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class OpenedMessageEntityConfiguration : IEntityTypeConfiguration<OpenedMessageEntity>
    {
        public void Configure(EntityTypeBuilder<OpenedMessageEntity> builder)
        {
            builder.ToTable("OpenedMessage");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.InformationMessageId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
        }
    }
}
