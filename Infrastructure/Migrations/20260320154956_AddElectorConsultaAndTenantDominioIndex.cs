using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddElectorConsultaAndTenantDominioIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "elector_consulta",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tenant_id = table.Column<int>(type: "integer", nullable: false),
                    cedula = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ip_cliente = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    user_agent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    origin = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    host = table.Column<string>(type: "character varying(253)", maxLength: 253, nullable: false),
                    metodo_http = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "GET"),
                    encontrado = table.Column<bool>(type: "boolean", nullable: false),
                    consultado_en = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_elector_consulta", x => x.id);
                    table.ForeignKey(
                        name: "fk_elector_consulta_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_tenant_dominio",
                table: "tenant",
                column: "dominio",
                unique: true,
                filter: "dominio IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_elector_consulta_cedula",
                table: "elector_consulta",
                column: "cedula");

            migrationBuilder.CreateIndex(
                name: "ix_elector_consulta_ip_cliente",
                table: "elector_consulta",
                column: "ip_cliente");

            migrationBuilder.CreateIndex(
                name: "ix_elector_consulta_tenant_id_consultado_en",
                table: "elector_consulta",
                columns: new[] { "tenant_id", "consultado_en" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "elector_consulta");

            migrationBuilder.DropIndex(
                name: "ix_tenant_dominio",
                table: "tenant");
        }
    }
}
