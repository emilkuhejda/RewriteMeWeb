using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class ApplicationLogEntityConfiguration : IEntityTypeConfiguration<ApplicationLogEntity>
    {
        public void Configure(EntityTypeBuilder<ApplicationLogEntity> builder)
        {
            builder.ToTable("ApplicationLog");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.LogLevel).IsRequired();
            builder.Property(x => x.Message).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired();
        }
    }
}
