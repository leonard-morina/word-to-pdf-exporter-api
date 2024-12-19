using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WordToPdfExporter.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentExports",
                columns: table => new
                {
                    DocumentExportRequestId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RequestHost = table.Column<string>(type: "TEXT", nullable: true),
                    RequestBody = table.Column<string>(type: "TEXT", nullable: true),
                    RequestedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProcessedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentExports", x => x.DocumentExportRequestId);
                });

            migrationBuilder.CreateTable(
                name: "CurrentDocumentExports",
                columns: table => new
                {
                    CurrentDocumentExportId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DocumentExportRequestId = table.Column<int>(type: "INTEGER", nullable: false),
                    RequestHost = table.Column<string>(type: "TEXT", nullable: true),
                    RequestBody = table.Column<string>(type: "TEXT", nullable: true),
                    RequestedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProcessedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentDocumentExports", x => x.CurrentDocumentExportId);
                    table.ForeignKey(
                        name: "FK_CurrentDocumentExports_DocumentExports_DocumentExportRequestId",
                        column: x => x.DocumentExportRequestId,
                        principalTable: "DocumentExports",
                        principalColumn: "DocumentExportRequestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrentDocumentExports_DocumentExportRequestId",
                table: "CurrentDocumentExports",
                column: "DocumentExportRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrentDocumentExports");

            migrationBuilder.DropTable(
                name: "DocumentExports");
        }
    }
}
