using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class DeletedUserEntityConfiguration : IEntityTypeConfiguration<DeletedUserEntity>
    {
        public void Configure(EntityTypeBuilder<DeletedUserEntity> builder)
        {
            builder.ToTable("DeletedUser");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
        }
    }
}
