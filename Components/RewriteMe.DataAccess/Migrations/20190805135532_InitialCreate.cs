﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrator",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    PasswordHash = table.Column<byte[]>(nullable: false),
                    PasswordSalt = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactForm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    Email = table.Column<string>(maxLength: 150, nullable: false),
                    Message = table.Column<string>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactForm", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    GivenName = table.Column<string>(maxLength: 100, nullable: false),
                    FamilyName = table.Column<string>(maxLength: 100, nullable: false),
                    ApplicationId = table.Column<Guid>(nullable: false),
                    DateRegistered = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: true),
                    LogLevel = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationLog_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillingPurchase",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    PurchaseId = table.Column<string>(nullable: false),
                    ProductId = table.Column<string>(maxLength: 100, nullable: false),
                    AutoRenewing = table.Column<bool>(nullable: false),
                    PurchaseState = table.Column<string>(maxLength: 100, nullable: false),
                    ConsumptionState = table.Column<string>(maxLength: 100, nullable: false),
                    Platform = table.Column<string>(maxLength: 50, nullable: false),
                    TransactionDateUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingPurchase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillingPurchase_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ApplicationId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    FileName = table.Column<string>(maxLength: 150, nullable: false),
                    Language = table.Column<string>(maxLength: 20, nullable: true),
                    RecognitionState = table.Column<int>(nullable: false),
                    OriginalSourceFileName = table.Column<string>(maxLength: 100, nullable: false),
                    SourceFileName = table.Column<string>(maxLength: 100, nullable: true),
                    OriginalContentType = table.Column<string>(maxLength: 100, nullable: false),
                    TotalTime = table.Column<TimeSpan>(nullable: false),
                    TranscribedTime = table.Column<TimeSpan>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateProcessed = table.Column<DateTime>(nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileItem_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecognizedAudioSample",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecognizedAudioSample", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecognizedAudioSample_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSubscription",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ApplicationId = table.Column<Guid>(nullable: false),
                    Time = table.Column<TimeSpan>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSubscription_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TranscribeItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileItemId = table.Column<Guid>(nullable: false),
                    ApplicationId = table.Column<Guid>(nullable: false),
                    Alternatives = table.Column<string>(nullable: false),
                    UserTranscript = table.Column<string>(nullable: true),
                    SourceFileName = table.Column<string>(maxLength: 100, nullable: false),
                    StartTime = table.Column<TimeSpan>(nullable: false),
                    EndTime = table.Column<TimeSpan>(nullable: false),
                    TotalTime = table.Column<TimeSpan>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranscribeItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TranscribeItem_FileItem_FileItemId",
                        column: x => x.FileItemId,
                        principalTable: "FileItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpeechResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RecognizedAudioSampleId = table.Column<Guid>(nullable: false),
                    DisplayText = table.Column<string>(nullable: true),
                    TotalTime = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeechResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpeechResult_RecognizedAudioSample_RecognizedAudioSampleId",
                        column: x => x.RecognizedAudioSampleId,
                        principalTable: "RecognizedAudioSample",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationLog_UserId",
                table: "ApplicationLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BillingPurchase_UserId",
                table: "BillingPurchase",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FileItem_UserId",
                table: "FileItem",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecognizedAudioSample_UserId",
                table: "RecognizedAudioSample",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SpeechResult_RecognizedAudioSampleId",
                table: "SpeechResult",
                column: "RecognizedAudioSampleId");

            migrationBuilder.CreateIndex(
                name: "IX_TranscribeItem_FileItemId",
                table: "TranscribeItem",
                column: "FileItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscription_UserId",
                table: "UserSubscription",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administrator");

            migrationBuilder.DropTable(
                name: "ApplicationLog");

            migrationBuilder.DropTable(
                name: "BillingPurchase");

            migrationBuilder.DropTable(
                name: "ContactForm");

            migrationBuilder.DropTable(
                name: "SpeechResult");

            migrationBuilder.DropTable(
                name: "TranscribeItem");

            migrationBuilder.DropTable(
                name: "UserSubscription");

            migrationBuilder.DropTable(
                name: "RecognizedAudioSample");

            migrationBuilder.DropTable(
                name: "FileItem");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}