using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarShop.WepApi.Migrations
{
    /// <inheritdoc />
    public partial class mog6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FeedBacks",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeedBacks",
                table: "Cars");
        }
    }
}
