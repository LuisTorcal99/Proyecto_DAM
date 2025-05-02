using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestAPI.Migrations
{
    /// <inheritdoc />
    public partial class news : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BienestarEstudiantil",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoDeAnimo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NivelDeEstres = table.Column<int>(type: "int", nullable: false),
                    Sugerencias = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BienestarEstudiantil", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gamificacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoDeLogro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Puntos = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gamificacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistroTiempoEstudiado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AsignaturaID = table.Column<int>(type: "int", nullable: false),
                    TiempoEstudiado = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroTiempoEstudiado", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BienestarEstudiantil");

            migrationBuilder.DropTable(
                name: "Gamificacion");

            migrationBuilder.DropTable(
                name: "RegistroTiempoEstudiado");
        }
    }
}
