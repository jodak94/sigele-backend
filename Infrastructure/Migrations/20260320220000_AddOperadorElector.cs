using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOperadorElector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "operador_elector",
                columns: table => new
                {
                    user_id    = table.Column<int>(type: "integer", nullable: false),
                    elector_id = table.Column<int>(type: "integer", nullable: false),
                    tenant_id  = table.Column<int>(type: "integer", nullable: false),
                    disponible_miembro_mesa = table.Column<bool>(type: "boolean", nullable: false),
                    requiere_transporte     = table.Column<bool>(type: "boolean", nullable: false),
                    nro_telefono       = table.Column<string>(type: "character varying(20)",  maxLength: 20,  nullable: false),
                    direccion_recogida = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    is_active  = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_operador_elector", x => new { x.user_id, x.elector_id });
                    table.ForeignKey(
                        name: "fk_operador_elector_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_operador_elector_elector_elector_id",
                        column: x => x.elector_id,
                        principalTable: "elector",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_operador_elector_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_operador_elector_user_id",
                table: "operador_elector",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_operador_elector_elector_id",
                table: "operador_elector",
                column: "elector_id");

            // Garantiza que un elector solo puede estar activo en un operador por tenant
            migrationBuilder.CreateIndex(
                name: "ix_operador_elector_elector_tenant_active",
                table: "operador_elector",
                columns: new[] { "elector_id", "tenant_id" },
                unique: true,
                filter: "is_active = true");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "operador_elector");
        }
    }
}
