using Microsoft.EntityFrameworkCore;
using QuizSystem_backend.Models;

namespace QuizSystem_backend.Models
{
    public class QuizSystemDbContext : DbContext
    {
        public QuizSystemDbContext(DbContextOptions<QuizSystemDbContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<ExamQuestion> ExamQuestions { get; set; }
        public virtual DbSet<RoomExamSubject> RoomExamSubjects { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public DbSet<RoomExam> RoomExams { get; set; }
        public DbSet<TeacherSubjectClass> TeacherSubjectClasses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users"); // Cấu hình rõ TPT

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(e => e.Username)
                    .IsUnique();

                entity.Property(e => e.FullName)
                    .HasColumnName("full_name")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasMaxLength(15);

                entity.Property(e => e.Gender)
                    .HasColumnName("gender");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnName("date_of_birth");

                entity.Property(e => e.AvatarUrl)
                    .HasColumnName("avatar_url")
                    .HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasColumnName("status");

                entity.Property(e => e.PasswordHash)
                    .HasColumnName("password_hash")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .IsRequired();

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .IsRequired();

                entity.HasOne(e => e.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.IsCorrect)
                    .HasColumnName("is_correct");

                entity.Property(e => e.AnswerOrder)
                    .HasColumnName("answer_order");

                entity.Property(e => e.Status)
                    .HasColumnName("status");

                entity.Property(e => e.QuestionId)
                    .HasColumnName("question_id")
                    .IsRequired();

                entity.HasOne(e => e.Question)
                    .WithMany(q => q.Answers)
                    .HasForeignKey(e => e.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ClassCode)
                    .HasColumnName("class_code")
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasIndex(e => e.ClassCode)
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DepartmentId)
                    .HasColumnName("department_id")
                    .IsRequired();

                entity.Property(e => e.Status)
                    .HasColumnName("status");

                entity.HasOne(e => e.Department)
                    .WithMany(d => d.Classes)
                    .HasForeignKey(e => e.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.DepartmentCode)
                    .HasColumnName("department_code")
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasIndex(e => e.DepartmentCode)
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Status)
                    .HasColumnName("status");

