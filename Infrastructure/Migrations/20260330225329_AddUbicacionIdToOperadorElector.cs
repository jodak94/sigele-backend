using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUbicacionIdToOperadorElector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ubicacion_id",
                table: "operador_elector",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_operador_elector_ubicacion_id",
                table: "operador_elector",
                column: "ubicacion_id");

            migrationBuilder.AddForeignKey(
                name: "fk_operador_elector_ubicacion_ubicacion_id",
                table: "operador_elector",
                column: "ubicacion_id",
                principalTable: "ubicacion",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_operador_elector_ubicacion_ubicacion_id",
                table: "operador_elector");

            migrationBuilder.DropIndex(
                name: "ix_operador_elector_ubicacion_id",
                table: "operador_elector");

            migrationBuilder.DropColumn(
                name: "ubicacion_id",
                table: "operador_elector");
        }
    }
}
