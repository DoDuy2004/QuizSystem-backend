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

                // Seed Admin User
                if (!context.Users.Any())
                {
                    var admin = new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "admin1",
                        FullName = "Admin Nguyễn Văn A",
                        Email = "admin@example.com",
                        PhoneNumber = "0987654321",
                        Gender = true,
                        DateOfBirth = new DateTime(1980, 5, 15),
                        AvatarUrl = "http://example.com/admin.jpg",
                        Status = Status.ACTIVE,
                        PasswordHash = "hashedpassword1",
                        CreatedAt = currentDate,
                        Role = Role.ADMIN
                    };
                    context.Users.Add(admin);
                    context.SaveChanges();
                }

                // Seed Teacher
                if (!context.Teachers.Any())
                {
                    var teacher = new Teacher
                    {
                        Id = Guid.NewGuid(),
                        Username = "teacher1",
                        FullName = "Thầy Trần Văn B",
                        Email = "teacher1@example.com",
                        PhoneNumber = "0912345678",
                        Gender = true,
                        DateOfBirth = new DateTime(1985, 3, 10),
                        AvatarUrl = "http://example.com/teacher1.jpg",
                        Status = Status.ACTIVE,
                        PasswordHash = "hashedpassword2",
                        CreatedAt = currentDate,
                        Facutly = "Công Nghệ Thông Tin",
                        Role = Role.TEACHER
                    };
                    context.Teachers.Add(teacher);
                    context.SaveChanges();
                }

                // Seed 20 Students
                if (!context.Students.Any())
                {
                    var students = new Student[20];
                    for (int i = 1; i <= 20; i++)
                    {
                        students[i - 1] = new Student
                        {
                            Id = Guid.NewGuid(),
                            Username = $"student{i}",
                            FullName = $"Đỗ Đình Duy {i}",
                            Email = $"student{i}@example.com",
                            PhoneNumber = $"01234567{i:00}",
                            Gender = i % 2 == 0, // Alternating gender
                            DateOfBirth = new DateTime(2003, i % 12 + 1, 15),
                            AvatarUrl = $"http://example.com/student{i}.jpg",
                            Status = Status.ACTIVE,
                            Facutly = "Công Nghệ Thông Tin",
                            PasswordHash = "123456789",
                            CreatedAt = currentDate
                        };
                    }
                    context.Students.AddRange(students);
                    context.SaveChanges();
                }

                var teacherId = context.Teachers.First().Id;
                var studentIds = context.Students.Select(s => s.Id).ToList();

                // Seed CourseClass
                if (!context.CourseClasses.Any())
                {
                    var courseClass = new CourseClass
                    {
                        Id = Guid.NewGuid(),
                        ClassCode = "CDTH22WEBC - PPLTHDT",
                        Name = "CDTH22WEBC - PPLTHDT",
                        Credit = 3,
                        Status = Status.ACTIVE,
                        Subject = "Lập trình hướng đối tượng",
                        TeacherId = teacherId
                    };
                    context.CourseClasses.Add(courseClass);
                    context.SaveChanges();
                }

                var courseClassId = context.CourseClasses.First().Id;

                // Seed Chapters
                if (!context.Chapters.Any())
                {
                    var chapters = new[]
                    {
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 1: Cơ bản về Lập trình hướng đối tượng",
                            Description = "Giới thiệu các khái niệm cơ bản về OOP",
                            Status = Status.ACTIVE,
                            CourseClassId = courseClassId
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 2: Lớp và Đối tượng",
                            Description = "Tạo và sử dụng lớp, đối tượng",
                            Status = Status.ACTIVE,
                            CourseClassId = courseClassId
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
                            Name = "Ngân hàng câu hỏi OOP",
                            Description = "Câu hỏi liên quan đến Lập trình hướng đối tượng",
                            Status = Status.ACTIVE,
                            CourseClassId = courseClassId
                        }
                    };
                    context.QuestionBanks.AddRange(questionBanks);
                    context.SaveChanges();
                }

                var questionBankId = context.QuestionBanks.First().Id;
                var chapter1Id = context.Chapters.First().Id;
                var chapter2Id = context.Chapters.Skip(1).First().Id;

                // Seed Questions
                if (!context.Questions.Any())
                {
                    var questions = new[]
                    {
                        new Question
                        {
                            Id = Guid.NewGuid(),
                            Content = "Khái niệm nào dưới đây là đặc trưng của Lập trình hướng đối tượng?",
                            Image = "",
                            CreatedBy = teacherId,
                            Type = "Multiple Choice",
                            Difficulty = "Easy",
                            Status = Status.ACTIVE,
                            Topic = "OOP Basics",
                            QuestionBankId = questionBankId,
                            ChapterId = chapter1Id
                        },
                        new Question
                        {
                            Id = Guid.NewGuid(),
                            Content = "Lớp trong Lập trình hướng đối tượng được định nghĩa như thế nào?",
                            Image = "",
                            CreatedBy = teacherId,
                            Type = "Multiple Choice",
                            Difficulty = "Medium",
                            Status = Status.ACTIVE,
                            Topic = "Classes",
                            QuestionBankId = questionBankId,
                            ChapterId = chapter2Id
                        },
                        new Question
                        {
                            Id = Guid.NewGuid(),
                            Content = "Đối tượng là gì trong Lập trình hướng đối tượng?",
                            Image = "",
                            CreatedBy = teacherId,
                            Type = "Multiple Choice",
                            Difficulty = "Easy",
                            Status = Status.ACTIVE,
                            Topic = "Objects",
                            QuestionBankId = questionBankId,
                            ChapterId = chapter2Id
                        }
                    };
                    context.Questions.AddRange(questions);
                    context.SaveChanges();
                }

                var questionIds = context.Questions.Select(q => q.Id).ToList();

                // Seed Answers
                if (!context.Answers.Any())
                {
                    var answers = new[]
                    {
                        new Answer { Id = Guid.NewGuid(), Content = "Kế thừa", IsCorrect = true, AnswerOrder = 1, Status = Status.ACTIVE, QuestionId = questionIds[0] },
                        new Answer { Id = Guid.NewGuid(), Content = "Vòng lặp", IsCorrect = false, AnswerOrder = 2, Status = Status.ACTIVE, QuestionId = questionIds[0] },
                        new Answer { Id = Guid.NewGuid(), Content = "Hàm toán học", IsCorrect = false, AnswerOrder = 3, Status = Status.ACTIVE, QuestionId = questionIds[0] },
                        new Answer { Id = Guid.NewGuid(), Content = "Một bản thiết kế cho đối tượng", IsCorrect = true, AnswerOrder = 1, Status = Status.ACTIVE, QuestionId = questionIds[1] },
                        new Answer { Id = Guid.NewGuid(), Content = "Một biến số", IsCorrect = false, AnswerOrder = 2, Status = Status.ACTIVE, QuestionId = questionIds[1] },
                        new Answer { Id = Guid.NewGuid(), Content = "Một instance của lớp", IsCorrect = true, AnswerOrder = 1, Status = Status.ACTIVE, QuestionId = questionIds[2] },
                        new Answer { Id = Guid.NewGuid(), Content = "Một hàm tĩnh", IsCorrect = false, AnswerOrder = 2, Status = Status.ACTIVE, QuestionId = questionIds[2] }
                    };
                    context.Answers.AddRange(answers);
                    context.SaveChanges();
                }

                // Seed RoomExams
                if (!context.RoomExams.Any())
                {
                    var roomExam = new RoomExam
                    {
                        Id = Guid.NewGuid(),
                        Name = "Phòng thi A101",
                        StartDate = new DateTime(2025, 6, 25, 8, 0, 0),
                        EndDate = new DateTime(2025, 6, 25, 10, 0, 0),
                        Status = Status.ACTIVE,
                        CourseClassId = courseClassId
                    };
                    context.RoomExams.Add(roomExam);
                    context.SaveChanges();
                }

                var roomExamId = context.RoomExams.First().Id;

                // Seed Exams
                if (!context.Exams.Any())
                {
                    var exam = new Exam
                    {
                        Id = Guid.NewGuid(),
                        ExamCode = "PPLTHDT-MID",
                        Name = "Giữa kỳ Lập trình hướng đối tượng",
                        StartDate = new DateTime(2025, 6, 25, 8, 30, 0),
                        DurationMinutes = 90,
                        NumberOfQuestions = 3,
                        RoomExamId = roomExamId,
                        Status = Status.ACTIVE
                    };
                    context.Exams.Add(exam);
                    context.SaveChanges();
                }

                var examId = context.Exams.First().Id;

                // Seed ExamQuestions
                if (!context.ExamQuestions.Any())
                {
                    var examQuestions = new[]
                    {
                        new ExamQuestion { ExamId = examId, QuestionId = questionIds[0], Order = 1, Score = 10 },
                        new ExamQuestion { ExamId = examId, QuestionId = questionIds[1], Order = 2, Score = 10 },
                        new ExamQuestion { ExamId = examId, QuestionId = questionIds[2], Order = 3, Score = 10 }
                    };
                    context.ExamQuestions.AddRange(examQuestions);
                    context.SaveChanges();
                }

                // Seed StudentCourseClasses
                if (!context.StudentCourseClasses.Any())
                {
                    var studentCourseClasses = studentIds.Select(s => new StudentCourseClass
                    {
                        StudentId = s,
                        CourseClassId = courseClassId,
                        Grade = null,
                        Note = "Chưa có điểm",
                        Status = Status.ACTIVE
                    }).ToArray();
                    context.StudentCourseClasses.AddRange(studentCourseClasses);
                    context.SaveChanges();
                }

                // Seed StudentExams
                if (!context.StudentExams.Any())
                {
                    var studentExams = studentIds.Select(s => new StudentExam
                    {
                        Id = Guid.NewGuid(),
                        StudentId = s,
                        CourseClassId = courseClassId,
                        ExamId = examId,
                        DurationMinutes = 90,
                        Status = Status.ACTIVE
                    }).ToArray();
                    context.StudentExams.AddRange(studentExams);
                    context.SaveChanges();
                }

                var studentExamIds = context.StudentExams.Select(se => se.Id).ToList();
                var answerIds = context.Answers.Select(a => a.Id).ToList();

                // Seed StudentExamDetails (Bài làm của học sinh)
                if (!context.StudentExamDetails.Any())
                {
                    var studentExamDetails = new List<StudentExamDetail>();
                    for (int i = 0; i < studentIds.Count; i++)
                    {
                        // Sinh viên trả lời ngẫu nhiên, một số đúng, một số sai
                        studentExamDetails.Add(new StudentExamDetail
                        {
                            AnswerId = i % 2 == 0 ? answerIds[i % 4] : answerIds[(i % 4) + 1], // Chọn câu trả lời ngẫu nhiên
                            QuestionId = questionIds[0],
                            StudentExamId = studentExamIds[i]
                        });
                        studentExamDetails.Add(new StudentExamDetail
                        {
                            AnswerId = i % 2 == 0 ? answerIds[3] : answerIds[4], // Câu 2: Một số đúng, một số sai
                            QuestionId = questionIds[1],
                            StudentExamId = studentExamIds[i]
                        });
                        studentExamDetails.Add(new StudentExamDetail
                        {
                            AnswerId = i % 2 == 0 ? answerIds[5] : answerIds[6], // Câu 3: Một số đúng, một số sai
                            QuestionId = questionIds[2],
                            StudentExamId = studentExamIds[i]
                        });
                    }
                    context.StudentExamDetails.AddRange(studentExamDetails);
                    context.SaveChanges();
                }
            }
        }
    }
}