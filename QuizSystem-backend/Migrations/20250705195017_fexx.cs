using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizSystem_backend.Migrations
{
    /// <inheritdoc />
    public partial class fexx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "NganHangCauHoi",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "NganHangCauHoi",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_NganHangCauHoi_SubjectId",
                table: "NganHangCauHoi",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_NganHangCauHoi_TeacherId",
                table: "NganHangCauHoi",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_NganHangCauHoi_GiangVien_TeacherId",
                table: "NganHangCauHoi",
                column: "TeacherId",
                principalTable: "GiangVien",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NganHangCauHoi_MonHoc_SubjectId",
                table: "NganHangCauHoi",
                column: "SubjectId",
                principalTable: "MonHoc",
                principalColumn: "ma_mon_hoc",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NganHangCauHoi_GiangVien_TeacherId",
                table: "NganHangCauHoi");

            migrationBuilder.DropForeignKey(
                name: "FK_NganHangCauHoi_MonHoc_SubjectId",
                table: "NganHangCauHoi");

            migrationBuilder.DropIndex(
                name: "IX_NganHangCauHoi_SubjectId",
                table: "NganHangCauHoi");

            migrationBuilder.DropIndex(
                name: "IX_NganHangCauHoi_TeacherId",
                table: "NganHangCauHoi");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "NganHangCauHoi");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "NganHangCauHoi");
        }
    }
}
