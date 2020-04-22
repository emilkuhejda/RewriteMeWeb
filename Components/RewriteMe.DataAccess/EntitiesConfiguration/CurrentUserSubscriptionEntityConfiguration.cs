using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class CurrentUserSubscriptionEntityConfiguration : IEntityTypeConfiguration<CurrentUserSubscriptionEntity>
    {
        public void Configure(EntityTypeBuilder<CurrentUserSubscriptionEntity> builder)
        {
            builder.ToTable("CurrentUserSubscription");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Ticks).IsRequired();
            builder.Property(x => x.DateUpdatedUtc).IsRequired();
        }
    }
}
