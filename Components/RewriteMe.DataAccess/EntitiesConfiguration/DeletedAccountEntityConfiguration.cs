using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class DeletedAccountEntityConfiguration : IEntityTypeConfiguration<DeletedAccountEntity>
    {
        public void Configure(EntityTypeBuilder<DeletedAccountEntity> builder)
        {
            builder.ToTable("DeletedAccount");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
        }
    }
}
