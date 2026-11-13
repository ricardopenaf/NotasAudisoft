using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotasAudisoft.Datos.Migraciones
{
    /// <inheritdoc />
    public partial class AgregarNumeroIdentificacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "NumeroIdentificacion",
                table: "Profesor",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "NumeroIdentificacion",
                table: "Estudiante",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "Estudiante",
                keyColumn: "Id",
                keyValue: 1,
                column: "NumeroIdentificacion",
                value: 1001L);

            migrationBuilder.UpdateData(
                table: "Estudiante",
                keyColumn: "Id",
                keyValue: 2,
                column: "NumeroIdentificacion",
                value: 1002L);

            migrationBuilder.UpdateData(
                table: "Estudiante",
                keyColumn: "Id",
                keyValue: 3,
                column: "NumeroIdentificacion",
                value: 1003L);

            migrationBuilder.UpdateData(
                table: "Estudiante",
                keyColumn: "Id",
                keyValue: 4,
                column: "NumeroIdentificacion",
                value: 1004L);

            migrationBuilder.UpdateData(
                table: "Profesor",
                keyColumn: "Id",
                keyValue: 1,
                column: "NumeroIdentificacion",
                value: 2001L);

            migrationBuilder.UpdateData(
                table: "Profesor",
                keyColumn: "Id",
                keyValue: 2,
                column: "NumeroIdentificacion",
                value: 2002L);

            migrationBuilder.UpdateData(
                table: "Profesor",
                keyColumn: "Id",
                keyValue: 3,
                column: "NumeroIdentificacion",
                value: 2003L);

            migrationBuilder.CreateIndex(
                name: "IX_Profesor_NumeroIdentificacion",
                table: "Profesor",
                column: "NumeroIdentificacion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Estudiante_NumeroIdentificacion",
                table: "Estudiante",
                column: "NumeroIdentificacion",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Profesor_NumeroIdentificacion",
                table: "Profesor");

            migrationBuilder.DropIndex(
                name: "IX_Estudiante_NumeroIdentificacion",
                table: "Estudiante");

            migrationBuilder.DropColumn(
                name: "NumeroIdentificacion",
                table: "Profesor");

            migrationBuilder.DropColumn(
                name: "NumeroIdentificacion",
                table: "Estudiante");
        }
    }
}
