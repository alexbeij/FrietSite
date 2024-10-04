using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrietSite.Migrations
{
    /// <inheritdoc />
    public partial class orderhistoryadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderHistoryId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHistories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "OrderHistories",
                column: "Id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderHistoryId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderHistoryId",
                table: "Orders",
                column: "OrderHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderHistories_OrderHistoryId",
                table: "Orders",
                column: "OrderHistoryId",
                principalTable: "OrderHistories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderHistories_OrderHistoryId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "OrderHistories");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderHistoryId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderHistoryId",
                table: "Orders");
        }
    }
}
