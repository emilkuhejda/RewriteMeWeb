using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class BillingPurchaseEntityConfiguration : IEntityTypeConfiguration<BillingPurchaseEntity>
    {
        public void Configure(EntityTypeBuilder<BillingPurchaseEntity> builder)
        {
            builder.ToTable("BillingPurchase");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.PurchaseId).IsRequired();
            builder.Property(x => x.ProductId).IsRequired().HasMaxLength(100);
            builder.Property(x => x.AutoRenewing).IsRequired();
            builder.Property(x => x.PurchaseState).IsRequired().HasMaxLength(100);
            builder.Property(x => x.ConsumptionState).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Platform).IsRequired().HasMaxLength(50);
            builder.Property(x => x.TransactionDateUtc).IsRequired();
        }
    }
}
