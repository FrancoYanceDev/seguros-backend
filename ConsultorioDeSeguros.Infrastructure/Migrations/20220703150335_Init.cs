using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsultorioDeSeguros.Infrastructure.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "t_asegurado",
                schema: "dbo",
                columns: table => new
                {
                    cedula = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    telefono = table.Column<string>(type: "nvarchar(14)", nullable: false),
                    edad = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asegurado", x => x.cedula);
                });

            migrationBuilder.CreateTable(
                name: "t_seguro",
                schema: "dbo",
                columns: table => new
                {
                    codigo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    SumaAsegurada = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    prima = table.Column<decimal>(type: "decimal(8,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seguro", x => x.codigo);
                });

            migrationBuilder.CreateTable(
                name: "t_seguro_asegurado",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo = table.Column<int>(type: "int", nullable: false),
                    cedula = table.Column<string>(type: "nvarchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sseguro_asegurado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_t_seguro_asegurado_t_asegurado_cedula",
                        column: x => x.cedula,
                        principalSchema: "dbo",
                        principalTable: "t_asegurado",
                        principalColumn: "cedula",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_t_seguro_asegurado_t_seguro_codigo",
                        column: x => x.codigo,
                        principalSchema: "dbo",
                        principalTable: "t_seguro",
                        principalColumn: "codigo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "t_asegurado",
                columns: new[] { "cedula", "edad", "nombre", "telefono" },
                values: new object[,]
                {
                    { "0925613556", (byte)25, "Christian Franco", "+593985749632" },
                    { "1756359625", (byte)32, "Ed Sheeran", "0985369514" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "t_seguro",
                columns: new[] { "codigo", "nombre", "prima", "SumaAsegurada" },
                values: new object[,]
                {
                    { 1, "Vida", 20.00m, 200.00m },
                    { 2, "Salud", 20.00m, 200.00m },
                    { 3, "Vehícular", 20.00m, 200.00m }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "t_seguro_asegurado",
                columns: new[] { "Id", "cedula", "codigo" },
                values: new object[,]
                {
                    { 1, "1756359625", 1 },
                    { 2, "1756359625", 2 },
                    { 3, "1756359625", 3 },
                    { 4, "0925613556", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_seguro_asegurado_cedula",
                schema: "dbo",
                table: "t_seguro_asegurado",
                column: "cedula");

            migrationBuilder.CreateIndex(
                name: "IX_t_seguro_asegurado_codigo",
                schema: "dbo",
                table: "t_seguro_asegurado",
                column: "codigo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_seguro_asegurado",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "t_asegurado",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "t_seguro",
                schema: "dbo");
        }
    }
}