                entity.HasMany(e => e.Teachers)
                    .WithOne(t => t.Department)
                    .HasForeignKey(t => t.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Classes)
                    .WithOne(c => c.Department)
                    .HasForeignKey(c => c.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                //entity.Property(e => e.ExamCode)
                //    .HasColumnName("exam_code")
                //    .IsRequired()
                //    .HasMaxLength(20);

                //entity.HasIndex(e => e.ExamCode)
                //    .IsUnique();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .IsRequired();

                entity.Property(e => e.DurationMinutes)
                    .HasColumnName("duration_minutes")
                    .IsRequired();

                entity.Property(e => e.NumberOfQuestions)
                    .HasColumnName("number_of_questions")
                    .IsRequired();

                entity.Property(e => e.TotalScore)
                    .HasColumnName("total_score")
                    .IsRequired();

                entity.Property(e => e.RoomExamId)
                    .HasColumnName("room_exam_id") // Sửa từ examsession_id thành exam_session_id
                    .IsRequired();

                entity.Property(e => e.SubjectId)
                    .HasColumnName("subject_id")
                    .IsRequired();

                entity.Property(e => e.Status)
                    .HasColumnName("status");

                entity.HasOne(e => e.Subject)
                    .WithMany(s => s.Exams)
                    .HasForeignKey(e => e.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.RoomExam)
                    .WithMany(es => es.Exams)
                    .HasForeignKey(e => e.RoomExamId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ExamQuestions)
                    .WithOne(eq => eq.Exam)
                    .HasForeignKey(eq => eq.ExamId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ExamQuestion>(entity =>
            {
                entity.HasKey(eq => new { eq.ExamId, eq.QuestionId });

                entity.Property(eq => eq.Order)
                    .HasColumnName("order")
                    .IsRequired();

                entity.Property(eq => eq.Score)
                    .HasColumnName("score")
                    .IsRequired();

                entity.Property(eq => eq.QuestionId)
                    .HasColumnName("question_id")
                    .IsRequired();
                entity.Property(eq => eq.ExamId)
                    .HasColumnName("exam_id")
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

            modelBuilder.Entity<RoomExam>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .IsRequired();

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .IsRequired();

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .IsRequired();
            });

            modelBuilder.Entity<RoomExamSubject>(entity =>
            {
                entity.HasKey(e => new { e.RoomExamId, e.SubjectId });

                entity.Property(e => e.ExamDate)
                    .HasColumnName("exam_date")
                    .IsRequired();

                entity.Property(e => e.RoomExamId)
                    .HasColumnName("room_exam_id")
                    .IsRequired();

                entity.Property(e => e.SubjectId)
                    .HasColumnName("subject_id")
                    .IsRequired();

                entity.HasOne(e => e.RoomExam)
                    .WithMany(es => es.RoomExamSubjects)
                    .HasForeignKey(e => e.RoomExamId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Subject)
                    .WithMany(s => s.RoomExamSubjects)
                    .HasForeignKey(e => e.SubjectId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(q => q.Id);

                entity.Property(q => q.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(q => q.Content)
                    .HasColumnName("content")
                    .IsRequired(false)
                    .HasMaxLength(1000);

                entity.Property(q => q.Topic)
                    .HasColumnName("topic")
                    .IsRequired(false)
                    .HasMaxLength(255);

                entity.Property(q => q.CreatedBy)
                    .HasColumnName("created_by")
                    .IsRequired();

                entity.Property(q => q.Type)
                    .HasColumnName("type")
                    .IsRequired(false)
                    .HasMaxLength(50);

                entity.Property(q => q.Difficulty)
                    .HasColumnName("difficulty") // Sửa từ difficutly thành difficulty
                    .IsRequired(false)
                    .HasMaxLength(50);

                entity.Property(q => q.Status)
                    .HasColumnName("status")
                    .IsRequired();

                entity.HasOne(q => q.Teacher)
                    .WithMany(t => t.Questions)
                    .HasForeignKey(q => q.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(q => q.ExamQuestions)
                    .WithOne(eq => eq.Question)
                    .HasForeignKey(eq => eq.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(q => q.Answers)
                    .WithOne(a => a.Question)
                    .HasForeignKey(a => a.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(r => r.Name)
                    .HasColumnName("name")
                    .IsRequired(false)
                    .HasMaxLength(100);

                entity.HasMany(r => r.Users)
                    .WithOne(u => u.Role)
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Students"); // Cấu hình rõ TPT
                modelBuilder.Entity<Student>().HasBaseType<User>();

                entity.Property(s => s.StudentCode)
                    .HasColumnName("student_code")
                    .IsRequired(false)
                    .HasMaxLength(50);

                entity.Property(s => s.IsFirstTimeLogin)
                    .HasColumnName("is_first_time_login")
                    .IsRequired();

                entity.Property(s => s.ClassId)
                    .HasColumnName("class_id")
                    .IsRequired();

                entity.HasOne(s => s.Class)
                    .WithMany(c => c.Students)
                    .HasForeignKey(s => s.ClassId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(s => s.SubjectCode)
                    .HasColumnName("subject_code")
                    .IsRequired(false)
                    .HasMaxLength(50);

                entity.Property(s => s.Name)
                    .HasColumnName("name")
                    .IsRequired(false)
                    .HasMaxLength(200);

                entity.Property(s => s.DepartmentId)
                    .HasColumnName("department_id")
                    .IsRequired();

                entity.Property(s => s.Status)
                    .HasColumnName("status")
                    .IsRequired();

                entity.HasMany(s => s.Exams)
                    .WithOne(e => e.Subject)
                    .HasForeignKey(e => e.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.Department)
                    .WithMany(d => d.Subjects)
                    .HasForeignKey(s => s.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(s => s.RoomExamSubjects)
                    .WithOne(ess => ess.Subject)
                    .HasForeignKey(ess => ess.SubjectId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(s => s.TeacherSubjectClasses)
                    .WithOne(tsc => tsc.Subject)
                    .HasForeignKey(tsc => tsc.SubjectId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("Teachers"); // Cấu hình rõ TPT
                modelBuilder.Entity<Teacher>().HasBaseType<User>(); // Sửa từ Student thành Teacher

                entity.Property(t => t.TeacherCode)
                    .HasColumnName("teacher_code")
                    .IsRequired(false)
                    .HasMaxLength(50);

                entity.Property(t => t.IsFirstTimeLogin)
                    .HasColumnName("is_first_time_login")
                    .IsRequired();

                entity.Property(t => t.DepartmentId)
                    .HasColumnName("department_id")
                    .IsRequired();

                entity.HasMany(t => t.Questions)
                    .WithOne(q => q.Teacher)
                    .HasForeignKey(q => q.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Department)
                    .WithMany(d => d.Teachers)
                    .HasForeignKey(t => t.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(t => t.TeacherSubjectClasses)
                    .WithOne(tsc => tsc.Teacher)
                    .HasForeignKey(tsc => tsc.TeacherId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TeacherSubjectClass>(entity =>
            {
                entity.HasKey(tsc => new { tsc.TeacherId, tsc.SubjectId, tsc.ClassId });

                entity.Property(eq => eq.SubjectId)
                    .HasColumnName("subject_id")
                    .IsRequired();

                entity.Property(eq => eq.ClassId)
                    .HasColumnName("class_id")
                    .IsRequired();

                entity.Property(eq => eq.ClassId)
                    .HasColumnName("class_id")
                    .IsRequired();

                entity.HasOne(tsc => tsc.Teacher)
                    .WithMany(t => t.TeacherSubjectClasses)
                    .HasForeignKey(tsc => tsc.TeacherId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(tsc => tsc.Subject)
                    .WithMany(s => s.TeacherSubjectClasses)
                    .HasForeignKey(tsc => tsc.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(tsc => tsc.Class)
                    .WithMany(c => c.TeacherSubjectClasses)
                    .HasForeignKey(tsc => tsc.ClassId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<QuestionBank>(entity =>
            {
                entity.HasKey(qb => qb.Id);

                entity.Property(qb => qb.Name)
                      .HasColumnName("name")
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(qb => qb.Description)
                      .HasColumnName("description")
                      .HasMaxLength(1000);

                entity.Property(qb => qb.SubjectId)
                      .IsRequired()
                      .HasColumnName("subject_id");

                entity.HasMany(qb => qb.questions)
                      .WithOne(q => q.QuestionBank)
                      .HasForeignKey(q => q.QuestionBankId);

                entity.HasOne(qb => qb.Subject)
                      .WithMany(s => s.QuestionBanks)
                      .HasForeignKey(qb => qb.SubjectId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
        public DbSet<QuizSystem_backend.Models.GeneratedExam> GeneratedExam { get; set; } = default!;
    }
}