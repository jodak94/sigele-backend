using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedConsultasReadPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "permission",
                columns: new[] { "id", "name", "description" },
                values: new object[] { 5, "consultas:read", "Can read consultas statistics" });

            migrationBuilder.InsertData(
                table: "permission_role",
                columns: new[] { "permissions_id", "roles_id" },
                values: new object[,]
                {
                    { 5, 1 }, // Admin
                    { 5, 2 }, // Coordinador
                    { 5, 3 }  // Operador
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "permission_role",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "permission_role",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "permission_role",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "permission",
                keyColumn: "id",
                keyValue: 5);
        }
    }
}
