using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmpresaApi.Migrations
{
    /// <inheritdoc />
    public partial class AjusteArquitecturaAuditoriaYValidaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "Empleados");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Empleados",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Correo",
                table: "Empleados",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ApellidoMaterno",
                table: "Empleados",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApellidoPaterno",
                table: "Empleados",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaActualizacion",
                table: "Empleados",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaBaja",
                table: "Empleados",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Empleados",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_Correo",
                table: "Empleados",
                column: "Correo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Empleados_Correo",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "ApellidoMaterno",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "ApellidoPaterno",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "FechaActualizacion",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "FechaBaja",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Empleados");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Empleados",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Correo",
                table: "Empleados",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "Empleados",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
