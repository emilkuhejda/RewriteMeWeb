using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class InternalValueEntityConfiguration : IEntityTypeConfiguration<InternalValueEntity>
    {
        public void Configure(EntityTypeBuilder<InternalValueEntity> builder)
        {
            builder.ToTable("InternalValue");

            builder.HasKey(x => x.Key);
            builder.Property(x => x.Value).HasMaxLength(100);
        }
    }
}
