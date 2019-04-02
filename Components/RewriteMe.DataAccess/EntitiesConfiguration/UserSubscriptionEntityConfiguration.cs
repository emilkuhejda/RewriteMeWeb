using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class UserSubscriptionEntityConfiguration : IEntityTypeConfiguration<UserSubscriptionEntity>
    {
        public void Configure(EntityTypeBuilder<UserSubscriptionEntity> builder)
        {
            builder.ToTable("UserSubscription");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Time).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired();
        }
    }
}
