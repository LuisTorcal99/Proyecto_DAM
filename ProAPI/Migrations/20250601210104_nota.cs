using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestAPI.Migrations
{
    /// <inheritdoc />
    public partial class nota : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AsignaturaId",
                table: "Notas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdAsignatura",
                table: "Notas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notas_AsignaturaId",
                table: "Notas",
                column: "AsignaturaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_Asignaturas_AsignaturaId",
                table: "Notas",
                column: "AsignaturaId",
                principalTable: "Asignaturas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notas_Asignaturas_AsignaturaId",
                table: "Notas");

            migrationBuilder.DropIndex(
                name: "IX_Notas_AsignaturaId",
                table: "Notas");

            migrationBuilder.DropColumn(
                name: "AsignaturaId",
                table: "Notas");

            migrationBuilder.DropColumn(
                name: "IdAsignatura",
                table: "Notas");
        }
    }
}
