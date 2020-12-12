using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentManagement.DataAccess.Migrations
{
    public partial class SubjectModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ__Summary___73013082B281F30A",
                table: "Summary_Subject");

            migrationBuilder.DropIndex(
                name: "UQ__Summary___F5B4DD73D003A2C0",
                table: "Summary");

            migrationBuilder.DropIndex(
                name: "UQ__Record_S__CA821CD02A967351",
                table: "Record_Subject");

            migrationBuilder.DropColumn(
                name: "SubjectName",
                table: "Summary_Subject");

            migrationBuilder.DropColumn(
                name: "SubjectName",
                table: "Record_Subject");

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Summary_Subject",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Record_Subject",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Summary_Subject_SubjectId",
                table: "Summary_Subject",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "UQ__Summary___F5B4DD73D003A2C0",
                table: "Summary",
                columns: new[] { "Semeter", "ClassId" },
                unique: true,
                filter: "([Semeter] IS NOT NULL AND [ClassId] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_Record_Subject_SubjectId",
                table: "Record_Subject",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Record_Subject_Subject",
                table: "Record_Subject",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Summary_Subject_Subject",
                table: "Summary_Subject",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Record_Subject_Subject",
                table: "Record_Subject");

            migrationBuilder.DropForeignKey(
                name: "FK_Summary_Subject_Subject",
                table: "Summary_Subject");

            migrationBuilder.DropTable(
                name: "Subject");

            migrationBuilder.DropIndex(
                name: "IX_Summary_Subject_SubjectId",
                table: "Summary_Subject");

            migrationBuilder.DropIndex(
                name: "UQ__Summary___F5B4DD73D003A2C0",
                table: "Summary");

            migrationBuilder.DropIndex(
                name: "IX_Record_Subject_SubjectId",
                table: "Record_Subject");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Summary_Subject");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Record_Subject");

            migrationBuilder.AddColumn<string>(
                name: "SubjectName",
                table: "Summary_Subject",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubjectName",
                table: "Record_Subject",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Summary___73013082B281F30A",
                table: "Summary_Subject",
                columns: new[] { "SubjectName", "Semeter", "ClassId" },
                unique: true,
                filter: "[SubjectName] IS NOT NULL AND [Semeter] IS NOT NULL AND [ClassId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__Summary___F5B4DD73D003A2C0",
                table: "Summary",
                columns: new[] { "Semeter", "ClassId" },
                unique: true,
                filter: "[Semeter] IS NOT NULL AND [ClassId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__Record_S__CA821CD02A967351",
                table: "Record_Subject",
                columns: new[] { "SubjectName", "Semeter", "ClassId", "StudentId" },
                unique: true,
                filter: "[SubjectName] IS NOT NULL AND [Semeter] IS NOT NULL AND [ClassId] IS NOT NULL AND [StudentId] IS NOT NULL");
        }
    }
}
