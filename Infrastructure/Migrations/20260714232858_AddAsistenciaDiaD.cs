using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAsistenciaDiaD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "asistio",
                table: "operador_persona",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "asistio_marcado_en",
                table: "operador_persona",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "asistio_marcado_por",
                table: "operador_persona",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_operador_persona_asistio_marcado_por",
                table: "operador_persona",
                column: "asistio_marcado_por");

            migrationBuilder.AddForeignKey(
                name: "fk_operador_persona_user_asistio_marcado_por",
                table: "operador_persona",
                column: "asistio_marcado_por",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.InsertData(
                table: "permission",
                columns: new[] { "id", "name", "description" },
                values: new object[,]
                {
                    { 10, "asistencia:read",   "Can read attendance" },
                    { 11, "asistencia:update", "Can update attendance" }
                });

            // Admin (1) and Coordinador (2) get attendance permissions
            migrationBuilder.InsertData(
                table: "permission_role",
                columns: new[] { "permissions_id", "roles_id" },
                values: new object[,]
                {
                    { 10, 1 }, { 10, 2 },
                    { 11, 1 }, { 11, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("permission_role", new[] { "permissions_id", "roles_id" }, new object[] { 10, 1 });
            migrationBuilder.DeleteData("permission_role", new[] { "permissions_id", "roles_id" }, new object[] { 10, 2 });
            migrationBuilder.DeleteData("permission_role", new[] { "permissions_id", "roles_id" }, new object[] { 11, 1 });
            migrationBuilder.DeleteData("permission_role", new[] { "permissions_id", "roles_id" }, new object[] { 11, 2 });
            migrationBuilder.DeleteData("permission", "id", 10);
            migrationBuilder.DeleteData("permission", "id", 11);

            migrationBuilder.DropForeignKey(
                name: "fk_operador_persona_user_asistio_marcado_por",
                table: "operador_persona");

            migrationBuilder.DropIndex(
                name: "ix_operador_persona_asistio_marcado_por",
                table: "operador_persona");

            migrationBuilder.DropColumn(
                name: "asistio",
                table: "operador_persona");

            migrationBuilder.DropColumn(
                name: "asistio_marcado_en",
                table: "operador_persona");

            migrationBuilder.DropColumn(
                name: "asistio_marcado_por",
                table: "operador_persona");
        }
    }
}
