using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVehiculoRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "vehiculo_request",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    elector_id = table.Column<int>(type: "integer", nullable: false),
                    operador_id = table.Column<int>(type: "integer", nullable: false),
                    nombre_dueno = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    telefono_dueno = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    capacidad = table.Column<int>(type: "integer", nullable: false),
                    monto_alquiler = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    estado = table.Column<int>(type: "integer", nullable: false),
                    observacion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    aprobado_por_id = table.Column<int>(type: "integer", nullable: true),
                    fecha_resolucion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("pk_vehiculo_request", x => x.id);
                    table.ForeignKey(
                        name: "fk_vehiculo_request_elector_elector_id",
                        column: x => x.elector_id,
                        principalTable: "elector",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_vehiculo_request_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_vehiculo_request_user_aprobado_por_id",
                        column: x => x.aprobado_por_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_vehiculo_request_user_operador_id",
                        column: x => x.operador_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_vehiculo_request_aprobado_por_id",
                table: "vehiculo_request",
                column: "aprobado_por_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehiculo_request_elector_id",
                table: "vehiculo_request",
                column: "elector_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehiculo_request_operador_id",
                table: "vehiculo_request",
                column: "operador_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehiculo_request_tenant_id",
                table: "vehiculo_request",
                column: "tenant_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vehiculo_request");
        }
    }
}
