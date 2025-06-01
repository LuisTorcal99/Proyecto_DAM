using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestAPI.Migrations
{
    /// <inheritdoc />
    public partial class cascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notas_Asignaturas_IdAsignatura",
                table: "Notas");

            migrationBuilder.DropIndex(
                name: "IX_Notas_IdAsignatura",
                table: "Notas");

            migrationBuilder.DropColumn(
                name: "IdAsignatura",
                table: "Notas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdAsignatura",
                table: "Notas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notas_IdAsignatura",
                table: "Notas",
                column: "IdAsignatura");

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_Asignaturas_IdAsignatura",
                table: "Notas",
                column: "IdAsignatura",
                principalTable: "Asignaturas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
