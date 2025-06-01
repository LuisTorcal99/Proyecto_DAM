using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestAPI.Migrations
{
    /// <inheritdoc />
    public partial class ids : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Asignaturas_IdAsignatura",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_Notas_Asignaturas_AsignaturaId",
                table: "Notas");

            migrationBuilder.DropForeignKey(
                name: "FK_Notas_Eventos_IdEvento",
                table: "Notas");

            migrationBuilder.DropIndex(
                name: "IX_Notas_AsignaturaId",
                table: "Notas");

            migrationBuilder.DropIndex(
                name: "IX_Notas_IdEvento",
                table: "Notas");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_IdAsignatura",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "AsignaturaId",
                table: "Notas");

            migrationBuilder.AddColumn<int>(
                name: "AsignaturaEntityId",
                table: "Notas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AsignaturaEntityId",
                table: "Eventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Nota",
                table: "Eventos",
                type: "float",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notas_AsignaturaEntityId",
                table: "Notas",
                column: "AsignaturaEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_AsignaturaEntityId",
                table: "Eventos",
                column: "AsignaturaEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Asignaturas_AsignaturaEntityId",
                table: "Eventos",
                column: "AsignaturaEntityId",
                principalTable: "Asignaturas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_Asignaturas_AsignaturaEntityId",
                table: "Notas",
                column: "AsignaturaEntityId",
                principalTable: "Asignaturas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Asignaturas_AsignaturaEntityId",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_Notas_Asignaturas_AsignaturaEntityId",
                table: "Notas");

            migrationBuilder.DropIndex(
                name: "IX_Notas_AsignaturaEntityId",
                table: "Notas");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_AsignaturaEntityId",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "AsignaturaEntityId",
                table: "Notas");

            migrationBuilder.DropColumn(
                name: "AsignaturaEntityId",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "Nota",
                table: "Eventos");

            migrationBuilder.AddColumn<int>(
                name: "AsignaturaId",
                table: "Notas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notas_AsignaturaId",
                table: "Notas",
                column: "AsignaturaId");

            migrationBuilder.CreateIndex(
                name: "IX_Notas_IdEvento",
                table: "Notas",
                column: "IdEvento",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_IdAsignatura",
                table: "Eventos",
                column: "IdAsignatura");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Asignaturas_IdAsignatura",
                table: "Eventos",
                column: "IdAsignatura",
                principalTable: "Asignaturas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_Asignaturas_AsignaturaId",
                table: "Notas",
                column: "AsignaturaId",
                principalTable: "Asignaturas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notas_Eventos_IdEvento",
                table: "Notas",
                column: "IdEvento",
                principalTable: "Eventos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
