using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizSystem_backend.Models;
using QuizSystem_backend.Enums;
using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

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

                var hasher = new PasswordHasher<User>();

                // Seed Admin User
                if (!context.Users.Any())
                {
                    var admin = new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "admin",
                        FullName = "Nguyễn Đức Duy",
                        Email = "admin@caothang.edu.vn",
                        PhoneNumber = "0987654321",
                        Gender = true,
                        DateOfBirth = new DateTime(2004, 6, 20),
                        AvatarUrl = "",
                        Status = Status.ACTIVE,
                        PasswordHash = hasher.HashPassword(null, "123456789"),
                        CreatedAt = currentDate,
                        Role = Role.ADMIN
                    };
                    context.Users.Add(admin);
                    context.SaveChanges();
                }

                // Seed Teacher
                if (!context.Teachers.Any())
                {
                    var teachers = new List<Teacher> {
                        new Teacher
                        {
                            Id = Guid.NewGuid(),
                            Username = "ndduy",
                            FullName = "Nguyễn Đức Duy",
                            Email = "ndduy@caothang.edu.vn",
                            PhoneNumber = "0912345678",
                            Gender = true,
                            DateOfBirth = new DateTime(1985, 3, 10),
                            AvatarUrl = "",
                            Status = Status.ACTIVE,
                            PasswordHash = hasher.HashPassword(null, "123456789"),
                            CreatedAt = currentDate,
                            Facutly = "Công Nghệ Thông Tin",
                            Role = Role.TEACHER
                        },
                        new Teacher
                        {
                            Id = Guid.NewGuid(),
                            Username = "lctien",
                            FullName = "Lữ Cao Tiến",
                            Email = "lctien@caothang.edu.vn",
                            PhoneNumber = "0912345678",
                            Gender = true,
                            DateOfBirth = new DateTime(1985, 3, 10),
                            AvatarUrl = "",
                            Status = Status.ACTIVE,
                            PasswordHash = hasher.HashPassword(null, "123456789"),
                            CreatedAt = currentDate,
                            Facutly = "Công Nghệ Thông Tin",
                            Role = Role.TEACHER
                        },
                        new Teacher
                        {
                            Id = Guid.NewGuid(),
                            Username = "pkanh",
                            FullName = "Phù Khác Anh",
                            Email = "pkanh@caothang.edu.vn",
                            PhoneNumber = "0912345678",
                            Gender = true,
                            DateOfBirth = new DateTime(1985, 3, 10),
                            AvatarUrl = "",
                            Status = Status.ACTIVE,
                            PasswordHash = hasher.HashPassword(null, "123456789"),
                            CreatedAt = currentDate,
                            Facutly = "Công Nghệ Thông Tin",
                            Role = Role.TEACHER
                        },
                        new Teacher
                        {
                            Id = Guid.NewGuid(),
                            Username = "qkhai",
                            FullName = "Trần Quang Khải",
                            Email = "tqkhai@caothang.edu.vn",
                            PhoneNumber = "0912345678",
                            Gender = true,
                            DateOfBirth = new DateTime(1985, 3, 10),
                            AvatarUrl = "",
                            Status = Status.ACTIVE,
                            PasswordHash = hasher.HashPassword(null, "123456789"),
                            CreatedAt = currentDate,
                            Facutly = "Công Nghệ Thông Tin",
                            Role = Role.TEACHER
                        }
                    };
                    context.Teachers.AddRange(teachers);
                    context.SaveChanges();
                }

                // Seed 20 Students
                if (!context.Students.Any())
                {
                    var students = new List<Student> {
                        new Student {
                            Id = Guid.NewGuid(),
                            Username = "duy0604",
                            FullName = "Đỗ Đình Duy",
                            Email = "0306221210@caothang.edu.vn",
                            PhoneNumber = "0918557317",
                            Gender = true, // Alternating gender
                            DateOfBirth = new DateTime(2004, 06, 20),
                            AvatarUrl = "",
                            Status = Status.ACTIVE,
                            Facutly = "Công Nghệ Thông Tin",
                            PasswordHash = hasher.HashPassword(null, "123456789"),
                            CreatedAt = currentDate,
                            Role = Role.STUDENT
                        },
                        new Student {
                            Id = Guid.NewGuid(),
                            Username = "pnam",
                            FullName = "Nguyễn Phương Nam",
                            Email = "0306221252@caothang.edu.vn",
                            PhoneNumber = "",
                            Gender = true, // Alternating gender
                            DateOfBirth = new DateTime(2004, 06, 20),
                            AvatarUrl = "",
                            Status = Status.ACTIVE,
                            Facutly = "Công Nghệ Thông Tin",
                            PasswordHash = hasher.HashPassword(null, "123456789"),
                            CreatedAt = currentDate,
                            Role = Role.STUDENT
                        },
                        new Student {
                            Id = Guid.NewGuid(),
                            Username = "ttu",
                            FullName = "Nguyễn Duy Thanh Tú",
                            Email = "0306221250@caothang.edu.vn",
                            PhoneNumber = "",
                            Gender = true, // Alternating gender
                            DateOfBirth = new DateTime(2004, 06, 20),
                            AvatarUrl = "",
                            Status = Status.ACTIVE,
                            Facutly = "Công Nghệ Thông Tin",
                            PasswordHash = hasher.HashPassword(null, "123456789"),
                            CreatedAt = currentDate,
                            Role = Role.STUDENT
                        },
                        new Student {
                            Id = Guid.NewGuid(),
                            Username = "aduy",
                            FullName = "Lương Anh Duy",
                            Email = "0306221211@caothang.edu.vn",
                            PhoneNumber = "",
                            Gender = true, // Alternating gender
                            DateOfBirth = new DateTime(2004, 06, 20),
                            AvatarUrl = "",
                            Status = Status.ACTIVE,
                            Facutly = "Công Nghệ Thông Tin",
                            PasswordHash = hasher.HashPassword(null, "123456789"),
                            CreatedAt = currentDate,
                            Role = Role.STUDENT
                        },
                        new Student {
                            Id = Guid.NewGuid(),
                            Username = "hanh",
                            FullName = "Lê Bá Hoàng Ánh",
                            Email = "0306221202@caothang.edu.vn",
                            PhoneNumber = "",
                            Gender = true, // Alternating gender
                            DateOfBirth = new DateTime(2004, 06, 20),
                            AvatarUrl = "",
                            Status = Status.ACTIVE,
                            Facutly = "Công Nghệ Thông Tin",
                            PasswordHash = hasher.HashPassword(null, "123456789"),
                            CreatedAt = currentDate,
                            Role = Role.STUDENT
                        },
                        new Student {
                            Id = Guid.NewGuid(),
                            Username = "vlinh",
                            FullName = "Nguyễn Văn Linh",
                            Email = "0306221349@caothang.edu.vn",
                            PhoneNumber = "",
                            Gender = true, // Alternating gender
                            DateOfBirth = new DateTime(2004, 06, 20),
                            AvatarUrl = "",
                            Status = Status.ACTIVE,
                            Facutly = "Công Nghệ Thông Tin",
                            PasswordHash = hasher.HashPassword(null, "123456789"),
                            CreatedAt = currentDate,
                            Role = Role.STUDENT
                        },
                        new Student {
                            Id = Guid.NewGuid(),
                            Username = "atu",
                            FullName = "Huỳnh Anh Tú",
                            Email = "0306221260@caothang.edu.vn",
                            PhoneNumber = "",
                            Gender = true, // Alternating gender
                            DateOfBirth = new DateTime(2004, 06, 20),
                            AvatarUrl = "",
                            Status = Status.ACTIVE,
                            Facutly = "Công Nghệ Thông Tin",
                            PasswordHash = hasher.HashPassword(null, "123456789"),
                            CreatedAt = currentDate,
                            Role = Role.STUDENT
                        },
                        new Student {
                            Id = Guid.NewGuid(),
                            Username = "tphat",
                            FullName = "Nguyễn Tấn Phát",
                            Email = "0306221253@caothang.edu.vn",
                            PhoneNumber = "",
                            Gender = true, // Alternating gender
                            DateOfBirth = new DateTime(2004, 06, 20),
                            AvatarUrl = "",
                            Status = Status.ACTIVE,
                            Facutly = "Công Nghệ Thông Tin",
                            PasswordHash = hasher.HashPassword(null, "123456789"),
                            CreatedAt = currentDate,
                            Role = Role.STUDENT
                        },
                        new Student {
                            Id = Guid.NewGuid(),
                            Username = "ntruong",
                            FullName = "Phạm Nhật Trường",
                            Email = "0306221259@caothang.edu.vn",
                            PhoneNumber = "",
                            Gender = true, // Alternating gender
                            DateOfBirth = new DateTime(2004, 06, 20),
                            AvatarUrl = "",
                            Status = Status.ACTIVE,
                            Facutly = "Công Nghệ Thông Tin",
                            PasswordHash = hasher.HashPassword(null, "123456789"),
                            CreatedAt = currentDate,
                            Role = Role.STUDENT
                        },
                        new Student {
                            Id = Guid.NewGuid(),
                            Username = "baki",
                            FullName = "Nguyễn Bá Kiệt",
                            Email = "0306221370@caothang.edu.vn",
                            PhoneNumber = "",
                            Gender = true, // Alternating gender
                            DateOfBirth = new DateTime(2004, 06, 20),
                            AvatarUrl = "",
                            Status = Status.ACTIVE,
                            Facutly = "Công Nghệ Thông Tin",
                            PasswordHash = hasher.HashPassword(null, "123456789"),
                            CreatedAt = currentDate,
                            Role = Role.STUDENT
                        },
                        new Student {
                            Id = Guid.NewGuid(),
                            Username = "kngan",
                            FullName = "Tạ Kiều Ngân",
                            Email = "0306221269@caothang.edu.vn",
                            PhoneNumber = "",
                            Gender = true, // Alternating gender
                            DateOfBirth = new DateTime(2004, 06, 20),
                            AvatarUrl = "",
                            Status = Status.ACTIVE,
                            Facutly = "Công Nghệ Thông Tin",
                            PasswordHash = hasher.HashPassword(null, "123456789"),
                            CreatedAt = currentDate,
                            Role = Role.STUDENT
                        },
                        new Student {
                            Id = Guid.NewGuid(),
                            Username = "ttruc",
                            FullName = "Đặng Phan Thanh Trúc",
                            Email = "0306221268@caothang.edu.vn",
                            PhoneNumber = "",
                            Gender = true, // Alternating gender
                            DateOfBirth = new DateTime(2004, 06, 20),
                            AvatarUrl = "",
                            Status = Status.ACTIVE,
                            Facutly = "Công Nghệ Thông Tin",
                            PasswordHash = hasher.HashPassword(null, "123456789"),
                            CreatedAt = currentDate,
                            Role = Role.STUDENT
                        },
                    };
                   
                    context.Students.AddRange(students);
                    context.SaveChanges();
                }

                if (!context.Subjects.Any())
                {
                    var subjects = new[]
                    {
                        new Subject
                        {
                            Id = Guid.NewGuid(),
                            SubjectCode = "PPLTHDT",
                            Name = "Phương pháp lập trình hướng đối tượng",
                            Status = Status.ACTIVE
                        },
                        new Subject
                        {
                            Id = Guid.NewGuid(),
                            SubjectCode = "PYTHON",
                            Name = "Lập trình Python",
                            Status = Status.ACTIVE
                        },
                        new Subject
                        {
                            Id = Guid.NewGuid(),
                            SubjectCode = "ASPNETCORE",
                            Name = "Lập trình web ASP.NET",
                            Status = Status.ACTIVE
                        },
                        new Subject
                        {
                            Id = Guid.NewGuid(),
                            SubjectCode = "CSDL",
                            Name = "Cơ sở dữ liệu",
                            Status = Status.ACTIVE
                        },
                        new Subject
                        {
                            Id = Guid.NewGuid(),
                            SubjectCode = "JAVA",
                            Name = "Ngôn ngữ lập trình Java",
                            Status = Status.ACTIVE
                        }
                    };
                    context.Subjects.AddRange(subjects);
                    context.SaveChanges();
                }

                var subjectsList = context.Subjects.ToList();

                var teacherId = context.Teachers.First(t => t.Username == "ndduy").Id;
                var studentIds = context.Students.Select(s => s.Id).ToList();

                // Seed CourseClass
                if (!context.CourseClasses.Any())
                {
                    var courseClasses = new List<CourseClass>
                    {
                        new CourseClass
                        {
                            Id = Guid.NewGuid(),
                            ClassCode = "CDTH22WEBC - PPLTHDT",
                            Name = "CDTH22WEBC - PPLTHDT",
                            Description = "Học Kỳ 1 2024 - 2025",
                            Credit = 3,
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "PPLTHDT").Id,
                            UserId = teacherId,
                        },
                        new CourseClass
                        {
                            Id = Guid.NewGuid(),
                            ClassCode = "CDTH22WEBC - PYTHON",
                            Name = "CDTH22WEBC - Lập trình Python",
                            Description = "Học Kỳ 2 2024 - 2025",
                            Credit = 3,
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "PYTHON").Id,
                            UserId = teacherId,
                        },
                        new CourseClass
                        {
                            Id = Guid.NewGuid(),
                            ClassCode = "CDTH22WEBC - CSDL",
                            Name = "CDTH22WEBC - CSDL",
                            Description = "Học Kỳ 1 2024 - 2025",
                            Credit = 3,
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "CSDL").Id,
                            UserId = teacherId,
                        },
                        new CourseClass
                        {
                            Id = Guid.NewGuid(),
                            ClassCode = "CDTH22WEBC - ASPNET",
                            Name = "CDTH22WEBC - Lập trình web ASP.NET",
                            Description = "Học Kỳ 1 2024 - 2025",
                            Credit = 3,
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "ASPNETCORE").Id,
                            UserId = teacherId,
                        },
                        new CourseClass
                        {
                            Id = Guid.NewGuid(),
                            ClassCode = "CDTH22WEBC - JAVA",
                            Name = "CDTH22WEBC - Ngôn ngữ lập trình Java",
                            Description = "Học Kỳ 2 2024 - 2025",
                            Credit = 3,
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "JAVA").Id,
                            UserId = teacherId,
                        },
                    };
                    context.CourseClasses.AddRange(courseClasses);
                    context.SaveChanges();
                }

                var courseClassId = context.CourseClasses.First().Id;

                //Seed Chapters
                if (!context.Chapters.Any())
                {
                    var chapters = new[]
                    {
                        // oop
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 01: Giới thiệu về OOP",
                            Description = "Giới thiệu các khái niệm cơ bản về OOP",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "PPLTHDT").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 02: Lớp và Đối tượng",
                            Description = "Tạo và sử dụng lớp, đối tượng",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "PPLTHDT").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 03: Tính đóng gói (Encapsulation)",
                            Description = "",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "PPLTHDT").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 04: Tính kế thừa (Inheritance)",
                            Description = "",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "PPLTHDT").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 05: Tính đa hình (Polymorphism)",
                            Description = "",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "PPLTHDT").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 06: Tính trừu tượng (Abstraction)",
                            Description = "Tạo và sử dụng lớp, đối tượng",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "PPLTHDT").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 07: Quan hệ giữa các lớp",
                            Description = "Tạo và sử dụng lớp, đối tượng",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "PPLTHDT").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 08: Xử lý ngoại lệ",
                            Description = "Tạo và sử dụng lớp, đối tượng",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "PPLTHDT").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 09: Thiết kế lớp nâng cao",
                            Description = "Tạo và sử dụng lớp, đối tượng",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "PPLTHDT").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 10: OOP trong thực tiễn",
                            Description = "Tạo và sử dụng lớp, đối tượng",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "PPLTHDT").Id
                        },

                        // python
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 01: Giới thiệu Python",
                            Description = "",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "PYTHON").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 02: Biến và kiểu dữ liệu",
                            Description = "",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "PYTHON").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 03: Cấu trúc điều khiển",
                            Description = "",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "PYTHON").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 04: Hàm",
                            Description = "",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "PYTHON").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 05: Cấu trúc dữ liệu",
                            Description = "",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "PYTHON").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 06: Các hướng ứng dụng",
                            Description = "",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "PYTHON").Id
                        },

                        // java
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 01: Giới thiệu về lập trình Java",
                            Description = "Giới thiệu các khái niệm cơ bản về OOP",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "JAVA").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 02: Cấu trúc chương trình Java và các khái niệm cơ bản",
                            Description = "Tạo và sử dụng lớp, đối tượng",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "JAVA").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 03: Câu lệnh điều kiện và lặp",
                            Description = "",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "JAVA").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 04: Mảng (Array) và chuỗi (String)",
                            Description = "",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "JAVA").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 05: Hàm (Phương thức) và Lập trình hướng đối tượng (OOP)",
                            Description = "",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "JAVA").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 06: Tính đóng gói, kế thừa, đa hình",
                            Description = "Tạo và sử dụng lớp, đối tượng",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "JAVA").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 07: Xử lý ngoại lệ (Exception Handling)",
                            Description = "Tạo và sử dụng lớp, đối tượng",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "JAVA").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 08: Lập trình giao diện (GUI) cơ bản với Java Swing",
                            Description = "Tạo và sử dụng lớp, đối tượng",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "JAVA").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 09: Làm việc với File và IO trong Java",
                            Description = "Tạo và sử dụng lớp, đối tượng",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "JAVA").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 10: Các Collection trong Java",
                            Description = "Tạo và sử dụng lớp, đối tượng",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "JAVA").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 11: Lập trình đa luồng (Multithreading)",
                            Description = "Tạo và sử dụng lớp, đối tượng",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "JAVA").Id
                        },
                        new Chapter
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chương 12: Kết nối cơ sở dữ liệu với JDBC",
                            Description = "Tạo và sử dụng lớp, đối tượng",
                            Status = Status.ACTIVE,
                            SubjectId = subjectsList.First(s => s.SubjectCode == "JAVA").Id
                        },
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
                            Name = "Ngân hàng câu hỏi Python",
                            Description = "Bộ câu hỏi về công nghệ thông tin",
                            Status = Status.ACTIVE,
                             SubjectId = subjectsList.First(s => s.SubjectCode == "PYTHON").Id,
                            //CourseClassId = courseClassId
                            TeacherId = teacherId
                        }
                    };
                    context.QuestionBanks.AddRange(questionBanks);
                    context.SaveChanges();
                }

                var questionBankId = context.QuestionBanks.First().Id;

                // Seed Questions
                if (!context.Questions.Any())
                {
                    var chapters = context.Chapters.ToList();

                    // Lấy ID của các chapter quan trọng
                    var oopChapter1 = chapters.First(c => c.Name.Contains("Chương 01") && c.Subject.SubjectCode == "PPLTHDT").Id;
                    var oopChapter2 = chapters.First(c => c.Name.Contains("Chương 02") && c.Subject.SubjectCode == "PPLTHDT").Id;
                    var oopChapter3 = chapters.First(c => c.Name.Contains("Chương 03") && c.Subject.SubjectCode == "PPLTHDT").Id;
                    var oopChapter4 = chapters.First(c => c.Name.Contains("Chương 04") && c.Subject.SubjectCode == "PPLTHDT").Id;
                    var oopChapter5 = chapters.First(c => c.Name.Contains("Chương 05") && c.Subject.SubjectCode == "PPLTHDT").Id;

                    var javaChapter1 = chapters.First(c => c.Name.Contains("Chương 01") && c.Subject.SubjectCode == "JAVA").Id;
                    var javaChapter2 = chapters.First(c => c.Name.Contains("Chương 02") && c.Subject.SubjectCode == "JAVA").Id;
                    var javaChapter5 = chapters.First(c => c.Name.Contains("Chương 05") && c.Subject.SubjectCode == "JAVA").Id;

                    var questionsWithAnswers = new List<(Question Question, List<Answer> Answers)>();

                    // OOP Chapter 1 Questions
                    questionsWithAnswers.Add((
                        new Question
                        {
                            Id = Guid.NewGuid(),
                            Content = "Khái niệm nào dưới đây là đặc trưng của Lập trình hướng đối tượng?",
                            Image = "",
                            CreatedBy = teacherId,

                            Type = TypeOfQuestion.MultipleChoice,

                            Difficulty = Difficulty.EASY,
                            Status = Status.ACTIVE,
                            Topic = "OOP Basics",
                            //QuestionBankId = questionBankId,
                            ChapterId = oopChapter1
                        },
                        new List<Answer>
                        {
                            new Answer { Id = Guid.NewGuid(), Content = "Kế thừa", IsCorrect = true, AnswerOrder = 1, Status = Status.ACTIVE },
                            new Answer { Id = Guid.NewGuid(), Content = "Đóng gói", IsCorrect = true, AnswerOrder = 2, Status = Status.ACTIVE },
                            new Answer { Id = Guid.NewGuid(), Content = "Vòng lặp", IsCorrect = false, AnswerOrder = 3, Status = Status.ACTIVE },
                            new Answer { Id = Guid.NewGuid(), Content = "Hàm toán học", IsCorrect = false, AnswerOrder = 4, Status = Status.ACTIVE }
                        }
                    ));

                    questionsWithAnswers.Add((
                        new Question
                        {
                            Id = Guid.NewGuid(),
                            Content = "Lập trình hướng đối tượng khác với lập trình thủ tục ở điểm nào?",
                            Image = "",
                            CreatedBy = teacherId,
                            Type = TypeOfQuestion.TrueFalse,
                            Difficulty = Difficulty.MEDIUM,
                            Status = Status.ACTIVE,
                            Topic = "OOP Basics",
                            //QuestionBankId = questionBankId,
                            ChapterId = oopChapter1
                        },
                        new List<Answer>
                        {
                            new Answer { Id = Guid.NewGuid(), Content = "Tập trung vào đối tượng và dữ liệu", IsCorrect = true, AnswerOrder = 1, Status = Status.ACTIVE },
                            new Answer { Id = Guid.NewGuid(), Content = "Sử dụng các hàm độc lập", IsCorrect = false, AnswerOrder = 2, Status = Status.ACTIVE },
                            new Answer { Id = Guid.NewGuid(), Content = "Không có sự khác biệt", IsCorrect = false, AnswerOrder = 3, Status = Status.ACTIVE },
                            new Answer { Id = Guid.NewGuid(), Content = "Chỉ khác về cú pháp", IsCorrect = false, AnswerOrder = 4, Status = Status.ACTIVE }
                        }
                    ));

                    // OOP Chapter 2 Questions
                    questionsWithAnswers.Add((
                        new Question
                        {
                            Id = Guid.NewGuid(),
                            Content = "Lớp trong Lập trình hướng đối tượng được định nghĩa như thế nào?",
                            Image = "",
                            CreatedBy = teacherId,

                            Type = TypeOfQuestion.MultipleChoice,

                            Difficulty = Difficulty.MEDIUM,
                            Status = Status.ACTIVE,
                            Topic = "Classes",
                            //QuestionBankId = questionBankId,
                            ChapterId = oopChapter2
                        },
                        new List<Answer>
                        {
                            new Answer { Id = Guid.NewGuid(), Content = "Một bản thiết kế cho đối tượng", IsCorrect = true, AnswerOrder = 1, Status = Status.ACTIVE },
                            new Answer { Id = Guid.NewGuid(), Content = "Một biến số", IsCorrect = false, AnswerOrder = 2, Status = Status.ACTIVE },
                            new Answer { Id = Guid.NewGuid(), Content = "Một hàm", IsCorrect = false, AnswerOrder = 3, Status = Status.ACTIVE },
                            new Answer { Id = Guid.NewGuid(), Content = "Một vòng lặp", IsCorrect = false, AnswerOrder = 4, Status = Status.ACTIVE }
                        }
                    ));

                    questionsWithAnswers.Add((
                        new Question
                        {
                            Id = Guid.NewGuid(),
                            Content = "Đối tượng là gì trong Lập trình hướng đối tượng?",
                            Image = "",
                            CreatedBy = teacherId,

                            Type = TypeOfQuestion.MultipleChoice,

                            Difficulty = Difficulty.EASY,
                            Status = Status.ACTIVE,
                            Topic = "Objects",
                            //QuestionBankId = questionBankId,
                            ChapterId = oopChapter2
                        },
                        new List<Answer>
                        {
                            new Answer { Id = Guid.NewGuid(), Content = "Thực thể được tạo từ lớp", IsCorrect = true, AnswerOrder = 1, Status = Status.ACTIVE },
                            new Answer { Id = Guid.NewGuid(), Content = "Một biến toàn cục", IsCorrect = false, AnswerOrder = 2, Status = Status.ACTIVE },
                            new Answer { Id = Guid.NewGuid(), Content = "Một hàm tĩnh", IsCorrect = false, AnswerOrder = 3, Status = Status.ACTIVE },
                            new Answer { Id = Guid.NewGuid(), Content = "Một mảng", IsCorrect = false, AnswerOrder = 4, Status = Status.ACTIVE }
                        }
                    ));

                    questionsWithAnswers.Add((
                        new Question
                        {
                            Id = Guid.NewGuid(),
                            Content = "Phương thức khởi tạo (constructor) có thể trả về giá trị không?",
                            Image = "",
                            CreatedBy = teacherId,
                            Type = TypeOfQuestion.TrueFalse,
                            Difficulty = Difficulty.EASY,
                            Status = Status.ACTIVE,
                            Topic = "Constructors",
                            //QuestionBankId = questionBankId,
                            ChapterId = oopChapter2
                        },
                        new List<Answer>
                        {
                            new Answer { Id = Guid.NewGuid(), Content = "Không", IsCorrect = true, AnswerOrder = 1, Status = Status.ACTIVE },
                            new Answer { Id = Guid.NewGuid(), Content = "Có", IsCorrect = false, AnswerOrder = 2, Status = Status.ACTIVE }
                        }
                    ));

                    // Thêm các câu hỏi khác tương tự cho các chapter còn lại...

                    // Java Chapter 1 Questions
                    questionsWithAnswers.Add((
                        new Question
                        {
                            Id = Guid.NewGuid(),
                            Content = "Java là ngôn ngữ lập trình thuộc loại nào?",
                            Image = "",
                            CreatedBy = teacherId,
                            Type = TypeOfQuestion.TrueFalse,
                            Difficulty = Difficulty.EASY,
                            Status = Status.ACTIVE,
                            Topic = "Java Basics",
                            //QuestionBankId = questionBankId,
                            ChapterId = javaChapter1
                        },
                        new List<Answer>
                        {
                            new Answer { Id = Guid.NewGuid(), Content = "Hướng đối tượng", IsCorrect = true, AnswerOrder = 1, Status = Status.ACTIVE },
                            new Answer { Id = Guid.NewGuid(), Content = "Thủ tục", IsCorrect = false, AnswerOrder = 2, Status = Status.ACTIVE },
                            new Answer { Id = Guid.NewGuid(), Content = "Hàm", IsCorrect = false, AnswerOrder = 3, Status = Status.ACTIVE },
                            new Answer { Id = Guid.NewGuid(), Content = "Logic", IsCorrect = false, AnswerOrder = 4, Status = Status.ACTIVE }
                        }
                    ));

                    questionsWithAnswers.Add((
                        new Question
                        {
                            Id = Guid.NewGuid(),
                            Content = "JVM là viết tắt của gì?",
                            Image = "",
                            CreatedBy = teacherId,
                            Type = TypeOfQuestion.TrueFalse,
                            Difficulty = Difficulty.EASY,
                            Status = Status.ACTIVE,
                            Topic = "Java Basics",
                            //QuestionBankId = questionBankId,
                            ChapterId = javaChapter1
                        },
                        new List<Answer>
                        {
                            new Answer { Id = Guid.NewGuid(), Content = "Java Virtual Machine", IsCorrect = true, AnswerOrder = 1, Status = Status.ACTIVE },
                            new Answer { Id = Guid.NewGuid(), Content = "Java Variable Method", IsCorrect = false, AnswerOrder = 2, Status = Status.ACTIVE },
                            new Answer { Id = Guid.NewGuid(), Content = "Java Visual Machine", IsCorrect = false, AnswerOrder = 3, Status = Status.ACTIVE },
                            new Answer { Id = Guid.NewGuid(), Content = "Java Virtual Method", IsCorrect = false, AnswerOrder = 4, Status = Status.ACTIVE }
                        }
                    ));

                    // Thêm các câu hỏi cho các chapter khác...

                    // Lưu vào context
                    foreach (var (question, answers) in questionsWithAnswers)
                    {
                        context.Questions.Add(question);
                        foreach (var answer in answers)
                        {
                            answer.QuestionId = question.Id; // Gán QuestionId cho các đáp án
                            context.Answers.Add(answer);
                        }
                    }
                    context.SaveChanges();
                }

                var questionIds = context.Questions.Select(q => q.Id).ToList();

                // Seed RoomExams
                if (!context.RoomExams.Any())
                {
                    var roomExam = new RoomExam
                    {
                        Id = Guid.NewGuid(),
                        Name = "CDTH22WEBC - Thi giữa kỳ Lập trình Python ",
                        StartDate = new DateTime(2025, 7, 4, 10, 30, 0),
                        EndDate = new DateTime(2025, 7, 4, 13, 0, 0),
                        Status = Status.ACTIVE,
                        SubjectId = subjectsList.First(s => s.SubjectCode == "PYTHON").Id,
                        CourseClassId  = courseClassId,

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
                        Name = "Giữa kỳ Lập trình Python",
                        DurationMinutes = 90,
                        NoOfQuestions = 40,
                        RoomExamId = roomExamId,
                        SubjectId = subjectsList.First(s => s.SubjectCode == "PYTHON").Id,
                        UserId = teacherId,
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
                    //var studentExams = studentIds.Select(s => new StudentExam
                    //{
                    //    Id = Guid.NewGuid(),
                    //    StudentId = s,
                    //    ExamId = examId,
                    //    DurationMinutes = 90,
                    //    Status = Status.ACTIVE,
                    //    RoomId = roomExamId
                    //}).ToArray();
                    //context.StudentExams.AddRange(studentExams);
                    //context.SaveChanges();
                }

                var studentExamIds = context.StudentExams.Select(se => se.Id).ToList();
                var answerIds = context.Answers.Select(a => a.Id).ToList();

                // Seed StudentExamDetails (Bài làm của học sinh)
                if (!context.StudentExamDetails.Any())
                {
                    //var studentExamDetails = new List<StudentExamDetail>();
                    //for (int i = 0; i < studentIds.Count; i++)
                    //{
                    //    // Sinh viên trả lời ngẫu nhiên, một số đúng, một số sai
                    //    studentExamDetails.Add(new StudentExamDetail
                    //    {
                    //        AnswerId = i % 2 == 0 ? answerIds[i % 4] : answerIds[(i % 4) + 1], // Chọn câu trả lời ngẫu nhiên
                    //        QuestionId = questionIds[0],
                    //        StudentExamId = studentExamIds[i]
                    //    });
                    //    studentExamDetails.Add(new StudentExamDetail
                    //    {
                    //        AnswerId = i % 2 == 0 ? answerIds[3] : answerIds[4], // Câu 2: Một số đúng, một số sai
                    //        QuestionId = questionIds[1],
                    //        StudentExamId = studentExamIds[i]
                    //    });
                    //    studentExamDetails.Add(new StudentExamDetail
                    //    {
                    //        AnswerId = i % 2 == 0 ? answerIds[5] : answerIds[6], // Câu 3: Một số đúng, một số sai
                    //        QuestionId = questionIds[2],
                    //        StudentExamId = studentExamIds[i]
                    //    });
                    //}
                    //context.StudentExamDetails.AddRange(studentExamDetails);
                    //context.SaveChanges();
                }
            }
        }
    }
}