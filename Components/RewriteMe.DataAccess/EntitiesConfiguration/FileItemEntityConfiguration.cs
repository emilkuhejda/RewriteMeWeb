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
            builder.Property(x => x.ApplicationId).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
            builder.Property(x => x.FileName).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Language).HasMaxLength(20);
            builder.Property(x => x.RecognitionState).IsRequired();
            builder.Property(x => x.OriginalSourceFileName).HasMaxLength(100);
            builder.Property(x => x.SourceFileName).HasMaxLength(100);
            builder.Property(x => x.Storage).IsRequired();
            builder.Property(x => x.UploadStatus).IsRequired();
            builder.Property(x => x.TotalTime).IsRequired();
            builder.Property(x => x.TranscribedTime).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired();
            builder.Property(x => x.DateUpdatedUtc).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.IsPermanentlyDeleted).IsRequired();
            builder.Property(x => x.WasCleaned).IsRequired();

            builder.HasOne(x => x.FileItemSource).WithOne(x => x.FileItem).HasForeignKey<FileItemSourceEntity>(x => x.FileItemId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.TranscribeItems).WithOne(x => x.FileItem).HasForeignKey(x => x.FileItemId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.TranscribeItemSources).WithOne(x => x.FileItem).HasForeignKey(x => x.FileItemId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.WavPartialFiles).WithOne(x => x.FileItem).HasForeignKey(x => x.FileItemId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
