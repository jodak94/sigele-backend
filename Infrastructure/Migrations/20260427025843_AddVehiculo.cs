using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVehiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "vehiculo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    capacidad = table.Column<int>(type: "integer", nullable: false),
                    nombre_dueno = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    telefono_dueno = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    operador_id = table.Column<int>(type: "integer", nullable: true),
                    monto_alquiler = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    observacion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    createt_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<int>(type: "integer", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    tenant_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehiculo", x => x.id);
                    table.ForeignKey(
                        name: "fk_vehiculo_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_vehiculo_user_operador_id",
                        column: x => x.operador_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_vehiculo_operador_id",
                table: "vehiculo",
                column: "operador_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehiculo_tenant_id",
                table: "vehiculo",
                column: "tenant_id");

            migrationBuilder.InsertData(
                table: "permission",
                columns: new[] { "id", "name", "description" },
                values: new object[,]
                {
                    { 6, "vehiculo:read",   "Can read vehicles" },
                    { 7, "vehiculo:create", "Can create vehicles" },
                    { 8, "vehiculo:update", "Can update vehicles" },
                    { 9, "vehiculo:delete", "Can delete vehicles" }
                });

            // Admin (1) and Coordinador (2) get all vehicle permissions
            migrationBuilder.InsertData(
                table: "permission_role",
                columns: new[] { "permissions_id", "roles_id" },
                values: new object[,]
                {
                    { 6, 1 }, { 6, 2 },
                    { 7, 1 }, { 7, 2 },
                    { 8, 1 }, { 8, 2 },
                    { 9, 1 }, { 9, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("permission_role", new[] { "permissions_id", "roles_id" }, new object[] { 6, 1 });
            migrationBuilder.DeleteData("permission_role", new[] { "permissions_id", "roles_id" }, new object[] { 6, 2 });
            migrationBuilder.DeleteData("permission_role", new[] { "permissions_id", "roles_id" }, new object[] { 7, 1 });
            migrationBuilder.DeleteData("permission_role", new[] { "permissions_id", "roles_id" }, new object[] { 7, 2 });
            migrationBuilder.DeleteData("permission_role", new[] { "permissions_id", "roles_id" }, new object[] { 8, 1 });
            migrationBuilder.DeleteData("permission_role", new[] { "permissions_id", "roles_id" }, new object[] { 8, 2 });
            migrationBuilder.DeleteData("permission_role", new[] { "permissions_id", "roles_id" }, new object[] { 9, 1 });
            migrationBuilder.DeleteData("permission_role", new[] { "permissions_id", "roles_id" }, new object[] { 9, 2 });
            migrationBuilder.DeleteData("permission", "id", 6);
            migrationBuilder.DeleteData("permission", "id", 7);
            migrationBuilder.DeleteData("permission", "id", 8);
            migrationBuilder.DeleteData("permission", "id", 9);

            migrationBuilder.DropTable(
                name: "vehiculo");
        }
    }
}
