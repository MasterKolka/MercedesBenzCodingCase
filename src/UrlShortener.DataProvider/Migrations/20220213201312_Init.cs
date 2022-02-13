using Microsoft.EntityFrameworkCore.Migrations;
using UrlShortener.DataProvider.Helpers;

#nullable disable

namespace UrlShortener.DataProvider.Migrations;

public partial class Init : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "HashingSpace",
            columns: table => new
            {
                Id = table.Column<long>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                RangeStart = table.Column<long>(type: "INTEGER", nullable: false),
                RangeEnd = table.Column<long>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HashingSpace", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ShortenedUrls",
            columns: table => new
            {
                Id = table.Column<long>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Shortened = table.Column<string>(type: "TEXT", nullable: false),
                Url = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ShortenedUrls", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ShortenedUrlAnalytics",
            columns: table => new
            {
                Id = table.Column<long>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                Ip = table.Column<string>(type: "TEXT", nullable: true),
                ShortenedUrlId = table.Column<long>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ShortenedUrlAnalytics", x => x.Id);
                table.ForeignKey(
                    name: "FK_ShortenedUrlAnalytics_ShortenedUrls_ShortenedUrlId",
                    column: x => x.ShortenedUrlId,
                    principalTable: "ShortenedUrls",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_HashingSpace_RangeStart_RangeEnd",
            table: "HashingSpace",
            columns: new[] { "RangeStart", "RangeEnd" });

        migrationBuilder.CreateIndex(
            name: "IX_ShortenedUrlAnalytics_Date",
            table: "ShortenedUrlAnalytics",
            column: "Date");

        migrationBuilder.CreateIndex(
            name: "IX_ShortenedUrlAnalytics_ShortenedUrlId",
            table: "ShortenedUrlAnalytics",
            column: "ShortenedUrlId");

        migrationBuilder.CreateIndex(
            name: "IX_ShortenedUrls_Shortened",
            table: "ShortenedUrls",
            column: "Shortened",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ShortenedUrls_Url",
            table: "ShortenedUrls",
            column: "Url",
            unique: true);

        migrationBuilder.Sql($"INSERT INTO HashingSpace VALUES(null, 0, {HashAlphabetHelper.MaxHashItems - 1})");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "HashingSpace");

        migrationBuilder.DropTable(
            name: "ShortenedUrlAnalytics");

        migrationBuilder.DropTable(
            name: "ShortenedUrls");
    }
}
