using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookfinder.Migrations
{
    /// <inheritdoc />
    public partial class Favoritefixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteBooks_Books_BookId",
                table: "FavoriteBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteBooks_Users_UserId",
                table: "FavoriteBooks");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteBooks_BookId",
                table: "FavoriteBooks");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteBooks_UserId",
                table: "FavoriteBooks");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "FavoriteBooks");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "FavoriteBooks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cover",
                table: "FavoriteBooks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "FavoriteBooks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "FavoriteBooks");

            migrationBuilder.DropColumn(
                name: "Cover",
                table: "FavoriteBooks");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "FavoriteBooks");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "FavoriteBooks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteBooks_BookId",
                table: "FavoriteBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteBooks_UserId",
                table: "FavoriteBooks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteBooks_Books_BookId",
                table: "FavoriteBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteBooks_Users_UserId",
                table: "FavoriteBooks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
