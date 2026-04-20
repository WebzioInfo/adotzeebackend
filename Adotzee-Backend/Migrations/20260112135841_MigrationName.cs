using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adotzee_Backend.Migrations
{
    /// <inheritdoc />
    public partial class MigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Colleges");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Colleges",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Colleges",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleMapsUrl",
                table: "Colleges",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecommended",
                table: "Colleges",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Colleges",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Colleges",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "PlaceId",
                table: "Colleges",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Colleges");

            migrationBuilder.DropColumn(
                name: "GoogleMapsUrl",
                table: "Colleges");

            migrationBuilder.DropColumn(
                name: "IsRecommended",
                table: "Colleges");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Colleges");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Colleges");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "Colleges");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Colleges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Colleges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
