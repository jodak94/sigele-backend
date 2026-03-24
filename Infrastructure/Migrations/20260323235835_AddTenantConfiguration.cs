using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_operador_elector_elector_id",
                table: "operador_elector");

            migrationBuilder.DeleteData(
                table: "permission_role",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "permission_role",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "permission_role",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "permission",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.RenameIndex(
                name: "ix_operador_elector_elector_tenant_active",
                table: "operador_elector",
                newName: "ix_operador_elector_elector_id_tenant_id");

            migrationBuilder.CreateTable(
                name: "tenant_branding",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tenant_id = table.Column<int>(type: "integer", nullable: false),
                    app_title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    primary_color = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    secondary_color = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    favicon_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    candidate_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    candidate_title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tenant_branding", x => x.id);
                    table.ForeignKey(
                        name: "fk_tenant_branding_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_operador_elector_tenant_id",
                table: "operador_elector",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "ix_tenant_branding_tenant_id",
                table: "tenant_branding",
                column: "tenant_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tenant_branding");

            migrationBuilder.DropIndex(
                name: "ix_operador_elector_tenant_id",
                table: "operador_elector");

            migrationBuilder.RenameIndex(
                name: "ix_operador_elector_elector_id_tenant_id",
                table: "operador_elector",
                newName: "ix_operador_elector_elector_tenant_active");

            migrationBuilder.InsertData(
                table: "permission",
                columns: new[] { "id", "description", "name" },
                values: new object[] { 4, "Can read consultas statistics", "consultas:read" });

            migrationBuilder.InsertData(
                table: "permission_role",
                columns: new[] { "permissions_id", "roles_id" },
                values: new object[,]
                {
                    { 4, 1 },
                    { 4, 2 },
                    { 4, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "ix_operador_elector_elector_id",
                table: "operador_elector",
                column: "elector_id");
        }
    }
}
