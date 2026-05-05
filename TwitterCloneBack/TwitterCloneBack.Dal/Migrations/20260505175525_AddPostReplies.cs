using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterCloneBack.Dal.Migrations
{
    /// <inheritdoc />
    public partial class AddPostReplies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReplyToPostId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ReplyToPostId",
                table: "Posts",
                column: "ReplyToPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Posts_ReplyToPostId",
                table: "Posts",
                column: "ReplyToPostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Posts_ReplyToPostId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_ReplyToPostId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ReplyToPostId",
                table: "Posts");
        }
    }
}
