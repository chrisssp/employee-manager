using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmpresaApi.Migrations
{
    /// <inheritdoc />
    public partial class ConfiguracionFluentApiProfesional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Empleados_Correo",
                table: "Empleados",
                newName: "IX_Empleado_Correo_Unico");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCreacion",
                table: "Empleados",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "Activo",
                table: "Empleados",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Empleado_Correo_Unico",
                table: "Empleados",
                newName: "IX_Empleados_Correo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCreacion",
                table: "Empleados",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<bool>(
                name: "Activo",
                table: "Empleados",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);
        }
    }
}
