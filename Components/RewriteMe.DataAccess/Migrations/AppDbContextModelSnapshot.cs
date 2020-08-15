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
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.AdministratorEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Administrator");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.BillingPurchaseEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("AutoRenewing")
                        .HasColumnType("bit");

                    b.Property<string>("ConsumptionState")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Platform")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("PurchaseId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PurchaseState")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("PurchaseToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TransactionDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("BillingPurchase");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.ContactFormEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("ContactForm");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.CurrentUserSubscriptionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateUpdatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<long>("Ticks")
                        .HasColumnType("bigint");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("CurrentUserSubscription");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.DeletedAccountEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateDeleted")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("DeletedAccount");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.FileItemEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateProcessedUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPermanentlyDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Language")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<string>("OriginalSourceFileName")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("RecognitionState")
                        .HasColumnType("int");

                    b.Property<string>("SourceFileName")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("Storage")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("TotalTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("TranscribedTime")
                        .HasColumnType("time");

                    b.Property<int>("UploadStatus")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("WasCleaned")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("FileItem");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.FileItemSourceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("FileItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("OriginalSource")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("Source")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.HasIndex("FileItemId")
                        .IsUnique();

                    b.ToTable("FileItemSource");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.InformationMessageEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CampaignName")
                        .IsRequired()
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.Property<DateTime>("DateCreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DatePublishedUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateUpdatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("WasOpened")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("InformationMessage");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.InternalValueEntity", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Key");

                    b.ToTable("InternalValue");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.LanguageVersionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("InformationMessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Language")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<bool>("SentOnAndroid")
                        .HasColumnType("bit");

                    b.Property<bool>("SentOnOsx")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(150)")
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.HasIndex("InformationMessageId");

                    b.ToTable("LanguageVersion");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.RecognizedAudioSampleEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RecognizedAudioSample");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.SpeechResultEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DisplayText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RecognizedAudioSampleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeSpan>("TotalTime")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("RecognizedAudioSampleId");

                    b.ToTable("SpeechResult");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.TranscribeItemEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Alternatives")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<Guid>("FileItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SourceFileName")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.Property<int>("Storage")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("TotalTime")
                        .HasColumnType("time");

                    b.Property<string>("UserTranscript")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FileItemId");

                    b.ToTable("TranscribeItem");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.TranscribeItemSourceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("FileItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Source")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.HasIndex("FileItemId");

                    b.ToTable("TranscribeItemSource");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.UploadedChunkEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("FileItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<byte[]>("Source")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.ToTable("UploadedChunk");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.UserDeviceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateRegisteredUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("InstallationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("InstalledVersionNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<int>("Language")
                        .HasColumnType("int");

                    b.Property<int>("RuntimePlatform")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserDevice");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateRegisteredUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("FamilyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("GivenName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.UserSubscriptionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("Operation")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserSubscription");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.WavPartialFileEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AudioChannels")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<Guid>("FileItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("TotalTime")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("FileItemId");

                    b.ToTable("WavPartialFile");
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.BillingPurchaseEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.UserEntity", "User")
                        .WithMany("BillingPurchases")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.CurrentUserSubscriptionEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.UserEntity", "User")
                        .WithOne("CurrentUserSubscription")
                        .HasForeignKey("RewriteMe.DataAccess.Entities.CurrentUserSubscriptionEntity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.FileItemEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.UserEntity", "User")
                        .WithMany("FileItems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.FileItemSourceEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.FileItemEntity", "FileItem")
                        .WithOne("FileItemSource")
                        .HasForeignKey("RewriteMe.DataAccess.Entities.FileItemSourceEntity", "FileItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.RecognizedAudioSampleEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.UserEntity", "User")
                        .WithMany("RecognizedAudioSamples")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.SpeechResultEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.RecognizedAudioSampleEntity", "RecognizedAudioSample")
                        .WithMany("SpeechResults")
                        .HasForeignKey("RecognizedAudioSampleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.TranscribeItemEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.FileItemEntity", "FileItem")
                        .WithMany("TranscribeItems")
                        .HasForeignKey("FileItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.TranscribeItemSourceEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.FileItemEntity", "FileItem")
                        .WithMany("TranscribeItemSources")
                        .HasForeignKey("FileItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.UserDeviceEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.UserEntity", "User")
                        .WithMany("UserDevices")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.UserSubscriptionEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.UserEntity", "User")
                        .WithMany("UserSubscriptions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RewriteMe.DataAccess.Entities.WavPartialFileEntity", b =>
                {
                    b.HasOne("RewriteMe.DataAccess.Entities.FileItemEntity", "FileItem")
                        .WithMany("WavPartialFiles")
                        .HasForeignKey("FileItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
