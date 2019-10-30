﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RewriteMe.DataAccess;

namespace RewriteMe.DataAccess.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.AdministratorEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired();

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired();

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Administrator");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.ApplicationLogEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreatedUtc");

                    b.Property<int>("LogLevel");

                    b.Property<string>("Message")
                        .IsRequired();

                    b.Property<Guid?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ApplicationLog");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.BillingPurchaseEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AutoRenewing");

                    b.Property<string>("ConsumptionState")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Platform")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("PurchaseId")
                        .IsRequired();

                    b.Property<string>("PurchaseState")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("PurchaseToken")
                        .IsRequired();

                    b.Property<DateTime>("TransactionDateUtc");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("BillingPurchase");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.ContactFormEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreatedUtc");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("Message")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("ContactForm");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.FileItemEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ApplicationId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateProcessedUtc");

                    b.Property<DateTime>("DateUpdatedUtc");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsPermanentlyDeleted");

                    b.Property<string>("Language")
                        .HasMaxLength(20);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("OriginalContentType")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("OriginalSourceFileName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("RecognitionState");

                    b.Property<string>("SourceFileName")
                        .HasMaxLength(100);

                    b.Property<TimeSpan>("TotalTime");

                    b.Property<TimeSpan>("TranscribedTime");

                    b.Property<Guid>("UserId");

                    b.Property<bool>("WasCleaned");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("FileItem");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.FileItemSourceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreatedUtc");

                    b.Property<Guid>("FileItemId");

                    b.Property<byte[]>("OriginalSource")
                        .IsRequired();

                    b.Property<byte[]>("Source");

                    b.HasKey("Id");

                    b.HasIndex("FileItemId")
                        .IsUnique();

                    b.ToTable("FileItemSource");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.InformationMessageEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CampaignName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<DateTime>("DateCreatedUtc");

                    b.Property<DateTime?>("DatePublishedUtc");

                    b.Property<DateTime?>("DateUpdatedUtc");

                    b.Property<Guid?>("UserId");

                    b.Property<bool>("WasOpened");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("InformationMessage");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.InternalValueEntity", b =>
                {
                    b.Property<string>("Key")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Value")
                        .HasMaxLength(100);

                    b.HasKey("Key");

                    b.ToTable("InternalValue");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.LanguageVersionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<Guid>("InformationMessageId");

                    b.Property<int>("Language");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<bool>("SentOnAndroid");

                    b.Property<bool>("SentOnOsx");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.HasIndex("InformationMessageId");

                    b.ToTable("LanguageVersion");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.RecognizedAudioSampleEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreatedUtc");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RecognizedAudioSample");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.SpeechResultEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayText");

                    b.Property<Guid>("RecognizedAudioSampleId");

                    b.Property<TimeSpan>("TotalTime");

                    b.HasKey("Id");

                    b.HasIndex("RecognizedAudioSampleId");

                    b.ToTable("SpeechResult");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.TranscribeItemEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alternatives")
                        .IsRequired();

                    b.Property<Guid>("ApplicationId");

                    b.Property<DateTime>("DateCreatedUtc");

                    b.Property<DateTime>("DateUpdatedUtc");

                    b.Property<TimeSpan>("EndTime");

                    b.Property<Guid>("FileItemId");

                    b.Property<string>("SourceFileName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<TimeSpan>("StartTime");

                    b.Property<TimeSpan>("TotalTime");

                    b.Property<string>("UserTranscript");

                    b.HasKey("Id");

                    b.HasIndex("FileItemId");

                    b.ToTable("TranscribeItem");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.TranscribeItemSourceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreatedUtc");

                    b.Property<Guid>("FileItemId");

                    b.Property<byte[]>("Source")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("FileItemId");

                    b.ToTable("TranscribeItemSource");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.UserDeviceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateRegisteredUtc");

                    b.Property<Guid>("InstallationId");

                    b.Property<string>("InstalledVersionNumber")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<int>("Language");

                    b.Property<int>("RuntimePlatform");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserDevice");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateRegisteredUtc");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("FamilyName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("GivenName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.UserSubscriptionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ApplicationId");

                    b.Property<DateTime>("DateCreatedUtc");

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

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.BillingPurchaseEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.UserEntity", "User")
                        .WithMany("BillingPurchases")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.FileItemEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.UserEntity", "User")
                        .WithMany("FileItems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.FileItemSourceEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.FileItemEntity", "FileItem")
                        .WithOne("FileItemSource")
                        .HasForeignKey("RewriteMe.DataAccess.Entities.FileItemSourceEntity", "FileItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.InformationMessageEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.UserEntity", "User")
                        .WithMany("InformationMessages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.LanguageVersionEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.InformationMessageEntity", "InformationMessage")
                        .WithMany("LanguageVersions")
                        .HasForeignKey("InformationMessageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.RecognizedAudioSampleEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.UserEntity", "User")
                        .WithMany("RecognizedAudioSamples")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.SpeechResultEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.RecognizedAudioSampleEntity", "RecognizedAudioSample")
                        .WithMany("SpeechResults")
                        .HasForeignKey("RecognizedAudioSampleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.TranscribeItemEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.FileItemEntity", "FileItem")
                        .WithMany("TranscribeItems")
                        .HasForeignKey("FileItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.TranscribeItemSourceEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.FileItemEntity", "FileItem")
                        .WithMany("TranscribeItemSources")
                        .HasForeignKey("FileItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.UserDeviceEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.UserEntity", "User")
                        .WithMany("UserDevices")
                        .HasForeignKey("UserId")
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
