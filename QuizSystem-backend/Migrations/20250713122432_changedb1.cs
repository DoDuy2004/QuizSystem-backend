using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizSystem_backend.Migrations
{
    /// <inheritdoc />
    public partial class changedb1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_GiangVien_TeacherId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_TaiKhoan_UserId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationForCourseClass_LopHocPhan_CourseClassId",
                table: "NotificationForCourseClass");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentRoomExams_PhongThi_RoomExamId",
                table: "StudentRoomExams");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentRoomExams_SinhVien_StudentId",
                table: "StudentRoomExams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentRoomExams",
                table: "StudentRoomExams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationForCourseClass",
                table: "NotificationForCourseClass");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.RenameTable(
                name: "StudentRoomExams",
                newName: "Sinhvien -Kithi");

            migrationBuilder.RenameTable(
                name: "NotificationForCourseClass",
                newName: "ThongBaoLopHocPhan");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "ThongBao");

            migrationBuilder.RenameColumn(
                name: "SubmittedAt",
                table: "Sinhvien -Kithi",
                newName: "thoi_gian_nop");

            migrationBuilder.RenameColumn(
                name: "SubmitStatus",
                table: "Sinhvien -Kithi",
                newName: "trang_thai_bai_lam");

            migrationBuilder.RenameColumn(
                name: "RoomExamId",
                table: "Sinhvien -Kithi",
                newName: "ma_ki_thi");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Sinhvien -Kithi",
                newName: "ma_sinh_vien");

            migrationBuilder.RenameIndex(
                name: "IX_StudentRoomExams_RoomExamId",
                table: "Sinhvien -Kithi",
                newName: "IX_Sinhvien -Kithi_ma_ki_thi");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "ThongBaoLopHocPhan",
                newName: "ngay_tao");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "ThongBaoLopHocPhan",
                newName: "noi_dung");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationForCourseClass_CourseClassId",
                table: "ThongBaoLopHocPhan",
                newName: "IX_ThongBaoLopHocPhan_CourseClassId");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "ThongBao",
                newName: "tieu_de");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "ThongBao",
                newName: "tin_nhan");

            migrationBuilder.RenameColumn(
                name: "IsRead",
                table: "ThongBao",
                newName: "da_doc");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ThongBao",
                newName: "ngay_tao");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_UserId",
                table: "ThongBao",
                newName: "IX_ThongBao_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_TeacherId",
                table: "ThongBao",
                newName: "IX_ThongBao_TeacherId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sinhvien -Kithi",
                table: "Sinhvien -Kithi",
                columns: new[] { "ma_sinh_vien", "ma_ki_thi" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThongBaoLopHocPhan",
                table: "ThongBaoLopHocPhan",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThongBao",
                table: "ThongBao",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sinhvien -Kithi_PhongThi_ma_ki_thi",
                table: "Sinhvien -Kithi",
                column: "ma_ki_thi",
                principalTable: "PhongThi",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sinhvien -Kithi_SinhVien_ma_sinh_vien",
                table: "Sinhvien -Kithi",
                column: "ma_sinh_vien",
                principalTable: "SinhVien",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ThongBao_GiangVien_TeacherId",
                table: "ThongBao",
                column: "TeacherId",
                principalTable: "GiangVien",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ThongBao_TaiKhoan_UserId",
                table: "ThongBao",
                column: "UserId",
                principalTable: "TaiKhoan",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ThongBaoLopHocPhan_LopHocPhan_CourseClassId",
                table: "ThongBaoLopHocPhan",
                column: "CourseClassId",
                principalTable: "LopHocPhan",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sinhvien -Kithi_PhongThi_ma_ki_thi",
                table: "Sinhvien -Kithi");

            migrationBuilder.DropForeignKey(
                name: "FK_Sinhvien -Kithi_SinhVien_ma_sinh_vien",
                table: "Sinhvien -Kithi");

            migrationBuilder.DropForeignKey(
                name: "FK_ThongBao_GiangVien_TeacherId",
                table: "ThongBao");

            migrationBuilder.DropForeignKey(
                name: "FK_ThongBao_TaiKhoan_UserId",
                table: "ThongBao");

            migrationBuilder.DropForeignKey(
                name: "FK_ThongBaoLopHocPhan_LopHocPhan_CourseClassId",
                table: "ThongBaoLopHocPhan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThongBaoLopHocPhan",
                table: "ThongBaoLopHocPhan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThongBao",
                table: "ThongBao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sinhvien -Kithi",
                table: "Sinhvien -Kithi");

            migrationBuilder.RenameTable(
                name: "ThongBaoLopHocPhan",
                newName: "NotificationForCourseClass");

            migrationBuilder.RenameTable(
                name: "ThongBao",
                newName: "Notification");

            migrationBuilder.RenameTable(
                name: "Sinhvien -Kithi",
                newName: "StudentRoomExams");

            migrationBuilder.RenameColumn(
                name: "noi_dung",
                table: "NotificationForCourseClass",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "ngay_tao",
                table: "NotificationForCourseClass",
                newName: "CreateAt");

            migrationBuilder.RenameIndex(
                name: "IX_ThongBaoLopHocPhan_CourseClassId",
                table: "NotificationForCourseClass",
                newName: "IX_NotificationForCourseClass_CourseClassId");

            migrationBuilder.RenameColumn(
                name: "tin_nhan",
                table: "Notification",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "tieu_de",
                table: "Notification",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "ngay_tao",
                table: "Notification",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "da_doc",
                table: "Notification",
                newName: "IsRead");

            migrationBuilder.RenameIndex(
                name: "IX_ThongBao_UserId",
                table: "Notification",
                newName: "IX_Notification_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ThongBao_TeacherId",
                table: "Notification",
                newName: "IX_Notification_TeacherId");

            migrationBuilder.RenameColumn(
                name: "trang_thai_bai_lam",
                table: "StudentRoomExams",
                newName: "SubmitStatus");

            migrationBuilder.RenameColumn(
                name: "thoi_gian_nop",
                table: "StudentRoomExams",
                newName: "SubmittedAt");

            migrationBuilder.RenameColumn(
                name: "ma_ki_thi",
                table: "StudentRoomExams",
                newName: "RoomExamId");

            migrationBuilder.RenameColumn(
                name: "ma_sinh_vien",
                table: "StudentRoomExams",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Sinhvien -Kithi_ma_ki_thi",
                table: "StudentRoomExams",
                newName: "IX_StudentRoomExams_RoomExamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationForCourseClass",
                table: "NotificationForCourseClass",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentRoomExams",
                table: "StudentRoomExams",
                columns: new[] { "StudentId", "RoomExamId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_GiangVien_TeacherId",
                table: "Notification",
                column: "TeacherId",
                principalTable: "GiangVien",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_TaiKhoan_UserId",
                table: "Notification",
                column: "UserId",
                principalTable: "TaiKhoan",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationForCourseClass_LopHocPhan_CourseClassId",
                table: "NotificationForCourseClass",
                column: "CourseClassId",
                principalTable: "LopHocPhan",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentRoomExams_PhongThi_RoomExamId",
                table: "StudentRoomExams",
                column: "RoomExamId",
                principalTable: "PhongThi",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentRoomExams_SinhVien_StudentId",
                table: "StudentRoomExams",
                column: "StudentId",
                principalTable: "SinhVien",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
