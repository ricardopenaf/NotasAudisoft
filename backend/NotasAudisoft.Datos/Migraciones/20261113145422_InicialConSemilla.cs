using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NotasAudisoft.Datos.Migraciones
{
    /// <inheritdoc />
    public partial class InicialConSemilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estudiante",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEstudiante = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudiante", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreMateria = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profesor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreProfesor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nota",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMateria = table.Column<int>(type: "int", nullable: false),
                    IdProfesor = table.Column<int>(type: "int", nullable: false),
                    IdEstudiante = table.Column<int>(type: "int", nullable: false),
                    ValorNota = table.Column<decimal>(type: "decimal(3,1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nota", x => x.Id);
                    table.CheckConstraint("CK_Nota_ValorNota_Rango", "[ValorNota] >= 0 AND [ValorNota] <= 5");
                    table.ForeignKey(
                        name: "FK_Nota_Estudiante_IdEstudiante",
                        column: x => x.IdEstudiante,
                        principalTable: "Estudiante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nota_Materia_IdMateria",
                        column: x => x.IdMateria,
                        principalTable: "Materia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Nota_Profesor_IdProfesor",
                        column: x => x.IdProfesor,
                        principalTable: "Profesor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Estudiante",
                columns: new[] { "Id", "NombreEstudiante" },
                values: new object[,]
                {
                    { 1, "Ana María Gómez" },
                    { 2, "Carlos Pérez" },
                    { 3, "Luisa Fernández" },
                    { 4, "Juan Rodríguez" }
                });

            migrationBuilder.InsertData(
                table: "Materia",
                columns: new[] { "Id", "NombreMateria" },
                values: new object[,]
                {
                    { 1, "Matemáticas" },
                    { 2, "Lengua Castellana" },
                    { 3, "Ciencias Naturales" },
                    { 4, "Inglés" }
                });

            migrationBuilder.InsertData(
                table: "Profesor",
                columns: new[] { "Id", "NombreProfesor" },
                values: new object[,]
                {
                    { 1, "Jorge Ramírez" },
                    { 2, "Patricia Londoño" },
                    { 3, "Andrés Torres" }
                });

            migrationBuilder.InsertData(
                table: "Nota",
                columns: new[] { "Id", "IdEstudiante", "IdMateria", "IdProfesor", "ValorNota" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, 4.5m },
                    { 2, 1, 1, 1, 3.8m },
                    { 3, 1, 2, 2, 4.0m },
                    { 4, 2, 1, 1, 2.5m },
                    { 5, 2, 3, 3, 3.2m },
                    { 6, 3, 2, 2, 5.0m },
                    { 7, 3, 4, 3, 4.2m },
                    { 8, 4, 1, 1, 3.0m },
                    { 9, 4, 4, 3, 4.8m },
                    { 10, 2, 1, 1, 3.9m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nota_IdEstudiante",
                table: "Nota",
                column: "IdEstudiante");

            migrationBuilder.CreateIndex(
                name: "IX_Nota_IdMateria",
                table: "Nota",
                column: "IdMateria");

            migrationBuilder.CreateIndex(
                name: "IX_Nota_IdProfesor",
                table: "Nota",
                column: "IdProfesor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nota");

            migrationBuilder.DropTable(
                name: "Estudiante");

            migrationBuilder.DropTable(
                name: "Materia");

            migrationBuilder.DropTable(
                name: "Profesor");
        }
    }
}
