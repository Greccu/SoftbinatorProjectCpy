using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftbinatorProject.Api.Migrations
{
    public partial class SectionsAndChaptersFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Sections_SectionId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_SectionId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "Sections");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "Sections",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sections_SectionId",
                table: "Sections",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Sections_SectionId",
                table: "Sections",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
