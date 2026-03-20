using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "local_votacion",
                columns: table => new
                {
                    secc_loc = table.Column<int>(type: "integer", nullable: false),
                    nombre_loc = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    direccion = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    recibido = table.Column<string>(type: "character(1)", fixedLength: true, maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_local_votacion", x => x.secc_loc);
                });

            migrationBuilder.CreateTable(
                name: "permission",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permission", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "seccional",
                columns: table => new
                {
                    codigo_dep = table.Column<short>(type: "smallint", nullable: false),
                    codigo_dis = table.Column<short>(type: "smallint", nullable: false),
                    codigo_sec = table.Column<short>(type: "smallint", nullable: false),
                    n_depart = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    n_distrito = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    zona = table.Column<short>(type: "smallint", nullable: true),
                    descripcio = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    w_seccio = table.Column<string>(type: "character varying(35)", maxLength: 35, nullable: true),
                    direccion = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seccional", x => new { x.codigo_dep, x.codigo_dis, x.codigo_sec });
                });

            migrationBuilder.CreateTable(
                name: "tenant",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    subdomain = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tenant", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "elector",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numero_ced = table.Column<int>(type: "integer", nullable: false),
                    cod_dpto = table.Column<short>(type: "smallint", nullable: true),
                    cod_dist = table.Column<short>(type: "smallint", nullable: true),
                    sec_ant = table.Column<short>(type: "smallint", nullable: true),
                    codigo_sec = table.Column<short>(type: "smallint", nullable: true),
                    s_local = table.Column<short>(type: "smallint", nullable: true),
                    mesa = table.Column<short>(type: "smallint", nullable: true),
                    orden = table.Column<short>(type: "smallint", nullable: true),
                    apellido = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    nombre = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    direccion = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    fecha_naci = table.Column<DateOnly>(type: "date", nullable: true),
                    fecha_afil = table.Column<DateOnly>(type: "date", nullable: true),
                    anio = table.Column<short>(type: "smallint", nullable: true),
                    codigo_sex = table.Column<short>(type: "smallint", nullable: true),
                    sec_loc = table.Column<int>(type: "integer", nullable: true),
                    key_dd = table.Column<string>(type: "character(4)", fixedLength: true, maxLength: 4, nullable: true),
                    ced_ape_nom = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_elector", x => x.id);
                    table.ForeignKey(
                        name: "fk_elector_local_votacion_sec_loc",
                        column: x => x.sec_loc,
                        principalTable: "local_votacion",
                        principalColumn: "secc_loc",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "permission_role",
                columns: table => new
                {
                    permissions_id = table.Column<int>(type: "integer", nullable: false),
                    roles_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permission_role", x => new { x.permissions_id, x.roles_id });
                    table.ForeignKey(
                        name: "fk_permission_role_permission_permissions_id",
                        column: x => x.permissions_id,
                        principalTable: "permission",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_permission_role_role_roles_id",
                        column: x => x.roles_id,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "secc_local",
                columns: table => new
                {
                    codigo_dep = table.Column<short>(type: "smallint", nullable: false),
                    codigo_dis = table.Column<short>(type: "smallint", nullable: false),
                    codigo_sec = table.Column<short>(type: "smallint", nullable: false),
                    codigo_loc = table.Column<short>(type: "smallint", nullable: false),
                    cod_local = table.Column<short>(type: "smallint", nullable: true),
                    secc_loc = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_secc_local", x => new { x.codigo_dep, x.codigo_dis, x.codigo_sec, x.codigo_loc });
                    table.ForeignKey(
                        name: "fk_secc_local_local_votacion_secc_loc",
                        column: x => x.secc_loc,
                        principalTable: "local_votacion",
                        principalColumn: "secc_loc",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_secc_local_seccional_codigo_dep_codigo_dis_codigo_sec",
                        columns: x => new { x.codigo_dep, x.codigo_dis, x.codigo_sec },
                        principalTable: "seccional",
                        principalColumns: new[] { "codigo_dep", "codigo_dis", "codigo_sec" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    full_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    last_login = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    coordinator_id = table.Column<int>(type: "integer", nullable: true),
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
                    table.PrimaryKey("pk_user", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_user_coordinator_id",
                        column: x => x.coordinator_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "refresh_token",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    revoked_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_token", x => x.id);
                    table.ForeignKey(
                        name: "fk_refresh_token_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "permission",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { 1, "Can read users", "user:read" },
                    { 2, "Can create operator users", "user:create-operator" },
                    { 3, "Can create coordinator users", "user:create-coordinator" }
                });

            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Coordinador" },
                    { 3, "Operador" }
                });

            migrationBuilder.InsertData(
                table: "permission_role",
                columns: new[] { "permissions_id", "roles_id" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 2, 1 },
                    { 2, 2 },
                    { 3, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "idx_electores_apellido",
                table: "elector",
                column: "apellido");

            migrationBuilder.CreateIndex(
                name: "idx_electores_dpto_dist",
                table: "elector",
                columns: new[] { "cod_dpto", "cod_dist" });

            migrationBuilder.CreateIndex(
                name: "idx_electores_nombre",
                table: "elector",
                column: "nombre");

            migrationBuilder.CreateIndex(
                name: "idx_electores_sec_mesa",
                table: "elector",
                columns: new[] { "codigo_sec", "mesa" });

            migrationBuilder.CreateIndex(
                name: "ix_elector_sec_loc",
                table: "elector",
                column: "sec_loc");

            migrationBuilder.CreateIndex(
                name: "ix_permission_name",
                table: "permission",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_permission_role_roles_id",
                table: "permission_role",
                column: "roles_id");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_token_token",
                table: "refresh_token",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_refresh_token_user_id",
                table: "refresh_token",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_name",
                table: "role",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_secc_locales_secc_loc",
                table: "secc_local",
                column: "secc_loc");

            migrationBuilder.CreateIndex(
                name: "ix_tenant_subdomain",
                table: "tenant",
                column: "subdomain",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_coordinator_id",
                table: "user",
                column: "coordinator_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_email_tenant_id",
                table: "user",
                columns: new[] { "email", "tenant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_role_id",
                table: "user",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_tenant_id",
                table: "user",
                column: "tenant_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "elector");

            migrationBuilder.DropTable(
                name: "permission_role");

            migrationBuilder.DropTable(
                name: "refresh_token");

            migrationBuilder.DropTable(
                name: "secc_local");

            migrationBuilder.DropTable(
                name: "permission");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "local_votacion");

            migrationBuilder.DropTable(
                name: "seccional");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "tenant");
        }
    }
}
