using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PadronGeneral : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_vehiculo_request_elector_elector_id",
                table: "vehiculo_request");

            migrationBuilder.DropIndex(
                name: "ix_vehiculo_request_elector_id",
                table: "vehiculo_request");

            migrationBuilder.CreateTable(
                name: "operador_persona",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    cedula = table.Column<int>(type: "integer", nullable: false),
                    tenant_id = table.Column<int>(type: "integer", nullable: false),
                    disponible_miembro_mesa = table.Column<bool>(type: "boolean", nullable: false),
                    requiere_transporte = table.Column<bool>(type: "boolean", nullable: false),
                    nro_telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    direccion_recogida = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ubicacion_id = table.Column<int>(type: "integer", nullable: true),
                    operador_ubicacion_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_operador_persona", x => new { x.user_id, x.cedula });
                    table.ForeignKey(
                        name: "fk_operador_persona_persona_cedula",
                        column: x => x.cedula,
                        principalTable: "persona",
                        principalColumn: "cedula",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_operador_persona_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_operador_persona_ubicacion_operador_ubicacion_id",
                        column: x => x.operador_ubicacion_id,
                        principalTable: "ubicacion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_operador_persona_ubicacion_ubicacion_id",
                        column: x => x.ubicacion_id,
                        principalTable: "ubicacion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_operador_persona_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_operador_persona_cedula_tenant_id",
                table: "operador_persona",
                columns: new[] { "cedula", "tenant_id" },
                unique: true,
                filter: "is_active = true");

            migrationBuilder.CreateIndex(
                name: "ix_operador_persona_operador_ubicacion_id",
                table: "operador_persona",
                column: "operador_ubicacion_id");

            migrationBuilder.CreateIndex(
                name: "ix_operador_persona_tenant_id",
                table: "operador_persona",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "ix_operador_persona_ubicacion_id",
                table: "operador_persona",
                column: "ubicacion_id");

            migrationBuilder.CreateIndex(
                name: "ix_operador_persona_user_id",
                table: "operador_persona",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "operador_persona");

            migrationBuilder.CreateIndex(
                name: "ix_vehiculo_request_elector_id",
                table: "vehiculo_request",
                column: "elector_id");

            migrationBuilder.AddForeignKey(
                name: "fk_vehiculo_request_elector_elector_id",
                table: "vehiculo_request",
                column: "elector_id",
                principalTable: "elector",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
