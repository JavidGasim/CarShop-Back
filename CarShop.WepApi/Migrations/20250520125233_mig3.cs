using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarShop.WepApi.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favourite_AspNetUsers_UserId",
                table: "Favourite");

            migrationBuilder.DropForeignKey(
                name: "FK_Favourite_Cars_CarId",
                table: "Favourite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Favourite",
                table: "Favourite");

            migrationBuilder.RenameTable(
                name: "Favourite",
                newName: "Favourites");

            migrationBuilder.RenameIndex(
                name: "IX_Favourite_UserId",
                table: "Favourites",
                newName: "IX_Favourites_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Favourite_CarId",
                table: "Favourites",
                newName: "IX_Favourites_CarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Favourites",
                table: "Favourites",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Favourites_AspNetUsers_UserId",
                table: "Favourites",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favourites_Cars_CarId",
                table: "Favourites",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favourites_AspNetUsers_UserId",
                table: "Favourites");

            migrationBuilder.DropForeignKey(
                name: "FK_Favourites_Cars_CarId",
                table: "Favourites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Favourites",
                table: "Favourites");

            migrationBuilder.RenameTable(
                name: "Favourites",
                newName: "Favourite");

            migrationBuilder.RenameIndex(
                name: "IX_Favourites_UserId",
                table: "Favourite",
                newName: "IX_Favourite_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Favourites_CarId",
                table: "Favourite",
                newName: "IX_Favourite_CarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Favourite",
                table: "Favourite",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Favourite_AspNetUsers_UserId",
                table: "Favourite",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favourite_Cars_CarId",
                table: "Favourite",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
