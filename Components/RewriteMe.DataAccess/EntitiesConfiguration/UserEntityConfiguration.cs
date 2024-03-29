﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("User");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
            builder.Property(x => x.GivenName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.FamilyName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.DateRegisteredUtc).IsRequired();

            builder.HasOne(x => x.CurrentUserSubscription).WithOne(x => x.User).HasForeignKey<CurrentUserSubscriptionEntity>(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.UserSubscriptions).WithOne(x => x.User).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.FileItems).WithOne(x => x.User).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.RecognizedAudioSamples).WithOne(x => x.User).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.BillingPurchases).WithOne(x => x.User).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.InformationMessages).WithOne(x => x.User).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.UserDevices).WithOne(x => x.User).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
