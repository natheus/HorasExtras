using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OvertimeManagement.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "overTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HorarioInicial = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HorarioFinal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Porcentagem = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_overTimes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "overTimes");
        }
    }
}
