using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    GivenName = table.Column<string>(maxLength: 100, nullable: false),
                    FamilyName = table.Column<string>(maxLength: 100, nullable: false)
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
                name: "FileItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    FileName = table.Column<string>(maxLength: 150, nullable: false),
                    Language = table.Column<string>(maxLength: 20, nullable: false),
                    RecognitionState = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateProcessed = table.Column<DateTime>(nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    AudioSourceVersion = table.Column<int>(nullable: false)
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
                name: "UserSubscription",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
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
                name: "AudioSource",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileItemId = table.Column<Guid>(nullable: false),
                    OriginalSource = table.Column<byte[]>(nullable: false),
                    WavSource = table.Column<byte[]>(nullable: true),
                    ContentType = table.Column<string>(maxLength: 50, nullable: false),
                    TotalTime = table.Column<TimeSpan>(nullable: false),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioSource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AudioSource_FileItem_FileItemId",
                        column: x => x.FileItemId,
                        principalTable: "FileItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TranscribeItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileItemId = table.Column<Guid>(nullable: false),
                    Alternatives = table.Column<string>(nullable: false),
                    UserTranscript = table.Column<string>(nullable: true),
                    Source = table.Column<byte[]>(nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationLog_UserId",
                table: "ApplicationLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AudioSource_FileItemId",
                table: "AudioSource",
                column: "FileItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileItem_UserId",
                table: "FileItem",
                column: "UserId");

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
                name: "ApplicationLog");

            migrationBuilder.DropTable(
                name: "AudioSource");

            migrationBuilder.DropTable(
                name: "TranscribeItem");

            migrationBuilder.DropTable(
                name: "UserSubscription");

            migrationBuilder.DropTable(
                name: "FileItem");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
