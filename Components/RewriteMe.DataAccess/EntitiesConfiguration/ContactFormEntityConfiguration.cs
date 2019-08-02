using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class ContactFormEntityConfiguration : IEntityTypeConfiguration<ContactFormEntity>
    {
        public void Configure(EntityTypeBuilder<ContactFormEntity> builder)
        {
            builder.ToTable("ContactForm");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Message).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired();
        }
    }
}
