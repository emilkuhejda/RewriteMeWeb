﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.EntitiesConfiguration
{
    public class FileItemEntityConfiguration : IEntityTypeConfiguration<FileItemEntity>
    {
        public void Configure(EntityTypeBuilder<FileItemEntity> builder)
        {
            builder.ToTable("FileItem");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
            builder.Property(x => x.FileName).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Source).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired();
        }
    }
}
