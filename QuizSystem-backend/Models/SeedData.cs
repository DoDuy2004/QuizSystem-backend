using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace QuizSystem_backend.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<QuizSystemDbContext>();

                // Áp dụng migration nếu có migration chưa được thực thi
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                var currentDate = DateTime.UtcNow;

                // Seed Roles
                if (!context.Roles.Any())
                {
                    var roles = new[]
                    {
                        new Role { Name = "Admin" },
                        new Role { Name = "Teacher" },
                        new Role { Name = "Student" }
                    };
                    context.Roles.AddRange(roles);
                    context.SaveChanges();
                }

                // Seed Departments
                if (!context.Departments.Any())
                {
                    var departments = new[]
                    {
                        new Department { DepartmentCode = "CS", Name = "Computer Science", Status = 1 },
                        new Department { DepartmentCode = "MATH", Name = "Mathematics", Status = 1 }
                    };
                    context.Departments.AddRange(departments);
                    context.SaveChanges();
                }

                // Seed Users
                if (!context.Users.Any())
                {
                    var adminUser = new User
                    {
                        Username = "admin1",
                        FullName = "Admin User",
                        Email = "admin@example.com",
                        PhoneNumber = "1234567890",
                        Gender = true,
                        DateOfBirth = new DateTime(1990, 1, 1),
                        AvatarUrl = "http://example.com/admin.jpg",
                        Status = true,
                        PasswordHash = "hashedpassword1",
                        CreatedAt = currentDate,
                        RoleId = 1 // Giả định RoleId khớp với thứ tự chèn
                    };

                    context.Users.AddRange(adminUser);
                    context.SaveChanges();
                }

                // Seed Classes
                if (!context.Classes.Any())
                {
                    var classes = new[]
                    {
                        new Class { ClassCode = "CS101", Name = "Intro to CS", DepartmentId = 1, Status = 1 }
                    };
                    context.Classes.AddRange(classes);
                    context.SaveChanges();
                }

                // Seed Students
                if (!context.Students.Any())
                {
                    var student = new Student
                    {
                        Username = "student1",
                        FullName = "Student One",
                        Email = "student1@example.com",
                        PhoneNumber = "0987654321",
                        Gender = false,
                        DateOfBirth = new DateTime(2003, 5, 15),
                        AvatarUrl = "http://example.com/student1.jpg",
                        Status = true,
                        PasswordHash = "hashedpassword2",
                        CreatedAt = currentDate,
                        RoleId = 3,
                        StudentCode = "STU001",
                        IsFirstTimeLogin = true,
                        ClassId = 1 // Giả định ClassId sẽ được chèn sau
                    };
                    context.Students.Add(student);
                    context.SaveChanges();
                }

                // Seed Teachers
                if (!context.Teachers.Any())
                {
                    var teacher = new Teacher
                    {
                        Username = "teacher1",
                        FullName = "Teacher One",
                        Email = "teacher1@example.com",
                        PhoneNumber = "0123456789",
                        Gender = true,
                        DateOfBirth = new DateTime(1985, 3, 10),
                        AvatarUrl = "http://example.com/teacher1.jpg",
                        Status = true,
                        PasswordHash = "hashedpassword3",
                        CreatedAt = currentDate,
                        RoleId = 2,
                        TeacherCode = "TCH001",
                        IsFirstTimeLogin = true,
                        DepartmentId = 1 // Giả định DepartmentId sẽ được chèn trước
                    };
                    context.Teachers.Add(teacher);
                    context.SaveChanges();
                }

                // Seed Subjects
                if (!context.Subjects.Any())
                {
                    var subjects = new[]
                    {
                        new Subject { SubjectCode = "CS101", Name = "Programming 101", DepartmentId = 1, Status = 1 }
                    };
                    context.Subjects.AddRange(subjects);
                    context.SaveChanges();
                }

                // Seed ExamSessions
                if (!context.ExamSessions.Any())
                {
                    var examSessions = new[]
                    {
                        new ExamSession
                        {
                            Name = "Midterm June 2025",
                            StartDate = new DateTime(2025, 6, 15),
                            EndDate = new DateTime(2025, 6, 20),
                            Status = 1
                        }
                    };
                    context.ExamSessions.AddRange(examSessions);
                    context.SaveChanges();
                }

                // Seed Exams
                if (!context.Exams.Any())
                {
                    var exams = new[]
                    {
                        new Exam
                        {
                            ExamCode = "EXM001",
                            Name = "Programming Exam",
                            StartDate = new DateTime(2025, 6, 15, 9, 0, 0),
                            DurationMinutes = 90,
                            NumberOfQuestions = 10,
                            TotalScore = 100,
                            SubjectId = 1, // Giả định SubjectId khớp
                            Status = 1,
                            ExamSessionId = 1 // Giả định ExamSessionId khớp
                        }
                    };
                    context.Exams.AddRange(exams);
                    context.SaveChanges();
                }

                // Seed Questions
                if (!context.Questions.Any())
                {
                    var questions = new[]
                    {
                        new Question
                        {
                            Content = "What is a variable?",
                            CreatedBy = 3, // Giả định TeacherId = 3
                            Type = "Multiple Choice",
                            Difficulty = "Easy",
                            Status = 1
                        }
                    };
                    context.Questions.AddRange(questions);
                    context.SaveChanges();
                }

                // Seed Answers
                if (!context.Answers.Any())
                {
                    var answers = new[]
                    {
                        new Answer { Content = "A storage location", IsCorrect = true, AnswerOrder = 1, Status = 1, QuestionId = 1 },
                        new Answer { Content = "A function", IsCorrect = false, AnswerOrder = 2, Status = 1, QuestionId = 1 }
                    };
                    context.Answers.AddRange(answers);
                    context.SaveChanges();
                }

                // Seed ExamQuestions
                if (!context.ExamQuestions.Any())
                {
                    var examQuestions = new[]
                    {
                        new ExamQuestion { ExamId = 1, QuestionId = 1, Order = 1, Score = 10 }
                    };
                    context.ExamQuestions.AddRange(examQuestions);
                    context.SaveChanges();
                }

                // Seed ExamSessionSubjects
                if (!context.ExamSessionSubjects.Any())
                {
                    var examSessionSubjects = new[]
                    {
                        new ExamSessionSubject
                        {
                            ExamSessionId = 1,
                            SubjectId = 1,
                            ExamDate = new DateTime(2025, 6, 15)
                        }
                    };
                    context.ExamSessionSubjects.AddRange(examSessionSubjects);
                    context.SaveChanges();
                }

                // Seed TeacherSubjectClasses
                if (!context.TeacherSubjectClasses.Any())
                {
                    var teacherSubjectClasses = new[]
                    {
                        new TeacherSubjectClass
                        {
                            TeacherId = 3,
                            SubjectId = 1,
                            ClassId = 1
                        }
                    };
                    context.TeacherSubjectClasses.AddRange(teacherSubjectClasses);
                    context.SaveChanges();
                }
            }
        }
    }
}