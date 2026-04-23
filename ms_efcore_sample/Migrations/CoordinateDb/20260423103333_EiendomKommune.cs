using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ms_efcore_sample.Migrations.CoordinateDb
{
    /// <inheritdoc />
    public partial class EiendomKommune : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Eiendommer",
                columns: table => new
                {
                    EiendomId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KommuneId = table.Column<int>(type: "integer", nullable: false),
                    CoordinateId = table.Column<int>(type: "integer", nullable: true),
                    ByggType = table.Column<string>(type: "text", nullable: false),
                    Adresse = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eiendommer", x => x.EiendomId);
                });

            migrationBuilder.CreateTable(
                name: "Kommuner",
                columns: table => new
                {
                    KommuneId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Navn = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kommuner", x => x.KommuneId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Eiendommer");

            migrationBuilder.DropTable(
                name: "Kommuner");
        }
    }
}
