using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestAPI.Migrations
{
    /// <inheritdoc />
    public partial class faltas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Faltas",
                table: "Asignaturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Horas",
                table: "Asignaturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PorcentajeFaltas",
                table: "Asignaturas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Faltas",
                table: "Asignaturas");

            migrationBuilder.DropColumn(
                name: "Horas",
                table: "Asignaturas");

            migrationBuilder.DropColumn(
                name: "PorcentajeFaltas",
                table: "Asignaturas");
        }
    }
}
