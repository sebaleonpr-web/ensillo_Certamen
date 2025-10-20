using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace enso_Certamen.Migrations
{
    /// <inheritdoc />
    public partial class MakeGuidRolNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rolGeneral",
                columns: table => new
                {
                    GuidRol = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    nombreRol = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    descripRol = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rolGeneral", x => x.GuidRol);
                });

            migrationBuilder.CreateTable(
                name: "usuariosGeneral",
                columns: table => new
                {
                    GuidUsuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    contraUser = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    nombreUser = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    apellidoUser = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    emailUser = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    GuidRol = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuariosGeneral", x => x.GuidUsuario);
                    table.ForeignKey(
                        name: "FK_usuariosGeneral_Rol",
                        column: x => x.GuidRol,
                        principalTable: "rolGeneral",
                        principalColumn: "GuidRol");
                });

            migrationBuilder.CreateTable(
                name: "noticiaGeneral",
                columns: table => new
                {
                    GuidNoticia = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    tituloNoticia = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    resumenNoticia = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    contenidoNoticia = table.Column<string>(type: "text", nullable: true),
                    fechaNoticia = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    GuidUsuario = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_noticiaGeneral", x => x.GuidNoticia);
                    table.ForeignKey(
                        name: "FK_noticiaGeneral_Usuario",
                        column: x => x.GuidUsuario,
                        principalTable: "usuariosGeneral",
                        principalColumn: "GuidUsuario");
                });

            migrationBuilder.CreateTable(
                name: "boletinGeneral",
                columns: table => new
                {
                    GuidBoletin = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    TituloBoletin = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    DescripcionBoletin = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    FechaBoletin = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(CONVERT([date],getdate()))"),
                    GuidNoticia = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_boletinGeneral", x => x.GuidBoletin);
                    table.ForeignKey(
                        name: "FK_boletinGeneral_Noticia",
                        column: x => x.GuidNoticia,
                        principalTable: "noticiaGeneral",
                        principalColumn: "GuidNoticia");
                });

            migrationBuilder.CreateTable(
                name: "comentarioGeneral",
                columns: table => new
                {
                    GuidComentario = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    nombrelectorComentario = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    emailLectorComentario = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    contenidoComentario = table.Column<string>(type: "text", nullable: false),
                    fechaComentario = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    GuidNoticia = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comentarioGeneral", x => x.GuidComentario);
                    table.ForeignKey(
                        name: "FK_comentarioGeneral_Noticia",
                        column: x => x.GuidNoticia,
                        principalTable: "noticiaGeneral",
                        principalColumn: "GuidNoticia");
                });

            migrationBuilder.CreateTable(
                name: "suscripcionGeneral",
                columns: table => new
                {
                    GuidSuscripcion = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    nombreSuscripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    emailSuscripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fechaSuscripcion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    GuidBoletin = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_suscripcionGeneral", x => x.GuidSuscripcion);
                    table.ForeignKey(
                        name: "FK_suscripcionGeneral_Boletin",
                        column: x => x.GuidBoletin,
                        principalTable: "boletinGeneral",
                        principalColumn: "GuidBoletin");
                });

            migrationBuilder.CreateIndex(
                name: "IX_boletinGeneral_GuidNoticia",
                table: "boletinGeneral",
                column: "GuidNoticia");

            migrationBuilder.CreateIndex(
                name: "IX_comentarioGeneral_GuidNoticia",
                table: "comentarioGeneral",
                column: "GuidNoticia");

            migrationBuilder.CreateIndex(
                name: "IX_noticiaGeneral_GuidUsuario",
                table: "noticiaGeneral",
                column: "GuidUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_suscripcionGeneral_GuidBoletin",
                table: "suscripcionGeneral",
                column: "GuidBoletin");

            migrationBuilder.CreateIndex(
                name: "IX_usuariosGeneral_GuidRol",
                table: "usuariosGeneral",
                column: "GuidRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comentarioGeneral");

            migrationBuilder.DropTable(
                name: "suscripcionGeneral");

            migrationBuilder.DropTable(
                name: "boletinGeneral");

            migrationBuilder.DropTable(
                name: "noticiaGeneral");

            migrationBuilder.DropTable(
                name: "usuariosGeneral");

            migrationBuilder.DropTable(
                name: "rolGeneral");
        }
    }
}
