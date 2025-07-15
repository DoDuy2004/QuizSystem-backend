
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace QuizSystem_backend.Models
{
    public class QuizSystemDbContext : DbContext
    {
        public QuizSystemDbContext(DbContextOptions<QuizSystemDbContext> options)
            : base(options)
        {
        }

        // DbSets cho các entity
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<CourseClass> CourseClasses { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        //public DbSet<Facutly> Facutlies { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionBank> QuestionBanks { get; set; }
        public DbSet<RoomExam> RoomExams { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<NotificationForCourseClass> NotificationForCourseClass { get; set; }
        public DbSet<StudentCourseClass> StudentCourseClasses { get; set; }
        public DbSet<StudentExam> StudentExams { get; set; }
        public DbSet<StudentExamDetail> StudentExamDetails { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StudentRoomExam> StudentRoomExams { get; set; }
        public DbSet<NotificationMessage> NotificationMessage { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<NotFiniteNumberException>();
            //NorificationMessage
            modelBuilder.Entity<NotificationMessage>(entity=>
            {
                entity.ToTable("TinNhanThongBao");
                entity.HasKey(e=>e.Id);
                entity.Property(n => n.Content)
                .HasColumnName("noi_dung");

                entity.HasOne(n => n.User)
                        .WithMany(u => u.NotificationMessages)
                        .HasForeignKey(u => u.UserId)
                        .OnDelete(DeleteBehavior.Cascade); ;
                entity.HasOne(n => n.Notification)
                        .WithMany(n => n.Messages)
                        .HasForeignKey(n => n.NotificationId)
                        .OnDelete(DeleteBehavior.Cascade); 
            }
            );
            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("TaiKhoan");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Username)
                    .HasColumnName("ten_dang_nhap")
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FullName)
                    .HasColumnName("ho_ten")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("so_dien_thoai")
                    .HasMaxLength(15);

                entity.Property(e => e.Gender)
                    .HasColumnName("gioi_tinh");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnName("ngay_sinh");

                entity.Property(e => e.AvatarUrl)
                    .HasColumnName("url_anh")
                    .HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasColumnName("trang_thai");

                entity.Property(e => e.PasswordHash)
                    .HasColumnName("mat_khau")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("ngay_tao")
                    .IsRequired();

                entity.Property(e => e.Role)
                    .HasColumnName("vai_tro")
                    .IsRequired();

                entity.Property(e => e.Otp)
                    .HasColumnName("Otp");

                entity.Property(e => e.OtpExpireTime)
                    .HasColumnName("OtpExpireTime");


                entity.HasMany(t => t.Questions)
                    .WithOne(q => q.User)
                    .HasForeignKey(q => q.CreatedBy)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(t => t.CourseClasses)
                    .WithOne(cc => cc.User)
                    .HasForeignKey(cc => cc.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(t => t.QuestionBanks)
                     .WithOne(q => q.User)
                     .HasForeignKey(qb => qb.UserId)
                     .OnDelete(DeleteBehavior.Cascade);
            });

            // Student (TPT inheritance from User)
            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("SinhVien");
                entity.HasBaseType<User>();

                entity.Property(s => s.StudentCode)
                    .HasColumnName("ma_sinh_vien")
                    .HasMaxLength(50);

                entity.Property(s => s.IsFirstTimeLogin)
                    .HasColumnName("dang_nhap_lan_dau");

                entity.Property(s => s.Facutly)
                    .HasColumnName("khoa")
                    .HasMaxLength(50);

                //entity.Property(s => s.FacutlyId)
                //    .HasColumnName("ma_khoa")
                //    .IsRequired();

                //entity.HasOne(s => s.Facutly)
                //    .WithMany(f => f.Students)
                //    .HasForeignKey(s => s.FacutlyId)
                //    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(e => e.Courses)
                    .WithOne(scc => scc.Student)
                    .HasForeignKey(scc => scc.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            // Teacher (TPT inheritance from User)
            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("GiangVien");
                entity.HasBaseType<User>();

                entity.Property(t => t.TeacherCode)
                    .HasColumnName("ma_giao_vien")
                    .HasMaxLength(50);

                entity.Property(t => t.IsFirstTimeLogin)
                    .HasColumnName("dang_nhap_lan_dau");

                entity.Property(s => s.Facutly)
                    .HasColumnName("khoa")
                    .HasMaxLength(50);

                //entity.Property(t => t.FacutlyId)
                //    .HasColumnName("ma_khoa")
                //    .IsRequired();  

                //entity.HasOne(t => t.Department)
                //    .WithMany(f => f.Teachers)
                //    .HasForeignKey(t => t.FacutlyId)
                //    .OnDelete(DeleteBehavior.Restrict);
            });

            // Answer
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("CauTraLoi");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Content)
                    .HasColumnName("noi_dung")
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.IsCorrect)
                    .HasColumnName("dung");

                entity.Property(e => e.AnswerOrder)
                    .HasColumnName("thu_tu");

                entity.Property(e => e.Status)
                    .HasColumnName("trang_thai");

                entity.Property(e => e.QuestionId)
                    .HasColumnName("ma_cau_hoi")
                    .IsRequired();

                entity.HasOne(e => e.Question)
                    .WithMany(q => q.Answers)
                    .HasForeignKey(e => e.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Chapter
            modelBuilder.Entity<Chapter>(entity =>
            {
                entity.ToTable("Chuong");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasColumnName("ten_chuong")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .HasColumnName("mo_ta")
                    .HasMaxLength(500);

                entity.Property(e => e.Status)
                    .HasColumnName("trang_thai");

                entity.Property(e => e.SubjectId)
                    .HasColumnName("ma_mon_hoc")
                    .IsRequired();

                entity.HasOne(e => e.Subject)
                    .WithMany(s => s.Chapters)
                    .HasForeignKey(e => e.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Question)
                    .WithOne(q => q.Chapter)
                    .HasForeignKey(q => q.ChapterId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // CourseClass
            modelBuilder.Entity<CourseClass>(entity =>
            {
                entity.ToTable("LopHocPhan");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ClassCode)
                    .HasColumnName("ma_lop")
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(e => e.ClassCode)
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasColumnName("ten_lop")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                   .HasColumnName("mo_ta")
                   .HasMaxLength(100);

                entity.Property(e => e.Name)
                   .HasColumnName("ten_lop")
                   .IsRequired()
                   .HasMaxLength(100);

                entity.Property(e => e.Credit)
                    .HasColumnName("so_tin_chi");

                entity.Property(e => e.Status)
                    .HasColumnName("trang_thai");

                entity.Property(e => e.UserId)
                    .HasColumnName("ma_giao_vien")
                    .IsRequired();

                entity.Property(e => e.SubjectId)
                    .HasColumnName("ma_mon_hoc")
                    .IsRequired();

                entity.HasOne(e => e.Subject)
                    .WithMany(s => s.Courses)
                    .HasForeignKey(e => e.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Students)
                    .WithOne(scc => scc.Course)
                    .HasForeignKey(scc => scc.CourseClassId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Exam
            modelBuilder.Entity<Exam>(entity =>
            {
                entity.ToTable("DeThi");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ExamCode)
                    .HasColumnName("ma_de")
                    .IsRequired()
                    .HasMaxLength(20);

                //entity.HasIndex(e => e.ExamCode)
                //    .IsUnique();

                entity.Property(e => e.Name)
                    .HasColumnName("ten_bai_thi")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DurationMinutes)
                    .HasColumnName("thoi_gian_lam_bai");

                entity.Property(e => e.NoOfQuestions)
                    .HasColumnName("so_cau_hoi");

                entity.Property(e => e.SubjectId)
                    .HasColumnName("ma_mon_hoc");

                entity.Property(e => e.UserId)
                   .HasColumnName("ma_giang_vien");

                //entity.Property(e => e.TotalScore)
                //    .HasColumnName("tong_diem");

                //entity.Property(e => e.RoomExamId)
                //    .HasColumnName("ma_phong_thi");


                entity.Property(e => e.Status)
                    .HasColumnName("trang_thai");

                //entity.HasOne(e => e.RoomExam)
                //    .WithMany(re => re.Exams)
                //    .HasForeignKey(e => e.RoomExamId)
                //    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ExamQuestions)
                    .WithOne(eq => eq.Exam)
                    .HasForeignKey(eq => eq.ExamId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Subject)
                    .WithMany(s => s.Exams)
                    .HasForeignKey(e => e.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.User)
                    .WithMany(t => t.Exams)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ExamQuestion
            modelBuilder.Entity<ExamQuestion>(entity =>
            {
                entity.ToTable("ChiTietDeThi");
                entity.HasKey(eq => new { eq.ExamId, eq.QuestionId });

                entity.Property(eq => eq.Order)
                    .HasColumnName("thu_tu");

                entity.Property(eq => eq.Score)
                    .HasColumnName("diem_so");

                entity.Property(eq => eq.ExamId)
                    .HasColumnName("ma_bai_thi")
                    .IsRequired();

                entity.Property(eq => eq.QuestionId)
                    .HasColumnName("ma_cau_hoi")
                    .IsRequired();

                entity.HasOne(eq => eq.Exam)
                    .WithMany(e => e.ExamQuestions)
                    .HasForeignKey(eq => eq.ExamId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(eq => eq.Question)
                    .WithMany(q => q.ExamQuestions)
                    .HasForeignKey(eq => eq.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("CauHoi");
                entity.HasKey(q => q.Id);

                entity.Property(q => q.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(q => q.Content)
                    .HasColumnName("noi_dung")
                    .HasMaxLength(1000);

                entity.Property(q => q.Image)
                    .HasColumnName("hinh_anh")
                    .HasMaxLength(255);

                entity.Property(q => q.CreatedBy)
                    .HasColumnName("ma_nguoi_tao");

                entity.Property(q => q.Type)
                    .HasColumnName("loai_cau_hoi")
                    .HasMaxLength(50);

                entity.Property(q => q.Difficulty)
                    .HasColumnName("do_kho")
                    .HasMaxLength(50);

                entity.Property(q => q.Status)
                    .HasColumnName("trang_thai");

                entity.Property(q => q.Topic)
                    .HasColumnName("chu_de")
                    .HasMaxLength(255);

                entity.Property(q => q.QuestionBankId)
                    .HasColumnName("ma_ngan_hang_cau_hoi");

                entity.Property(q => q.ChapterId)
                    .HasColumnName("ma_chuong");

                entity.HasOne(q => q.User)
                    .WithMany(t => t.Questions)
                    .HasForeignKey(q => q.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(q => q.ExamQuestions)
                    .WithOne(eq => eq.Question)
                    .HasForeignKey(eq => eq.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(q => q.StudentExamDetails)
                    .WithOne(sed => sed.Question)
                    .HasForeignKey(sed => sed.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(q => q.QuestionBank)
                    .WithMany(qb => qb.Questions)
                    .HasForeignKey(q => q.QuestionBankId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(q => q.Chapter)
                    .WithMany(c => c.Question)
                    .HasForeignKey(q => q.ChapterId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(q => q.Answers)
                    .WithOne(a => a.Question)
                    .HasForeignKey(a => a.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // QuestionBank
            modelBuilder.Entity<QuestionBank>(entity =>
            {
                entity.ToTable("NganHangCauHoi");
                entity.HasKey(qb => qb.Id);

                entity.Property(qb => qb.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(qb => qb.Name)
                    .HasColumnName("ten_ngan_hang")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(qb => qb.Description)
                    .HasColumnName("mo_ta")
                    .HasMaxLength(500);

                entity.Property(qb => qb.Status)
                    .HasColumnName("trang_thai");

                //entity.Property(qb => qb.CourseClassId)
                //    .HasColumnName("mon_hoc")
                //    .IsRequired();

                //entity.Property(qb => qb.TeacherId)
                //    .HasColumnName("ma_giang_vien")
                //    .IsRequired();

                entity.HasMany(qb => qb.Questions)
                    .WithOne(q => q.QuestionBank)
                    .HasForeignKey(q => q.QuestionBankId)
                    .OnDelete(DeleteBehavior.Cascade);

                //entity.HasOne(qb => qb.Teacher)
                //    .WithMany(s => s.QuestionBanks)
                //    .HasForeignKey(qb => qb.TeacherId)
                //    .OnDelete(DeleteBehavior.Restrict);

                //entity.HasOne(qb => qb.Course)
                //    .WithMany(s => s.QuestionBanks)
                //    .HasForeignKey(qb => qb.CourseClassId)
                //    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(qb => qb.Subject)
                    .WithMany(s => s.QuestionBanks)
                    .HasForeignKey(qb => qb.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // RoomExam
            modelBuilder.Entity<RoomExam>(entity =>
            {
                entity.ToTable("PhongThi");
                entity.HasKey(re => re.Id);

                entity.Property(re => re.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(re => re.Name)
                    .HasColumnName("ten_phong")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(re => re.StartDate)
                    .HasColumnName("ngay_bat_dau")
                    .IsRequired();

                entity.Property(re => re.EndDate)
                    .HasColumnName("ngay_ket_thuc")
                    .IsRequired();

                entity.Property(re => re.Status)
                    .HasColumnName("trang_thai");

                entity.Property(re => re.ExamId)
                .HasColumnName("ma_de_thi");

                entity.HasOne(re => re.Exam)
                    .WithMany(e => e.RoomExams)
                    .HasForeignKey(r => r.ExamId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(re => re.Course)
                    .WithMany(e => e.RoomExams)
                    .HasForeignKey(e => e.CourseClassId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(re => re.StudentExams)
                    .WithOne(e => e.Room)
                    .HasForeignKey(e => e.RoomId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(re => re.Subject)
                   .WithMany(e => e.RoomExams)
                   .HasForeignKey(e => e.SubjectId)
                   .OnDelete(DeleteBehavior.Restrict);
            });



            // StudentCourseClass
            modelBuilder.Entity<StudentCourseClass>(entity =>
            {
                entity.ToTable("SinhVienLopHocPhan");
                entity.HasKey(scc => new { scc.StudentId, scc.CourseClassId });

                entity.Property(scc => scc.StudentId)
                    .HasColumnName("ma_sinh_vien")
                    .IsRequired();

                entity.Property(scc => scc.CourseClassId)
                    .HasColumnName("ma_lop_hoc_phan")
                    .IsRequired();

                entity.Property(scc => scc.Grade)
                    .HasColumnName("diem_so");

                entity.Property(scc => scc.Note)
                    .HasColumnName("ghi_chu")
                    .HasMaxLength(255);

                entity.Property(scc => scc.Status)
                    .HasColumnName("trang_thai");
            });

            // StudentExam
            modelBuilder.Entity<StudentExam>(entity =>
            {
                entity.ToTable("KetQuaBaiThi");
                entity.HasKey(se => se.Id);

                entity.Property(se => se.Id)
                    .HasColumnName("ma_ket_qua_bai_thi")
                    .ValueGeneratedOnAdd();

                entity.Property(se => se.StudentId)
                    .HasColumnName("ma_sinh_vien")
                    .IsRequired();

                entity.Property(se => se.RoomId)
                    .HasColumnName("ma_phong_thi")
                    .IsRequired();

                entity.Property(se => se.ExamId)
                    .HasColumnName("ma_bai_thi")
                    .IsRequired();

                entity.Property(scc => scc.Grade)
                    .HasColumnName("diem_so");

                entity.Property(scc => scc.Note)
                    .HasColumnName("ghi_chu")
                    .HasMaxLength(255);

                entity.Property(se => se.Status)
                    .HasColumnName("trang_thai");

                entity.HasOne(se => se.Exam)
                    .WithMany(e => e.StudentExams)
                    .HasForeignKey(se => se.ExamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(se => se.StudentExamDetails)
                    .WithOne(sed => sed.StudentExam)
                    .HasForeignKey(sed => sed.StudentExamId)
                    .OnDelete(DeleteBehavior.Cascade);


            });

            // StudentExamDetail
            modelBuilder.Entity<StudentExamDetail>(entity =>
            {
                entity.ToTable("ChiTietKetQuaBaiThi");
                entity.HasKey(sed => new { sed.AnswerId, sed.QuestionId, sed.StudentExamId });

                entity.Property(sed => sed.AnswerId)
                    .HasColumnName("ma_cau_tra_loi")
                    .IsRequired();

                entity.Property(sed => sed.QuestionId)
                    .HasColumnName("ma_cau_hoi")
                    .IsRequired();

                entity.Property(sed => sed.StudentExamId)
                    .HasColumnName("ma_ket_qua_bai_thi")
                    .IsRequired();

                entity.HasOne(sed => sed.Answer)
                    .WithMany()
                    .HasForeignKey(sed => sed.AnswerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sed => sed.Question)
                    .WithMany(q => q.StudentExamDetails)
                    .HasForeignKey(sed => sed.QuestionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sed => sed.StudentExam)
                    .WithMany(se => se.StudentExamDetails)
                    .HasForeignKey(sed => sed.StudentExamId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Subject
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("MonHoc");
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Id)
                    .HasColumnName("ma_mon_hoc")
                    .ValueGeneratedOnAdd();

                entity.Property(s => s.SubjectCode)
                    .HasColumnName("ma_mon_hoc_code")
                    .HasMaxLength(20);

                entity.Property(s => s.Name)
                    .HasColumnName("ten_mon_hoc")
                    .HasMaxLength(100);

                //entity.Property(s => s.FacutlyId)
                //    .HasColumnName("ma_khoa")
                //    .IsRequired();

                entity.Property(s => s.Status)
                    .HasColumnName("trang_thai");

                //entity.HasOne(s => s.Facutly)
                //    .WithMany(f => f.Subjects)
                //    .HasForeignKey(s => s.FacutlyId)
                //    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(s => s.Courses)
                    .WithOne(cc => cc.Subject)
                    .HasForeignKey(cc => cc.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);

                //entity.HasMany(s => s.QuestionBanks)
                //    .WithOne(qb => qb.Subject)
                //    .HasForeignKey(qb => qb.SubjectId)
                //    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(s => s.Chapters)
                    .WithOne(c => c.Subject)
                    .HasForeignKey(c => c.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("ThongBao");
                entity.HasKey(n => n.Id);


                entity.Property(n => n.Title)
                      .HasColumnName("tieu_de")
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(n => n.Message)
                      .HasColumnName("tin_nhan")
                      .HasMaxLength(500);

                entity.Property(n => n.CreatedAt)
                      .HasColumnName("ngay_tao");

                entity.Property(n => n.IsRead)
                      .HasColumnName("da_doc");

                entity.HasOne(n => n.User)
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Restrict); // tuỳ bạn

            });

            modelBuilder.Entity<NotificationForCourseClass>(entity =>
            {
                entity.HasKey(n => n.Id);

                entity.ToTable("ThongBaoLopHocPhan");

                entity.Property(n => n.Content)
                      .HasColumnName("noi_dung")
                      .IsRequired()
                      .HasMaxLength(100);


                entity.Property(n => n.CreateAt)
                      .HasColumnName("ngay_tao");

                entity.HasOne(n => n.CourseClass)
                      .WithMany(c => c.NotificationForCourseClasses)
                      .HasForeignKey(n => n.CourseClassId)
                      .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<StudentRoomExam>(entity =>
            {
                entity.ToTable("Sinhvien -Kithi");

                entity.HasKey(sr => new { sr.StudentId, sr.RoomExamId });

                entity.Property(sr => sr.RoomExamId)
                      .HasColumnName("ma_ki_thi");

                entity.Property(sr => sr.StudentId)
                      .HasColumnName("ma_sinh_vien");

                entity.Property(sr => sr.SubmitStatus)
                      .HasColumnName("trang_thai_bai_lam");

                entity.Property(sr => sr.SubmittedAt)
                      .HasColumnName("thoi_gian_nop");

                entity.HasOne(sr => sr.Student)
                        .WithMany()
                        .HasForeignKey(sr => sr.StudentId);

                entity.HasOne(sr => sr.RoomExam)
                        .WithMany(r => r.StudentRoomExams)
                        .HasForeignKey(sr => sr.RoomExamId);
            }
            );


        }
    }
}