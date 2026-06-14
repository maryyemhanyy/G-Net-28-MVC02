using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EditMembershipConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberShips",
                table: "MemberShips");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MemberShips",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberShips",
                table: "MemberShips",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MemberShips_MemberId",
                table: "MemberShips",
                column: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberShips",
                table: "MemberShips");

            migrationBuilder.DropIndex(
                name: "IX_MemberShips_MemberId",
                table: "MemberShips");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MemberShips");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberShips",
                table: "MemberShips",
                columns: new[] { "MemberId", "PlanId" });
        }
    }
}
