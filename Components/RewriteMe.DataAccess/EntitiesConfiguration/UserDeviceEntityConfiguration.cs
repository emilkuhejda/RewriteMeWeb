using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class UserDeviceEntityConfiguration : IEntityTypeConfiguration<UserDeviceEntity>
    {
        public void Configure(EntityTypeBuilder<UserDeviceEntity> builder)
        {
            builder.ToTable("UserDevice");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.InstallationId).IsRequired();
            builder.Property(x => x.RuntimePlatform).IsRequired();
            builder.Property(x => x.InstalledVersionNumber).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Language).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired();
        }
    }
}
