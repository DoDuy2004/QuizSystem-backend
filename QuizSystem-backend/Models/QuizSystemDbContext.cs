using Microsoft.EntityFrameworkCore;

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
        public virtual DbSet<ExamSessionSubject> ExamSessionSubjects { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public DbSet<ExamSession> ExamSessions { get; set; }
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

                // Nếu bạn muốn số điện thoại unique:
                // entity.HasIndex(e => e.PhoneNumber).IsUnique();

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
                    .WithMany(r => r.Users) // assuming Role has public ICollection<User> Users
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
                    .HasMaxLength(1000); // Bạn có thể chọn 500 nếu muốn

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
                    .WithMany(q => q.Answers) // nhớ thêm trong Question: ICollection<Answer> Answers
                    .HasForeignKey(e => e.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade); // Khi xoá Question, xoá luôn Answer
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
                    .WithMany(d => d.Classes) // cần thêm ICollection<Class> Classes trong Department
                    .HasForeignKey(e => e.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Navigation với Student đã ok (Collection), không cần cấu hình gì thêm nếu để default.
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

                // Navigation properties:
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

                entity.Property(e => e.ExamCode)
                    .HasColumnName("exam_code")
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasIndex(e => e.ExamCode)
                    .IsUnique();

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

                entity.Property(e => e.SubjectId)
                    .HasColumnName("subject_id")
                    .IsRequired();

                entity.Property(e => e.Status)
                    .HasColumnName("status");

                entity.HasOne(e => e.Subject)
                    .WithMany(s => s.Exams) // cần thêm ICollection<Exam> Exams trong Subject
                    .HasForeignKey(e => e.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.ExamQuestions)
                    .WithOne(eq => eq.Exam)
                    .HasForeignKey(eq => eq.ExamId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ExamQuestion>(entity =>
            {
                entity.HasKey(eq => new { eq.ExamId, eq.QuestionId }); // composite PK

                entity.Property(eq => eq.Order)
                    .HasColumnName("order")
                    .IsRequired();

                entity.Property(eq => eq.Score)
                    .HasColumnName("score")
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

            modelBuilder.Entity<ExamSession>(entity =>
            {
                entity.HasKey(e => e.Id);

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

            modelBuilder.Entity<ExamSessionSubject>(entity =>
            {
                entity.HasKey(e => new { e.ExamSessionId, e.SubjectId });

                entity.Property(e => e.ExamDate)
                    .HasColumnName("exam_date")
                    .IsRequired();

                entity.HasOne(e => e.ExamSession)
                    .WithMany(es => es.ExamSessionSubjects)
                    .HasForeignKey(e => e.ExamSessionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Subject)
                    .WithMany(s => s.ExamSessionSubjects)
                    .HasForeignKey(e => e.SubjectId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(q => q.Id); 

                entity.Property(q => q.Content)
                    .HasColumnName("content")
                    .IsRequired(false) 
                    .HasMaxLength(1000);

                entity.Property(q => q.CreatedBy)
                    .HasColumnName("created_by")
                    .IsRequired();

                entity.Property(q => q.Type)
                    .HasColumnName("type")
                    .IsRequired(false)
                    .HasMaxLength(50);

                entity.Property(q => q.Difficulty)
                    .HasColumnName("difficutly")
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

                // Mối quan hệ với Answer (một-nhiều)
                entity.HasMany(q => q.Answers)
                    .WithOne(a => a.Question) 
                    .HasForeignKey(a => a.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade); // Xóa cascade nếu Question bị xóa
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);

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
                //entity.HasKey(s => s.Id); // Kế thừa Id từ User

                entity.Property(s => s.StudentCode)
                    .HasColumnName("student_code")
                    .IsRequired(false)
                    .HasMaxLength(50);

                entity.Property(s => s.isFirstTimeLogin)
                    .HasColumnName("is_first_time_login")
                    .IsRequired();

                entity.Property(s => s.ClassId)
                    .HasColumnName("class_id")
                    .IsRequired();

                entity.HasOne(s => s.Class)
                    .WithMany(c => c.Students) // Giả định Class có ICollection<Student> Students
                    .HasForeignKey(s => s.ClassId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(s => s.Id);

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

                entity.HasMany(s => s.ExamSessionSubjects)
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
                modelBuilder.Entity<Student>().HasBaseType<User>();
                //entity.HasKey(t => t.Id); // Kế thừa Id từ User


                entity.Property(t => t.TeacherCode)
                    .HasColumnName("teacher_code")
                    .IsRequired(false)
                    .HasMaxLength(50);

                entity.Property(t => t.isFirstTimeLogin)
                    .HasColumnName("is_first_time_login")
                    .IsRequired();

                entity.Property(t => t.DepartmentId)
                    .HasColumnName("department_id")
                    .IsRequired();

                entity.HasMany(t => t.Questions)
                    .WithOne(q => q.Teacher) // Giả định Question có Teacher
                    .HasForeignKey(q => q.CreatedBy) // Giả định CreatedBy là khóa ngoại
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Department)
                    .WithMany(d => d.Teachers) // Giả định Department có ICollection<Teacher> Teachers
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
        }
    }
}
