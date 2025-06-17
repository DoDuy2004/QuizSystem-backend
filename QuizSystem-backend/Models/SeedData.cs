using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizSystem_backend.Models;
using QuizSystem_backend.Enums;
using System;

namespace QuizSystem_backend.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<QuizSystemDbContext>();

                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                var currentDate = DateTime.UtcNow;

                if (!context.Users.Any())
                {
                    var users = new[]
                    {
                        new User
                        {
                            Id =  Guid.NewGuid(),
                            Username = "admin1",
                            FullName = "Admin User",
                            Email = "admin@example.com",
                            PhoneNumber = "1234567890",
                            Gender = true,
                            DateOfBirth = new DateTime(1990, 1, 1),
                            AvatarUrl = "http://example.com/admin.jpg",
                            Status = Status.ACTIVE,
                            PasswordHash = "hashedpassword1",
                            CreatedAt = currentDate,
                            Role = Role.ADMIN
                        },
                    };
                    context.Users.AddRange(users);
                    context.SaveChanges();
                }

                // Seed Facutlies
                if (!context.Facutlies.Any())
                {
                    var facutlies = new[]
                    {
                        new Facutly
                        {
                            Id = Guid.NewGuid(),
                            FacutlyCode = "CS",
                            Name = "Computer Science",
                            Status = Status.ACTIVE
                        }
                    };
                    context.Facutlies.AddRange(facutlies);
                    context.SaveChanges();
                }

                // Seed Subjects
                if (!context.Subjects.Any())
                {
                    var subjects = new[]
                    {
                        new Subject
                        {
                            Id = Guid.NewGuid(),
                            SubjectCode = "CS101",
                            Name = "Introduction to Programming",
                            FacutlyId = context.Facutlies.First().Id,
                            Status = Status.ACTIVE
                        }
                    };
                    context.Subjects.AddRange(subjects);
                    context.SaveChanges();
                }

                // Seed Teachers
                if (!context.Teachers.Any())
                {
                    var teachers = new[]
                    {
                        new Teacher
                        {
                            Id = Guid.NewGuid(),
                            Username = "teacher1",
                            FullName = "Teacher One",
                            Email = "teacher1@example.com",
                            PhoneNumber = "0987654321",
                            Gender = true,
                            DateOfBirth = new DateTime(1985, 3, 10),
                            AvatarUrl = "http://example.com/teacher1.jpg",
                            Status = Status.ACTIVE,
                            PasswordHash = "hashedpassword2",
                            CreatedAt = currentDate,
                            Role = Role.TEACHER
                        }
                    };
                    context.Teachers.AddRange(teachers);
                    context.SaveChanges();
                }

                // Seed Students
                if (!context.Students.Any())
                {
                    var students = new[]
                    {
                        new Student
                        {
                            Id = Guid.NewGuid(),
                            Username = "student1",
                            FullName = "Student One",
                            Email = "student1@example.com",
                            PhoneNumber = "0123456789",
                            Gender = false,
                            DateOfBirth = new DateTime(2003, 5, 15),
                            AvatarUrl = "http://example.com/student1.jpg",
                            Status = Status.ACTIVE,
                            PasswordHash = "hashedpassword3",
                            CreatedAt = currentDate
                        }
                    };
                    context.Students.AddRange(students);
                    context.SaveChanges();
                }

                // Seed Chapters
                if (!context.Chapters.Any())
                {
                    var chapters = new[]
                    {
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chapter 1: Basics",
                            Description = "Introduction to Programming",
                            Status = Status.ACTIVE,
                            SubjectId = context.Subjects.First().Id
                        }
                    };
                    context.Chapters.AddRange(chapters);
                    context.SaveChanges();
                }

                // Seed QuestionBanks
                if (!context.QuestionBanks.Any())
                {
                    var questionBanks = new[]
                    {
                        new QuestionBank
                        {
                            Id = Guid.NewGuid(),
                            Name = "Programming Basics",
                            Description = "Basic programming questions",
                            Status = Status.ACTIVE,
                            SubjectId = context.Subjects.First().Id
                        }
                    };
                    context.QuestionBanks.AddRange(questionBanks);
                    context.SaveChanges();
                }

                // Seed Questions
                if (!context.Questions.Any())
                {
                    var questions = new[]
                    {
                        new Question
                        {
                            Id = Guid.NewGuid(),
                            Content = "What is a variable?",
                            Image = "",
                            CreatedBy = context.Teachers.First().Id,
                            Type = "Multiple Choice",
                            Difficulty = "Easy",
                            Status = Status.ACTIVE,
                            Topic = "Variables",
                            QuestionBankId = context.QuestionBanks.First().Id,
                            ChapterId = context.Chapters.First().Id
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
                        new Answer
                        {
                            Id = Guid.NewGuid(),
                            Content = "A storage location",
                            IsCorrect = true,
                            AnswerOrder = 1,
                            Status = Status.ACTIVE,
                            QuestionId = context.Questions.First().Id
                        },
                        new Answer
                        {
                            Id = Guid.NewGuid(),
                            Content = "A function",
                            IsCorrect = false,
                            AnswerOrder = 2,
                            Status = Status.ACTIVE,
                            QuestionId = context.Questions.First().Id
                        }
                    };
                    context.Answers.AddRange(answers);
                    context.SaveChanges();
                }

                // Seed CourseClasses
                if (!context.CourseClasses.Any())
                {
                    var courseClasses = new[]
                    {
                        new CourseClass
                        {
                            Id = Guid.NewGuid(),
                            ClassCode = "CS101-2025",
                            Name = "Programming 101",
                            Credit = 3,
                            Status = Status.ACTIVE, // Sử dụng int thay vì enum trực tiếp
                            TeacherId = context.Teachers.First().Id,
                            SubjectId = context.Subjects.First().Id
                        }
                    };
                    context.CourseClasses.AddRange(courseClasses);
                    context.SaveChanges();
                }

                // Seed RoomExams
                if (!context.RoomExams.Any())
                {
                    var roomExams = new[]
                    {
                        new RoomExam
                        {
                            Id = Guid.NewGuid(),
                            Name = "Room 101",
                            StartDate = new DateTime(2025, 6, 20),
                            EndDate = new DateTime(2025, 6, 20, 12, 0, 0),
                            Status = Status.ACTIVE,
                            CourseClassId = context.CourseClasses.First().Id
                        }
                    };
                    context.RoomExams.AddRange(roomExams);
                    context.SaveChanges();
                }

                // Seed Exams
                if (!context.Exams.Any())
                {
                    var exams = new[]
                    {
                        new Exam
                        {
                            Id = Guid.NewGuid(),
                            ExamCode = "EXM001",
                            Name = "Midterm Exam",
                            StartDate = new DateTime(2025, 6, 20, 10, 0, 0),
                            DurationMinutes = 90,
                            NumberOfQuestions = 10,
                            TotalScore = 100,
                            RoomExamId = context.RoomExams.First().Id,
                            Status = Status.ACTIVE
                        }
                    };
                    context.Exams.AddRange(exams);
                    context.SaveChanges();
                }

                // Seed ExamQuestions
                if (!context.ExamQuestions.Any())
                {
                    var examQuestions = new[]
                    {
                        new ExamQuestion
                        {
                            ExamId = context.Exams.First().Id,
                            QuestionId = context.Questions.First().Id,
                            Order = 1,
                            Score = 10
                        }
                    };
                    context.ExamQuestions.AddRange(examQuestions);
                    context.SaveChanges();
                }

                // Seed StudentCourseClasses
                if (!context.StudentCourseClasses.Any())
                {
                    var studentCourseClasses = new[]
                    {
                        new StudentCourseClass
                        {
                            StudentId = context.Students.First().Id,
                            CourseClass = context.CourseClasses.First().Id,
                            Grade = null,
                            note = "Good student",
                            Status = Status.ACTIVE
                        }
                    };
                    context.StudentCourseClasses.AddRange(studentCourseClasses);
                    context.SaveChanges();
                }

                // Seed StudentExams
                if (!context.StudentExams.Any())
                {
                    var studentExams = new[]
                    {
                        new StudentExam
                        {
                            Id = Guid.NewGuid(),
                            StudentId = context.StudentCourseClasses.First().StudentId,
                            CourseClass = context.StudentCourseClasses.First().CourseClass,
                            ExamId = context.Exams.First().Id,
                            DurationMinutes = 90,
                            Status = Status.ACTIVE
                        }
                    };
                    context.StudentExams.AddRange(studentExams);
                    context.SaveChanges();
                }

                // Seed StudentExamDetails
                if (!context.StudentExamDetails.Any())
                {
                    var studentExamDetails = new[]
                    {
                        new StudentExamDetail
                        {
                            AnswerId = context.Answers.First(a => a.IsCorrect).Id,
                            QuestionId = context.Questions.First().Id,
                            StudentExamId = context.StudentExams.First().StudentId // Giả sử dùng StudentId làm khóa
                        }
                    };
                    context.StudentExamDetails.AddRange(studentExamDetails);
                    context.SaveChanges();
                }
            }
        }
    }
}