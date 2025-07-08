using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizSystem_backend.Migrations
{
    /// <inheritdoc />
    public partial class newInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonHoc",
                columns: table => new
                {
                    ma_mon_hoc = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_mon_hoc_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ten_mon_hoc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Major = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    trang_thai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonHoc", x => x.ma_mon_hoc);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoan",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ten_dang_nhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ho_ten = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    so_dien_thoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    gioi_tinh = table.Column<bool>(type: "bit", nullable: false),
                    ngay_sinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    url_anh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    trang_thai = table.Column<int>(type: "int", nullable: false),
                    mat_khau = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    vai_tro = table.Column<int>(type: "int", nullable: false),
                    Otp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtpExpireTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoan", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GiangVien",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_giao_vien = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    dang_nhap_lan_dau = table.Column<bool>(type: "bit", nullable: false),
                    khoa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiangVien", x => x.id);
                    table.ForeignKey(
                        name: "FK_GiangVien_TaiKhoan_id",
                        column: x => x.id,
                        principalTable: "TaiKhoan",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LopHocPhan",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_lop = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ten_lop = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    so_tin_chi = table.Column<int>(type: "int", nullable: false),
                    trang_thai = table.Column<int>(type: "int", nullable: false),
                    mo_ta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ma_giao_vien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_mon_hoc = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LopHocPhan", x => x.id);
                    table.ForeignKey(
                        name: "FK_LopHocPhan_MonHoc_ma_mon_hoc",
                        column: x => x.ma_mon_hoc,
                        principalTable: "MonHoc",
                        principalColumn: "ma_mon_hoc",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LopHocPhan_TaiKhoan_ma_giao_vien",
                        column: x => x.ma_giao_vien,
                        principalTable: "TaiKhoan",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NganHangCauHoi",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ten_ngan_hang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    mo_ta = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    trang_thai = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NganHangCauHoi", x => x.id);
                    table.ForeignKey(
                        name: "FK_NganHangCauHoi_MonHoc_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "MonHoc",
                        principalColumn: "ma_mon_hoc",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NganHangCauHoi_TaiKhoan_UserId",
                        column: x => x.UserId,
                        principalTable: "TaiKhoan",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SinhVien",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_sinh_vien = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    dang_nhap_lan_dau = table.Column<bool>(type: "bit", nullable: false),
                    khoa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinhVien", x => x.id);
                    table.ForeignKey(
                        name: "FK_SinhVien_TaiKhoan_id",
                        column: x => x.id,
                        principalTable: "TaiKhoan",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chuong",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ten_chuong = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    mo_ta = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    trang_thai = table.Column<int>(type: "int", nullable: false),
                    ma_mon_hoc = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chuong", x => x.id);
                    table.ForeignKey(
                        name: "FK_Chuong_LopHocPhan_CourseClassId",
                        column: x => x.CourseClassId,
                        principalTable: "LopHocPhan",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Chuong_MonHoc_ma_mon_hoc",
                        column: x => x.ma_mon_hoc,
                        principalTable: "MonHoc",
                        principalColumn: "ma_mon_hoc",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationForCourseClasses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CourseClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationForCourseClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationForCourseClasses_GiangVien_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "GiangVien",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_NotificationForCourseClasses_LopHocPhan_CourseClassId",
                        column: x => x.CourseClassId,
                        principalTable: "LopHocPhan",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationForCourseClasses_TaiKhoan_UserId",
                        column: x => x.UserId,
                        principalTable: "TaiKhoan",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhongThi",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ten_phong = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ngay_bat_dau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_ket_thuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    trang_thai = table.Column<int>(type: "int", nullable: false),
                    CourseClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhongThi", x => x.id);
                    table.ForeignKey(
                        name: "FK_PhongThi_LopHocPhan_CourseClassId",
                        column: x => x.CourseClassId,
                        principalTable: "LopHocPhan",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhongThi_MonHoc_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "MonHoc",
                        principalColumn: "ma_mon_hoc",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SinhVienLopHocPhan",
                columns: table => new
                {
                    ma_sinh_vien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_lop_hoc_phan = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    diem_so = table.Column<float>(type: "real", nullable: true),
                    ghi_chu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    trang_thai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinhVienLopHocPhan", x => new { x.ma_sinh_vien, x.ma_lop_hoc_phan });
                    table.ForeignKey(
                        name: "FK_SinhVienLopHocPhan_LopHocPhan_ma_lop_hoc_phan",
                        column: x => x.ma_lop_hoc_phan,
                        principalTable: "LopHocPhan",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SinhVienLopHocPhan_SinhVien_ma_sinh_vien",
                        column: x => x.ma_sinh_vien,
                        principalTable: "SinhVien",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CauHoi",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    noi_dung = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    hinh_anh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ma_nguoi_tao = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    loai_cau_hoi = table.Column<int>(type: "int", maxLength: 50, nullable: true),
                    do_kho = table.Column<int>(type: "int", maxLength: 50, nullable: true),
                    trang_thai = table.Column<int>(type: "int", nullable: false),
                    chu_de = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ma_ngan_hang_cau_hoi = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ma_chuong = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauHoi", x => x.id);
                    table.ForeignKey(
                        name: "FK_CauHoi_Chuong_ma_chuong",
                        column: x => x.ma_chuong,
                        principalTable: "Chuong",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CauHoi_NganHangCauHoi_ma_ngan_hang_cau_hoi",
                        column: x => x.ma_ngan_hang_cau_hoi,
                        principalTable: "NganHangCauHoi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CauHoi_TaiKhoan_ma_nguoi_tao",
                        column: x => x.ma_nguoi_tao,
                        principalTable: "TaiKhoan",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeThi",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_de = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ten_bai_thi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    thoi_gian_lam_bai = table.Column<int>(type: "int", nullable: false),
                    so_cau_hoi = table.Column<int>(type: "int", nullable: false),
                    ma_phong_thi = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    trang_thai = table.Column<int>(type: "int", nullable: false),
                    ma_mon_hoc = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_giang_vien = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeThi", x => x.id);
                    table.ForeignKey(
                        name: "FK_DeThi_MonHoc_ma_mon_hoc",
                        column: x => x.ma_mon_hoc,
                        principalTable: "MonHoc",
                        principalColumn: "ma_mon_hoc",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeThi_PhongThi_ma_phong_thi",
                        column: x => x.ma_phong_thi,
                        principalTable: "PhongThi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeThi_TaiKhoan_ma_giang_vien",
                        column: x => x.ma_giang_vien,
                        principalTable: "TaiKhoan",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentRoomExams",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomExamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmitStatus = table.Column<int>(type: "int", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentRoomExams", x => new { x.StudentId, x.RoomExamId });
                    table.ForeignKey(
                        name: "FK_StudentRoomExams_PhongThi_RoomExamId",
                        column: x => x.RoomExamId,
                        principalTable: "PhongThi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentRoomExams_SinhVien_StudentId",
                        column: x => x.StudentId,
                        principalTable: "SinhVien",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CauTraLoi",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    noi_dung = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    dung = table.Column<bool>(type: "bit", nullable: false),
                    thu_tu = table.Column<int>(type: "int", nullable: false),
                    trang_thai = table.Column<int>(type: "int", nullable: false),
                    ma_cau_hoi = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauTraLoi", x => x.id);
                    table.ForeignKey(
                        name: "FK_CauTraLoi_CauHoi_ma_cau_hoi",
                        column: x => x.ma_cau_hoi,
                        principalTable: "CauHoi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDeThi",
                columns: table => new
                {
                    ma_bai_thi = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_cau_hoi = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    thu_tu = table.Column<int>(type: "int", nullable: false),
                    diem_so = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDeThi", x => new { x.ma_bai_thi, x.ma_cau_hoi });
                    table.ForeignKey(
                        name: "FK_ChiTietDeThi_CauHoi_ma_cau_hoi",
                        column: x => x.ma_cau_hoi,
                        principalTable: "CauHoi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDeThi_DeThi_ma_bai_thi",
                        column: x => x.ma_bai_thi,
                        principalTable: "DeThi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KetQuaBaiThi",
                columns: table => new
                {
                    ma_ket_qua_bai_thi = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_sinh_vien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_phong_thi = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_bai_thi = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    trang_thai = table.Column<int>(type: "int", nullable: false),
                    diem_so = table.Column<float>(type: "real", nullable: false),
                    ghi_chu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SubmitStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KetQuaBaiThi", x => x.ma_ket_qua_bai_thi);
                    table.ForeignKey(
                        name: "FK_KetQuaBaiThi_DeThi_ma_bai_thi",
                        column: x => x.ma_bai_thi,
                        principalTable: "DeThi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KetQuaBaiThi_PhongThi_ma_phong_thi",
                        column: x => x.ma_phong_thi,
                        principalTable: "PhongThi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KetQuaBaiThi_SinhVien_ma_sinh_vien",
                        column: x => x.ma_sinh_vien,
                        principalTable: "SinhVien",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietKetQuaBaiThi",
                columns: table => new
                {
                    ma_cau_tra_loi = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_cau_hoi = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ma_ket_qua_bai_thi = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietKetQuaBaiThi", x => new { x.ma_cau_tra_loi, x.ma_cau_hoi, x.ma_ket_qua_bai_thi });
                    table.ForeignKey(
                        name: "FK_ChiTietKetQuaBaiThi_CauHoi_ma_cau_hoi",
                        column: x => x.ma_cau_hoi,
                        principalTable: "CauHoi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietKetQuaBaiThi_CauTraLoi_ma_cau_tra_loi",
                        column: x => x.ma_cau_tra_loi,
                        principalTable: "CauTraLoi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietKetQuaBaiThi_KetQuaBaiThi_ma_ket_qua_bai_thi",
                        column: x => x.ma_ket_qua_bai_thi,
                        principalTable: "KetQuaBaiThi",
                        principalColumn: "ma_ket_qua_bai_thi",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CauHoi_ma_chuong",
                table: "CauHoi",
                column: "ma_chuong");

            migrationBuilder.CreateIndex(
                name: "IX_CauHoi_ma_ngan_hang_cau_hoi",
                table: "CauHoi",
                column: "ma_ngan_hang_cau_hoi");

            migrationBuilder.CreateIndex(
                name: "IX_CauHoi_ma_nguoi_tao",
                table: "CauHoi",
                column: "ma_nguoi_tao");

            migrationBuilder.CreateIndex(
                name: "IX_CauTraLoi_ma_cau_hoi",
                table: "CauTraLoi",
                column: "ma_cau_hoi");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDeThi_ma_cau_hoi",
                table: "ChiTietDeThi",
                column: "ma_cau_hoi");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietKetQuaBaiThi_ma_cau_hoi",
                table: "ChiTietKetQuaBaiThi",
                column: "ma_cau_hoi");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietKetQuaBaiThi_ma_ket_qua_bai_thi",
                table: "ChiTietKetQuaBaiThi",
                column: "ma_ket_qua_bai_thi");

            migrationBuilder.CreateIndex(
                name: "IX_Chuong_CourseClassId",
                table: "Chuong",
                column: "CourseClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Chuong_ma_mon_hoc",
                table: "Chuong",
                column: "ma_mon_hoc");

            migrationBuilder.CreateIndex(
                name: "IX_DeThi_ma_giang_vien",
                table: "DeThi",
                column: "ma_giang_vien");

            migrationBuilder.CreateIndex(
                name: "IX_DeThi_ma_mon_hoc",
                table: "DeThi",
                column: "ma_mon_hoc");

            migrationBuilder.CreateIndex(
                name: "IX_DeThi_ma_phong_thi",
                table: "DeThi",
                column: "ma_phong_thi");

            migrationBuilder.CreateIndex(
                name: "IX_KetQuaBaiThi_ma_bai_thi",
                table: "KetQuaBaiThi",
                column: "ma_bai_thi");

            migrationBuilder.CreateIndex(
                name: "IX_KetQuaBaiThi_ma_phong_thi",
                table: "KetQuaBaiThi",
                column: "ma_phong_thi");

            migrationBuilder.CreateIndex(
                name: "IX_KetQuaBaiThi_ma_sinh_vien",
                table: "KetQuaBaiThi",
                column: "ma_sinh_vien");

            migrationBuilder.CreateIndex(
                name: "IX_LopHocPhan_ma_giao_vien",
                table: "LopHocPhan",
                column: "ma_giao_vien");

            migrationBuilder.CreateIndex(
                name: "IX_LopHocPhan_ma_lop",
                table: "LopHocPhan",
                column: "ma_lop",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LopHocPhan_ma_mon_hoc",
                table: "LopHocPhan",
                column: "ma_mon_hoc");

            migrationBuilder.CreateIndex(
                name: "IX_NganHangCauHoi_SubjectId",
                table: "NganHangCauHoi",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_NganHangCauHoi_UserId",
                table: "NganHangCauHoi",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationForCourseClasses_CourseClassId",
                table: "NotificationForCourseClasses",
                column: "CourseClassId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationForCourseClasses_TeacherId",
                table: "NotificationForCourseClasses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationForCourseClasses_UserId",
                table: "NotificationForCourseClasses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PhongThi_CourseClassId",
                table: "PhongThi",
                column: "CourseClassId");

            migrationBuilder.CreateIndex(
                name: "IX_PhongThi_SubjectId",
                table: "PhongThi",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SinhVienLopHocPhan_ma_lop_hoc_phan",
                table: "SinhVienLopHocPhan",
                column: "ma_lop_hoc_phan");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRoomExams_RoomExamId",
                table: "StudentRoomExams",
                column: "RoomExamId");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_email",
                table: "TaiKhoan",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_ten_dang_nhap",
                table: "TaiKhoan",
                column: "ten_dang_nhap",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDeThi");

            migrationBuilder.DropTable(
                name: "ChiTietKetQuaBaiThi");

            migrationBuilder.DropTable(
                name: "NotificationForCourseClasses");

            migrationBuilder.DropTable(
                name: "SinhVienLopHocPhan");

            migrationBuilder.DropTable(
                name: "StudentRoomExams");

            migrationBuilder.DropTable(
                name: "CauTraLoi");

            migrationBuilder.DropTable(
                name: "KetQuaBaiThi");

            migrationBuilder.DropTable(
                name: "GiangVien");

            migrationBuilder.DropTable(
                name: "CauHoi");

            migrationBuilder.DropTable(
                name: "DeThi");

            migrationBuilder.DropTable(
                name: "SinhVien");

            migrationBuilder.DropTable(
                name: "Chuong");

            migrationBuilder.DropTable(
                name: "NganHangCauHoi");

            migrationBuilder.DropTable(
                name: "PhongThi");

            migrationBuilder.DropTable(
                name: "LopHocPhan");

            migrationBuilder.DropTable(
                name: "MonHoc");

            migrationBuilder.DropTable(
                name: "TaiKhoan");
        }
    }
}
