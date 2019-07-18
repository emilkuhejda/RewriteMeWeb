using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RewriteMe.DataAccess.Migrations
{
    public partial class AddRecognizedAudioSample : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecognizedAudioSample",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecognizedAudioSample", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpeechResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RecognizedAudioSampleId = table.Column<Guid>(nullable: false),
                    DisplayText = table.Column<string>(nullable: false),
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
                name: "IX_SpeechResult_RecognizedAudioSampleId",
                table: "SpeechResult",
                column: "RecognizedAudioSampleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpeechResult");

            migrationBuilder.DropTable(
                name: "RecognizedAudioSample");
        }
    }
}
