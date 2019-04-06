﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RewriteMe.DataAccess;

namespace RewriteMe.DataAccess.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20190406121614_AddLanguageColumnToFileItemEntity")]
    partial class AddLanguageColumnToFileItemEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.ApplicationLogEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated");

                    b.Property<int>("LogLevel");

                    b.Property<string>("Message")
                        .IsRequired();

                    b.Property<Guid?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ApplicationLog");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.AudioSourceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<Guid>("FileItemId");

                    b.Property<byte[]>("OriginalSource")
                        .IsRequired();

                    b.Property<TimeSpan>("TotalTime");

                    b.Property<byte[]>("WavSource");

                    b.HasKey("Id");

                    b.HasIndex("FileItemId")
                        .IsUnique();

                    b.ToTable("AudioSource");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.FileItemEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateProcessed");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<int>("RecognitionState");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("FileItem");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.TranscribeItemEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alternatives")
                        .IsRequired();

                    b.Property<DateTime>("DateCreated");

                    b.Property<Guid>("FileItemId");

                    b.Property<byte[]>("Source")
                        .IsRequired();

                    b.Property<TimeSpan>("TotalTime");

                    b.Property<string>("UserTranscript");

                    b.HasKey("Id");

                    b.HasIndex("FileItemId");

                    b.ToTable("TranscribeItem");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.UserSubscriptionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<TimeSpan>("Time");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserSubscription");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.ApplicationLogEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.UserEntity", "User")
                        .WithMany("ApplicationLogs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.AudioSourceEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.FileItemEntity", "FileItem")
                        .WithOne("AudioSource")
                        .HasForeignKey("RewriteMe.DataAccess.Entities.AudioSourceEntity", "FileItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.FileItemEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.UserEntity", "User")
                        .WithMany("FileItems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.TranscribeItemEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.FileItemEntity", "FileItem")
                        .WithMany("TranscribeItems")
                        .HasForeignKey("FileItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.UserSubscriptionEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.UserEntity", "User")
                        .WithMany("UserSubscriptions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
