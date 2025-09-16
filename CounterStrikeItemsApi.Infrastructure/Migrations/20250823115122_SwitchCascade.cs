using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CounterStrikeItemsApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SwitchCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Founds_ItemCommons_ContainerId",
                table: "Founds");

            migrationBuilder.DropForeignKey(
                name: "FK_Founds_ItemCommons_ItemCommonId",
                table: "Founds");

            migrationBuilder.AddForeignKey(
                name: "FK_Founds_ItemCommons_ContainerId",
                table: "Founds",
                column: "ContainerId",
                principalTable: "ItemCommons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Founds_ItemCommons_ItemCommonId",
                table: "Founds",
                column: "ItemCommonId",
                principalTable: "ItemCommons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Founds_ItemCommons_ContainerId",
                table: "Founds");

            migrationBuilder.DropForeignKey(
                name: "FK_Founds_ItemCommons_ItemCommonId",
                table: "Founds");

            migrationBuilder.AddForeignKey(
                name: "FK_Founds_ItemCommons_ContainerId",
                table: "Founds",
                column: "ContainerId",
                principalTable: "ItemCommons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Founds_ItemCommons_ItemCommonId",
                table: "Founds",
                column: "ItemCommonId",
                principalTable: "ItemCommons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
