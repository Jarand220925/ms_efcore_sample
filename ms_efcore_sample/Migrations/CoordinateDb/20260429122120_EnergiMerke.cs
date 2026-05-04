using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ms_efcore_sample.Migrations.CoordinateDb
{
    /// <inheritdoc />
    public partial class EnergiMerke : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnergiMerke",
                columns: table => new
                {
                    EnergiMerkeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VurderingsGrad = table.Column<int>(type: "integer", nullable: false),
                    EiendomId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergiMerke", x => x.EnergiMerkeId);
                    table.ForeignKey(
                        name: "FK_EnergiMerke_Eiendommer_EiendomId",
                        column: x => x.EiendomId,
                        principalTable: "Eiendommer",
                        principalColumn: "EiendomId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Eiendommer_CoordinateId",
                table: "Eiendommer",
                column: "CoordinateId");

            migrationBuilder.CreateIndex(
                name: "IX_Eiendommer_KommuneId",
                table: "Eiendommer",
                column: "KommuneId");

            migrationBuilder.CreateIndex(
                name: "IX_EnergiMerke_EiendomId",
                table: "EnergiMerke",
                column: "EiendomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Eiendommer_Coordinates_CoordinateId",
                table: "Eiendommer",
                column: "CoordinateId",
                principalTable: "Coordinates",
                principalColumn: "CoordinateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Eiendommer_Kommuner_KommuneId",
                table: "Eiendommer",
                column: "KommuneId",
                principalTable: "Kommuner",
                principalColumn: "KommuneId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eiendommer_Coordinates_CoordinateId",
                table: "Eiendommer");

            migrationBuilder.DropForeignKey(
                name: "FK_Eiendommer_Kommuner_KommuneId",
                table: "Eiendommer");

            migrationBuilder.DropTable(
                name: "EnergiMerke");

            migrationBuilder.DropIndex(
                name: "IX_Eiendommer_CoordinateId",
                table: "Eiendommer");

            migrationBuilder.DropIndex(
                name: "IX_Eiendommer_KommuneId",
                table: "Eiendommer");
        }
    }
}
