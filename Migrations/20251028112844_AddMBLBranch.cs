using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseBuildingFinanceWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddMBLBranch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MBLBranch",
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
                    table.PrimaryKey("PK_MBLBranch", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MBLBranch_BranchCode",
                table: "MBLBranch",
                column: "BranchCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MBLBranch");
        }
    }
}
