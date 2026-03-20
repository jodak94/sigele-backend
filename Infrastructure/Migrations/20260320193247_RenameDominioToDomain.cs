using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameDominioToDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_tenant_dominio",
                table: "tenant");

            migrationBuilder.RenameColumn(
                name: "dominio",
                table: "tenant",
                newName: "domain");

            migrationBuilder.CreateIndex(
                name: "ix_tenant_domain",
                table: "tenant",
                column: "domain",
                unique: true,
                filter: "domain IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_tenant_domain",
                table: "tenant");

            migrationBuilder.RenameColumn(
                name: "domain",
                table: "tenant",
                newName: "dominio");

            migrationBuilder.CreateIndex(
                name: "ix_tenant_dominio",
                table: "tenant",
                column: "dominio",
                unique: true,
                filter: "dominio IS NOT NULL");
        }
    }
}
