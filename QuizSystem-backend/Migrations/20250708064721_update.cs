using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizSystem_backend.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_KetQuaBaiThi_ma_sinh_vien",
                table: "KetQuaBaiThi",
                column: "ma_sinh_vien");

            migrationBuilder.AddForeignKey(
                name: "FK_KetQuaBaiThi_SinhVien_ma_sinh_vien",
                table: "KetQuaBaiThi",
                column: "ma_sinh_vien",
                principalTable: "SinhVien",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KetQuaBaiThi_SinhVien_ma_sinh_vien",
                table: "KetQuaBaiThi");

            migrationBuilder.DropIndex(
                name: "IX_KetQuaBaiThi_ma_sinh_vien",
                table: "KetQuaBaiThi");
        }
    }
}
