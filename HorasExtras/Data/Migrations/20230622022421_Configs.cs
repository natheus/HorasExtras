using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleDeHorasExtras.Migrations
{
    /// <inheritdoc />
    public partial class Configs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salario",
                table: "HorasExtras");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Salario",
                table: "HorasExtras",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
