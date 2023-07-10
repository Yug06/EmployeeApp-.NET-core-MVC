using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lastt.Migrations
{
    /// <inheritdoc />
    public partial class CreateEmployeeRelationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_rel",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpId1 = table.Column<int>(type: "int", nullable: false),
                    EmpId2 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_rel", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tbl_rel_tbl_emp_EmpId1",
                        column: x => x.EmpId1,
                        principalTable: "tbl_emp",
                        principalColumn: "EmpId");
                    table.ForeignKey(
                        name: "FK_tbl_rel_tbl_emp_EmpId2",
                        column: x => x.EmpId2,
                        principalTable: "tbl_emp",
                        principalColumn: "EmpId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_rel_EmpId1",
                table: "tbl_rel",
                column: "EmpId1");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_rel_EmpId2",
                table: "tbl_rel",
                column: "EmpId2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_rel");
        }
    }
}
