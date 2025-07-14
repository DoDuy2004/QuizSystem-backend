using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizSystem_backend.Migrations
{
    /// <inheritdoc />
    public partial class changedb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationForCourseClasses_GiangVien_TeacherId",
                table: "NotificationForCourseClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationForCourseClasses_LopHocPhan_CourseClassId",
                table: "NotificationForCourseClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationForCourseClasses_TaiKhoan_UserId",
                table: "NotificationForCourseClasses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationForCourseClasses",
                table: "NotificationForCourseClasses");

            migrationBuilder.DropIndex(
                name: "IX_NotificationForCourseClasses_TeacherId",
                table: "NotificationForCourseClasses");

            migrationBuilder.DropIndex(
                name: "IX_NotificationForCourseClasses_UserId",
                table: "NotificationForCourseClasses");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "NotificationForCourseClasses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "NotificationForCourseClasses");

            migrationBuilder.RenameTable(
                name: "NotificationForCourseClasses",
                newName: "NotificationForCourseClass");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "NotificationForCourseClass",
                newName: "CreateAt");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationForCourseClasses_CourseClassId",
                table: "NotificationForCourseClass",
                newName: "IX_NotificationForCourseClass_CourseClassId");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                table: "StudentRoomExams",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "NotificationForCourseClass",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationForCourseClass",
                table: "NotificationForCourseClass",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_GiangVien_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "GiangVien",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Notification_TaiKhoan_UserId",
                        column: x => x.UserId,
                        principalTable: "TaiKhoan",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_TeacherId",
                table: "Notification",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserId",
                table: "Notification",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationForCourseClass_LopHocPhan_CourseClassId",
                table: "NotificationForCourseClass",
                column: "CourseClassId",
                principalTable: "LopHocPhan",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationForCourseClass_LopHocPhan_CourseClassId",
                table: "NotificationForCourseClass");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationForCourseClass",
                table: "NotificationForCourseClass");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "StudentRoomExams");

            migrationBuilder.RenameTable(
                name: "NotificationForCourseClass",
                newName: "NotificationForCourseClasses");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "NotificationForCourseClasses",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationForCourseClass_CourseClassId",
                table: "NotificationForCourseClasses",
                newName: "IX_NotificationForCourseClasses_CourseClassId");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "NotificationForCourseClasses",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "NotificationForCourseClasses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "NotificationForCourseClasses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationForCourseClasses",
                table: "NotificationForCourseClasses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationForCourseClasses_TeacherId",
                table: "NotificationForCourseClasses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationForCourseClasses_UserId",
                table: "NotificationForCourseClasses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationForCourseClasses_GiangVien_TeacherId",
                table: "NotificationForCourseClasses",
                column: "TeacherId",
                principalTable: "GiangVien",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationForCourseClasses_LopHocPhan_CourseClassId",
                table: "NotificationForCourseClasses",
                column: "CourseClassId",
                principalTable: "LopHocPhan",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationForCourseClasses_TaiKhoan_UserId",
                table: "NotificationForCourseClasses",
                column: "UserId",
                principalTable: "TaiKhoan",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
