using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WordToPdfExporter.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCurrentDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrentDocumentExports");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrentDocumentExports",
                columns: table => new
                {
                    CurrentDocumentExportId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DocumentExportRequestId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProcessedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RequestBody = table.Column<string>(type: "TEXT", nullable: true),
                    RequestHost = table.Column<string>(type: "TEXT", nullable: true),
                    RequestedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
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
    }
}
