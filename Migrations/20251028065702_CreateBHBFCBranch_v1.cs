using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseBuildingFinanceWebApp.Migrations
{
    /// <inheritdoc />
    public partial class CreateBHBFCBranch_v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BHBFC_Branch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BHBFC_Branch", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BHBFC_Branch_BranchCode",
                table: "BHBFC_Branch",
                column: "BranchCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BHBFC_Branch");
        }
    }
}
