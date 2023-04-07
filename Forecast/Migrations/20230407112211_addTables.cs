using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Forecast.Migrations
{
    /// <inheritdoc />
    public partial class addTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Latitude = table.Column<float>(type: "real", nullable: false),
                    Longitude = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Weathers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Dt = table.Column<long>(type: "bigint", nullable: false),
                    Temperature = table.Column<float>(type: "real", nullable: false),
                    TempMin = table.Column<float>(type: "real", nullable: false),
                    TempMax = table.Column<float>(type: "real", nullable: false),
                    Pressure = table.Column<int>(type: "integer", nullable: false),
                    SeaLevel = table.Column<int>(type: "integer", nullable: false),
                    GrndLevel = table.Column<int>(type: "integer", nullable: false),
                    Humidity = table.Column<int>(type: "integer", nullable: false),
                    WindSpeed = table.Column<float>(type: "real", nullable: false),
                    WindDeg = table.Column<float>(type: "real", nullable: false),
                    WindGust = table.Column<float>(type: "real", nullable: false),
                    Clouds = table.Column<int>(type: "integer", nullable: true),
                    Visibility = table.Column<int>(type: "integer", nullable: true),
                    WeatherId = table.Column<int>(type: "integer", nullable: false),
                    WeatherMain = table.Column<string>(type: "text", nullable: true),
                    WeatherDescription = table.Column<string>(type: "text", nullable: true),
                    WeatherIcon = table.Column<string>(type: "text", nullable: true),
                    Rain1h = table.Column<float>(type: "real", nullable: true),
                    Rain3h = table.Column<float>(type: "real", nullable: true),
                    Snow1h = table.Column<float>(type: "real", nullable: true),
                    Snow3h = table.Column<float>(type: "real", nullable: true),
                    icon = table.Column<string>(type: "text", nullable: false),
                    dt_txt = table.Column<string>(type: "text", nullable: false),
                    WeatherLocationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weathers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weathers_WeatherLocations_WeatherLocationId",
                        column: x => x.WeatherLocationId,
                        principalTable: "WeatherLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weathers_WeatherLocationId",
                table: "Weathers",
                column: "WeatherLocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weathers");

            migrationBuilder.DropTable(
                name: "WeatherLocations");
        }
    }
}
